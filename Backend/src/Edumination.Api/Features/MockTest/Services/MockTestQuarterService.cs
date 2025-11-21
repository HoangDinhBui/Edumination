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
            return await _context.MockTestQuarters
                                 .AsNoTracking()   // không track để tối ưu đọc
                                 .FirstOrDefaultAsync(m => m.Id == id);
        }
    }