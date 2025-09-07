// Edumination.Api.Services/PassageService.cs
using Edumination.Api.Domain.Entities;
using Edumination.Api.Dtos;
using Edumination.Api.Repositories.Interfaces;
using Edumination.Api.Services.Interfaces;
using Edumination.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Edumination.Api.Services
{
    public class PassageService : IPassageService
    {
        private readonly IPassageRepository _passageRepository;
        private readonly ISectionRepository _sectionRepository;

        public PassageService(IPassageRepository passageRepository, ISectionRepository sectionRepository)
        {
            _passageRepository = passageRepository;
            _sectionRepository = sectionRepository;
        }

        public async Task<Passage> CreatePassageAsync(long sectionId, PassageCreateDto dto)
        {
            var section = await _sectionRepository.GetByIdAsync(sectionId);
            if (section == null)
            {
                throw new KeyNotFoundException($"Section with ID {sectionId} not found.");
            }

            var existingPassage = await _passageRepository.GetBySectionIdAndPositionAsync(sectionId, dto.Position);
            if (existingPassage != null)
            {
                throw new InvalidOperationException($"A passage already exists at position {dto.Position} for section {sectionId}.");
            }

            var passage = new Passage
            {
                SectionId = sectionId,
                Title = dto.Title,
                ContentText = dto.ContentText,
                AudioId = dto.AudioId,
                TranscriptId = dto.TranscriptId,
                Position = dto.Position,
                CreatedAt = DateTime.UtcNow
            };

            return await _passageRepository.CreateAsync(passage);
        }

        public async Task<Passage> UpdatePassageAsync(long id, PassageUpdateDto dto)
        {
            var passage = await _passageRepository.GetByIdAsync(id);
            if (passage == null)
            {
                throw new KeyNotFoundException($"Passage with ID {id} not found.");
            }

            if (dto.Title != null) passage.Title = dto.Title;
            if (dto.ContentText != null) passage.ContentText = dto.ContentText;
            if (dto.AudioId.HasValue) passage.AudioId = dto.AudioId;
            if (dto.TranscriptId.HasValue) passage.TranscriptId = dto.TranscriptId;
            if (dto.Position.HasValue)
            {
                var existingPassage = await _passageRepository.GetBySectionIdAndPositionAsync(passage.SectionId, dto.Position.Value);
                if (existingPassage != null && existingPassage.Id != id)
                {
                    throw new InvalidOperationException($"A passage already exists at position {dto.Position.Value} for section {passage.SectionId}.");
                }
                passage.Position = dto.Position.Value;
            }

            await _passageRepository.UpdateAsync(passage);
            return passage;
        }

        public async Task DeletePassageAsync(long id)
        {
            var passage = await _passageRepository.GetByIdAsync(id);
            if (passage == null)
            {
                throw new KeyNotFoundException($"Passage with ID {id} not found.");
            }
            await _passageRepository.DeleteAsync(id);
        }
    }
}