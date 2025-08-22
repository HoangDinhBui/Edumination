using Edumination.Api.Features.Assets.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Edumination.Services.Interfaces;
using System.Threading.Tasks;

namespace Education.Api.Features.Assets;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "TEACHER,ADMIN")]
public class AssetsController : ControllerBase
{
    private readonly IAssetService _assetService;

    public AssetsController(IAssetService assetService)
    {
        _assetService = assetService;
    }

    [HttpPost]
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
        if (file == null || file.Length != dto.ByteSize)
        {
            return BadRequest("File is required or size mismatch");
        }

        // Gọi service để tạo metadata và lưu file
        var response = await _assetService.CreateAssetAsync(dto, User.Identity.Name);

        // Lưu file vào storage
        using var stream = file.OpenReadStream();
        var storageService = HttpContext.RequestServices.GetService<IStorageService>();
        await storageService.SaveFileAsync(response.Asset.StorageUrl, stream);

        return Ok(response);
    }
}