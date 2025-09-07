// Edumination.Api.Repositories.Interfaces/IPassageRepository.cs
using Edumination.Api.Domain.Entities;
using Edumination.Domain.Entities;
using System.Threading.Tasks;

namespace Edumination.Api.Repositories.Interfaces
{
    public interface IPassageRepository
    {
        Task<Passage> CreateAsync(Passage passage);
        Task<Passage> GetByIdAsync(long id);
        Task<Passage> GetBySectionIdAndPositionAsync(long sectionId, int position);
        Task<Passage> UpdateAsync(Passage passage);
        Task DeleteAsync(long id);
    }
}