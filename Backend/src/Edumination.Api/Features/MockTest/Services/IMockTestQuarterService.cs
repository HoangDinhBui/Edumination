using Edumination.Api.Features.MockTest.Dtos;

namespace Edumination.Api.Features.MockTest.Services;

public interface IMockTestQuarterService
    {
        Task<MockTestQuarter?> GetByIdAsync(long id);
    }