// Edumination.Api.Services.Interfaces/IPassageService.cs
using Edumination.Api.Domain.Entities;
using Edumination.Api.Dtos;
using Edumination.Domain.Entities;
using System.Threading.Tasks;

namespace Edumination.Api.Services.Interfaces
{
    public interface IPassageService
    {
        Task<Passage> CreatePassageAsync(long sectionId, PassageCreateDto dto);
        Task<Passage> UpdatePassageAsync(long id, PassageUpdateDto dto);
        Task DeletePassageAsync(long id);
    }
}