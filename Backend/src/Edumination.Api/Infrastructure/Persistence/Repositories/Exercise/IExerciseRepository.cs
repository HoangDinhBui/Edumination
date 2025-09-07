// Edumination.Api.Repositories.Interfaces/IExerciseRepository.cs
using Edumination.Api.Domain.Entities;
using Edumination.Domain.Entities;
using System.Threading.Tasks;

namespace Edumination.Api.Repositories.Interfaces
{
    public interface IExerciseRepository
    {
        Task<Exercise> GetByIdAsync(long id);
    }
}