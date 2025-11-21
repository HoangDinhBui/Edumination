using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;
using Stripe.Checkout;
using System.Security.Claims;
using Edumination.Api.Infrastructure.Persistence;
using Edumination.Api.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using Edumination.Api.Domain.Entities.Orders;
using EnrollEntity = Edumination.Api.Domain.Entities.Enrollment;

namespace Edumination.Api.Features.Checkout;

[ApiController]
[Route("api/v1")]
public class CheckoutController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IConfiguration _cfg;
    private readonly IStripeClient _stripe;
    public CheckoutController(AppDbContext db, IConfiguration cfg, IStripeClient stripe)
    {
        _db = db;
        _cfg = cfg;
        _stripe = stripe;
    }

    public record CheckoutRequest(string Provider = "STRIPE", string? ReturnUrl = null, string? CancelUrl = null);
    private long? GetUserId() =>
        long.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier) ??
                      User.FindFirstValue(JwtRegisteredClaimNames.Sub), out var id) ? id : null;

    // POST /api/v1/checkout/courses/{courseId}
    [HttpPost("checkout/courses/{courseId:long}")]
    [Authorize]
    public async Task<IActionResult> CreateCheckout([FromRoute] long courseId, [FromBody] CheckoutRequest req, CancellationToken ct)
    {
        var userId = GetUserId();
        if (userId is null) return Unauthorized(new { error = "Invalid token" });

        // 1) Course + publish
        var course = await _db.Courses.AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == courseId && c.IsPublished, ct);
        if (course is null) return NotFound(new { error = "Course not found or not published." });

        // 2) Price
        var price = await _db.CoursePrices.AsNoTracking()
            .FirstOrDefaultAsync(p => p.CourseId == courseId && p.IsActive, ct);
        if (price is null) return BadRequest(new { error = "Price not configured." });

        // 3) Already enrolled or free -> enroll ngay
        var already = await _db.Enrollments.AsNoTracking()
            .AnyAsync(e => e.UserId == userId && e.CourseId == courseId, ct);

        if (already || price.PriceVnd == 0)
        {
            if (!already)
            {
                _db.Enrollments.Add(new EnrollEntity { UserId = userId.Value, CourseId = courseId });
                await _db.SaveChangesAsync(ct);
            }
            return Ok(new { enrolled = true });
        }

        // 4) Close pending cũ nếu không phải cùng course
        var open = await _db.Orders.Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.UserId == userId && o.Status == OrderStatus.PENDING, ct);

        if (open != null && !open.Items.Any(i => i.CourseId == courseId))
        {
            open.Status = OrderStatus.CANCELLED;
            await _db.SaveChangesAsync(ct);
            open = null;
        }

        // 5) Create order + item
        var order = open ?? new Edumination.Api.Domain.Entities.Order
        {
            UserId = userId.Value,
            TotalVnd = price.PriceVnd,
            Status = OrderStatus.PENDING,
            CreatedAt = DateTime.UtcNow
        };
        if (open is null)
        {
            _db.Orders.Add(order);
            await _db.SaveChangesAsync(ct);
        }

        if (!order.Items.Any(i => i.CourseId == courseId))
        {
            _db.OrderItems.Add(new OrderItem
            {
                OrderId = order.Id,
                CourseId = courseId,
                UnitVnd = price.PriceVnd,
                Qty = 1
            });
            order.TotalVnd = price.PriceVnd;
            await _db.SaveChangesAsync(ct);
        }

        // 6) Create payment row
        var payment = new Payment
        {
            OrderId = order.Id,
            Provider = PaymentProvider.STRIPE,
            AmountVnd = price.PriceVnd,
            Status = PaymentStatus.INIT,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        _db.Payments.Add(payment);
        await _db.SaveChangesAsync(ct);

        // 7) Stripe Checkout Session (dùng StripeClient đã inject)
        // var successUrl = string.IsNullOrWhiteSpace(req.ReturnUrl)
        //     ? "http://localhost:8081/orders/thanks?sessionId={CHECKOUT_SESSION_ID}"
        //     : req.ReturnUrl + "?sessionId={CHECKOUT_SESSION_ID}";
        var successUrl = string.IsNullOrWhiteSpace(req.ReturnUrl)
            ? "http://localhost:8082/thanks"
            : req.ReturnUrl;
        var cancelUrl = string.IsNullOrWhiteSpace(req.CancelUrl)
            ? "http://localhost:8081/orders/cancel"
            : req.CancelUrl;

        var sessionOptions = new SessionCreateOptions
        {
            Mode = "payment",
            SuccessUrl = successUrl,
            CancelUrl = cancelUrl,
            LineItems =
            [
                new SessionLineItemOptions
                {
                    Quantity = 1,
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "vnd",                 // VND là zero-decimal
                        UnitAmount = price.PriceVnd,      // int
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = course.Title
                        }
                    }
                }
            ],
            Metadata = new Dictionary<string, string>
            {
                ["order_id"] = order.Id.ToString(),
                ["payment_id"] = payment.Id.ToString(),
                ["user_id"] = userId.Value.ToString(),
                ["course_id"] = courseId.ToString()
            }
        };

        var sessionService = new SessionService(_stripe);
        var session = await sessionService.CreateAsync(sessionOptions, cancellationToken: ct);

        payment.ProviderTxnId = session.Id;
        await _db.SaveChangesAsync(ct);

        return Ok(new
        {
            orderId = order.Id,
            paymentId = payment.Id,
            amountVnd = price.PriceVnd,
            provider = "STRIPE",
            checkoutUrl = session.Url
        });
    }


    // POST /api/v1/payments/webhooks/stripe
    [HttpPost("payments/webhooks/stripe")]
    [AllowAnonymous]
    public async Task<IActionResult> StripeWebhook(CancellationToken ct)
    {
        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync(ct);
        var sig = Request.Headers["Stripe-Signature"].ToString();
        var secret = _cfg["Stripe:WebhookSecret"]; // nạp từ env/dotenv/docker

        Console.WriteLine($"[WEBHOOK DEBUG] Full signature header: '{sig}'");  // Xem header có rỗng không
        Console.WriteLine($"[WEBHOOK DEBUG] Loaded secret prefix: {secret?.Substring(0, 10)}...");  // Xem secret load đúng
        Console.WriteLine($"[WEBHOOK DEBUG] Body length: {json.Length} chars");  // Xem body đến

        Event stripeEvent;
        try
        {
            stripeEvent = EventUtility.ConstructEvent(
                json,
                sig,
                secret,
                throwOnApiVersionMismatch: false
            );
            Console.WriteLine($"[WEBHOOK SUCCESS] Parsed event type: {stripeEvent.Type} (ID: {stripeEvent.Id})");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[WEBHOOK ERROR] Exception: {ex.Message}");  // Chi tiết lỗi
            return BadRequest(new { error = "Invalid signature", detail = ex.Message });
        }

        // Xử lý cả 2 luồng: Checkout Session & Payment Intent
        switch (stripeEvent.Type)
        {
            case EventTypes.CheckoutSessionCompleted:
                {
                    var session = (Session)stripeEvent.Data.Object;
                    var meta = session.Metadata ?? new Dictionary<string, string>();
                    if (!meta.TryGetValue("order_id", out var orderIdStr) ||
                        !meta.TryGetValue("payment_id", out var paymentIdStr) ||
                        !long.TryParse(orderIdStr, out var orderId) ||
                        !long.TryParse(paymentIdStr, out var paymentId))
                        return Ok(); // bỏ qua nếu không khớp

                    var order = await _db.Orders.FirstOrDefaultAsync(o => o.Id == orderId, ct);
                    var payment = await _db.Payments.FirstOrDefaultAsync(p => p.Id == paymentId && p.OrderId == orderId, ct);
                    if (order is null || payment is null) return Ok();

                    if (order.Status == OrderStatus.PAID && payment.Status == PaymentStatus.SUCCESS) return Ok(); // idempotent

                    payment.Status = PaymentStatus.SUCCESS;
                    payment.ProviderTxnId = session.PaymentIntentId ?? session.Id;
                    payment.UpdatedAt = DateTime.UtcNow;

                    order.Status = OrderStatus.PAID;
                    order.PaidAt = DateTime.UtcNow;

                    await _db.SaveChangesAsync(ct);

                    if (meta.TryGetValue("user_id", out var uid) && long.TryParse(uid, out var userId) &&
                        meta.TryGetValue("course_id", out var cid) && long.TryParse(cid, out var courseId))
                    {
                        var exists = await _db.Enrollments
                            .AnyAsync(e => e.UserId == userId && e.CourseId == courseId, ct);
                        if (!exists)
                        {
                            _db.Enrollments.Add(new EnrollEntity { UserId = userId, CourseId = courseId });
                            await _db.SaveChangesAsync(ct);
                        }
                    }

                    // Redirect to home page after successful payment
                    return Redirect("/");
                }

            case EventTypes.PaymentIntentSucceeded:
                {
                    // Nếu bạn đi theo flow PaymentIntent trực tiếp (không Checkout Session),
                    // có thể lưu intent.Id => tìm Payment tương ứng và mark SUCCESS.
                    // Ở flow hiện tại dùng Checkout Session, case này có thể bỏ qua.
                    break;
                }

            case EventTypes.PaymentIntentPaymentFailed:
                {
                    // Nếu cần, đánh dấu FAILED
                    break;
                }
        }

        return Ok();
    }
}
