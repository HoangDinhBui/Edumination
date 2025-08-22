using Education.Domain.Entities;

namespace Education.Repositories.Interfaces;

public interface IAssetRepository
{
    Task<long> CreateAssetAsync(Asset asset);
}