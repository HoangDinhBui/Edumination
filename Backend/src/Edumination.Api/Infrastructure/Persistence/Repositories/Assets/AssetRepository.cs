using Education.Domain.Entities;
using Education.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Edumination.Api.Infrastructure.Persistence;
using System.Collections.Generic;
using System.Threading.Tasks;
using Edumination.Api.Domain.Entities;

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
        _context.Assets.Add(asset);
        await _context.SaveChangesAsync();
        return asset.Id;
    }

    public async Task<Asset> GetAssetByIdAsync(long assetId)
    {
        return await _context.Assets.FindAsync(assetId);
    }

    public async Task<List<TestPaper>> GetRelatedTestPapersAsync(long assetId)
    {
        return await _context.TestPapers
            .Where(tp => tp.PdfAssetId == assetId || tp.TestSections.Any(ts => ts.AudioAssetId == assetId))
            .Include(tp => tp.TestSections) // Đảm bảo tải TestSections
            .ToListAsync();
    }
}