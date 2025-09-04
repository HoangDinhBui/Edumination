using Edumination.Api.Dtos;
using Edumination.Api.Domain.Entities;

namespace Edumination.Api.Services.Interfaces
{
    public interface ISectionService
    {
        Task<TestSection> UpdateSectionAsync(long id, UpdateSectionDto dto);
    }
}