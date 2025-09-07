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
            // Kiểm tra section có tồn tại không
            var section = await _sectionRepository.GetByIdAsync(sectionId);
            if (section == null)
            {
                throw new KeyNotFoundException($"Section với ID {sectionId} không được tìm thấy.");
            }

            // Kiểm tra trùng lặp vị trí
            var existingPassage = await _passageRepository.GetBySectionIdAndPositionAsync(sectionId, dto.Position);
            if (existingPassage != null)
            {
                throw new InvalidOperationException($"Đã có passage tại vị trí {dto.Position} cho section {sectionId}.");
            }

            // Tạo passage mới
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
    }
}