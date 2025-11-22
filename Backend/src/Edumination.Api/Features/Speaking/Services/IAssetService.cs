using Edumination.Api.Domain.Entities;
using System.IO;
using System.Threading.Tasks;

namespace Edumination.Api.Features.Speaking
{
    public interface IAssetService
    {
        Task<Asset> UploadAudioAsync(
            Stream stream, 
            string fileName, 
            string contentType, 
            long userId);

        Task<string> GetAssetUrlAsync(long assetId);

        Task DeleteAssetAsync(long assetId);
    }
}