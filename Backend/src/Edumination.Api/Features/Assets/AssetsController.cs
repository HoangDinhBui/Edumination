using Edumination.Api.Features.Assets.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Edumination.Services.Interfaces;
using System.Threading.Tasks;

namespace Education.Api.Features.Assets;

[Route("api/v1/assets")]
[ApiController]
public class AssetsController : ControllerBase
{
    private readonly IAssetService _assetService;

    public AssetsController(IAssetService assetService)
    {
        _assetService = assetService;
    }

    [HttpPost]
    [Authorize(Roles = "TEACHER,ADMIN")] // Chỉ TEACHER và ADMIN tạo asset
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

        // Kiểm tra và lấy file từ form-data
        var file = Request.Form.Files.GetFile("file");
        if (file == null || file.Length == 0)
        {
            return BadRequest("File is required or size mismatch");
        }

        dto.ByteSize = file.Length;
        if (string.IsNullOrWhiteSpace(dto.MediaType))
            dto.MediaType = file.ContentType;

        // Gọi service để tạo metadata và lưu file
        var response = await _assetService.CreateAssetAsync(dto, User);

        // Lưu file vào storage
        using var stream = file.OpenReadStream();
        var storageService = HttpContext.RequestServices.GetService<IStorageService>();
        if (storageService == null)
        {
            return StatusCode(500, "Storage service is not available");
        }
        await storageService.SaveFileAsync(response.Asset.StorageUrl, stream);

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsset(long id)
    {
        try
        {
            var userId = long.Parse(User.FindFirst("sub")?.Value ?? "0"); // Lấy userId từ claim 'sub'
            var userRoles = User.Claims.Where(c => c.Type == "role").Select(c => c.Value).ToArray(); // Lấy roles

            // Gọi service để lấy metadata và URL
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
}