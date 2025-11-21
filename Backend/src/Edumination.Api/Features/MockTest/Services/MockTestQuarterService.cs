using Edumination.Api.Features.MockTest.Dtos;
using Edumination.Api.Features.MockTest.Services;
using Edumination.Api.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Edumination.Api.Features.MockTest.Services;

public class MockTestQuarterService : IMockTestQuarterService
{
    private readonly AppDbContext _context;

    public MockTestQuarterService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<MockTestQuarter?> GetByIdAsync(long id)
    {
        var entity = await _context.MockTestQuarters
                                .AsNoTracking()
                                .FirstOrDefaultAsync(m => m.Id == id);

        if (entity == null)
            return null;

        // Ánh xạ từ Entities.MockTestQuarter sang Dtos.MockTestQuarter
        return new MockTestQuarter
        {
            Id = entity.Id,
            MockTestId = entity.MockTestId,
            Quarter = entity.Quarter,
            SetNumber = (byte)entity.SetNumber,
            ListeningPaperId = entity.ListeningPaperId,
            ReadingPaperId = entity.ReadingPaperId,
            WritingPaperId = entity.WritingPaperId,
            SpeakingPaperId = entity.SpeakingPaperId,
            Status = entity.Status,
            CreatedAt = entity.CreatedAt
        };
    }
}