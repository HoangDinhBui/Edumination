using Edumination.Api.Domain.Entities;
using Education.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Education.Repositories.Interfaces;

public interface IAssetRepository
{
    Task<long> CreateAssetAsync(Asset asset);
    Task<Asset> GetAssetByIdAsync(long assetId);
    Task<List<TestPaper>> GetRelatedTestPapersAsync(long assetId); // Định nghĩa rõ ràng
}