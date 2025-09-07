// Edumination.Api.Infrastructure.Persistence/Repositories/ExerciseRepository.cs
using Edumination.Api.Domain.Entities;
using Edumination.Api.Infrastructure.Persistence;
using Edumination.Api.Repositories.Interfaces;
using Edumination.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Edumination.Api.Infrastructure.Persistence.Repositories
{
    public class ExerciseRepository : IExerciseRepository
    {
        private readonly AppDbContext _context;

        public ExerciseRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Exercise> GetByIdAsync(long id)
        {
            return await _context.Exercises.FindAsync(id);
        }
    }
}