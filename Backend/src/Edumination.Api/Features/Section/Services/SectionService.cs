using Edumination.Api.Dtos;
using Edumination.Api.Domain.Entities;
using Edumination.Api.Repositories.Interfaces;
using Edumination.Api.Services.Interfaces;

namespace Edumination.Api.Services
{
    public class SectionService : ISectionService
    {
        private readonly ISectionRepository _sectionRepository;

        public SectionService(ISectionRepository sectionRepository)
        {
            _sectionRepository = sectionRepository;
        }

        public async Task<TestSection> UpdateSectionAsync(long id, UpdateSectionDto dto)
        {
            var section = await _sectionRepository.GetByIdAsync(id);
            if (section == null)
            {
                throw new KeyNotFoundException($"Section with ID {id} not found.");
            }

            if (dto.TimeLimitSec.HasValue)
            {
                section.TimeLimitSec = dto.TimeLimitSec.Value;
            }

            if (dto.AudioAssetId.HasValue)
            {
                // Optional: Validate if asset exists (bỏ qua nếu bạn muốn kiểm tra ở client)
                section.AudioAssetId = dto.AudioAssetId.Value;
            }

            if (dto.IsPublished.HasValue)
            {
                section.IsPublished = dto.IsPublished.Value;
            }

            await _sectionRepository.UpdateAsync(section);
            return section;
        }
    }
}