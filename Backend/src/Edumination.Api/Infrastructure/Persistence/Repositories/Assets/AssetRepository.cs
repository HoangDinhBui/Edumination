using Education.Domain.Entities;
using Education.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Edumination.Api.Infrastructure.Persistence;

namespace Education.Repositories;

public class AssetRepository : IAssetRepository
{
    private readonly AppDbContext _context;

    public AssetRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<long> CreateAssetAsync(Asset asset)
    {
        _ = _context.Assets.Add(asset);
        await _context.SaveChangesAsync();
        return asset.Id;
    }
}