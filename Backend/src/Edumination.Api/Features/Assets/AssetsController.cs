// Đảm bảo bạn có đủ các using này ở đầu file
using Edumination.Api.Features.Assets.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Edumination.Services.Interfaces;
using System.Threading.Tasks;
using Edumination.Api.Infrastructure.Persistence; 
using Microsoft.AspNetCore.Hosting; 
using Microsoft.EntityFrameworkCore; 
using System.IO; 
using System.Linq; // Cần cho .Select()
using System; // Cần cho Exception

namespace Education.Api.Features.Assets;

[Route("api/v1/assets")]
[ApiController]
public class AssetsController : ControllerBase
{
    // === KHAI BÁO BIẾN ===
    private readonly IAssetService _assetService;
    private readonly AppDbContext _db; // <-- Cần cho Download
    private readonly IWebHostEnvironment _env; // <-- Cần cho Download

    // === BƯỚC 1: SỬA CONSTRUCTOR ===
    // Thêm AppDbContext và IWebHostEnvironment
    public AssetsController(
        IAssetService assetService, 
        AppDbContext db, 
        IWebHostEnvironment env
    )
    {
        _assetService = assetService;
        _db = db;
        _env = env;
    }

    // === HÀM CŨ 1 (Giữ nguyên) ===
    [HttpPost]
    [Authorize(Roles = "TEACHER,ADMIN")]
    public async Task<IActionResult> CreateAsset([FromForm] CreateAssetDto dto)
    {
        if (!new[] { "VIDEO", "AUDIO", "IMAGE", "DOC", "SUBTITLE", "TRANSCRIPT", "OTHER" }.Contains(dto.Kind?.ToUpper()))
        {
            return BadRequest("Invalid kind");
        }
        if (string.IsNullOrEmpty(dto.MediaType) || dto.ByteSize <= 0)
        {
            return BadRequest("Invalid media_type or byte_size");
        }
        var file = Request.Form.Files.GetFile("file");
        if (file == null || file.Length == 0)
        {
            return BadRequest("File is required or size mismatch");
        }
        dto.ByteSize = file.Length;
        if (string.IsNullOrWhiteSpace(dto.MediaType))
            dto.MediaType = file.ContentType;
        var response = await _assetService.CreateAssetAsync(dto, User);
        using var stream = file.OpenReadStream();
        var storageService = HttpContext.RequestServices.GetService<IStorageService>();
        if (storageService == null)
        {
            return StatusCode(500, "Storage service is not available");
        }
        await storageService.SaveFileAsync(response.Asset.StorageUrl, stream);
        return Ok(response);
    }

    // === HÀM CŨ 2 (Giữ nguyên) ===
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsset(long id)
    {
        try
        {
            var userId = long.Parse(User.FindFirst("sub")?.Value ?? "0");
            var userRoles = User.Claims.Where(c => c.Type == "role").Select(c => c.Value).ToArray();
            var result = await _assetService.GetAssetMetadataAndUrlAsync(id, userId, userRoles);
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    // === BƯỚC 2: HÀM MỚI (Sửa lỗi 404) ===
    [HttpGet("download/{id}")]
    public async Task<IActionResult> DownloadAsset(long id)
    {
        var asset = await _db.Assets.FindAsync(id);
        if (asset == null || string.IsNullOrEmpty(asset.StorageUrl))
        {
            return NotFound(new { message = "File not found or StorageUrl is empty." });
        }

        // SỬA LẠI: Thêm thư mục con "uploads" vào đường dẫn
        var contentRootPath = _env.ContentRootPath; // (Đây là /app)
        var uploadRootPath = Path.Combine(contentRootPath, "uploads"); // (Đây là /app/uploads)
        
        var relativePath = asset.StorageUrl.TrimStart('/');
        var physicalPath = Path.Combine(uploadRootPath, relativePath); // <-- Sửa để dùng uploadRootPath

        // (Thêm log để kiểm tra)
        Console.WriteLine($"[DEBUG] Đang cố tải file từ: {physicalPath}");

        if (!System.IO.File.Exists(physicalPath))
        {
            return NotFound(new { message = "File not found on disk.", path = physicalPath });
        }

        var fileStream = new FileStream(physicalPath, FileMode.Open, FileAccess.Read);
        var contentType = asset.MediaType ?? "application/octet-stream";
        return File(fileStream, contentType);
    }
}