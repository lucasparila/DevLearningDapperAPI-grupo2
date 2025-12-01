using Dapper;
using DevLearning.API.Models;
using DevLearning.API.Models.DTOs.Carrer;

namespace DevLearning.API.Repositories.Interfaces
{
    public interface ICareerRepository
    {
        Task<List<CareerResponseDTO>> GetAllCareersAsync();
        Task<CareerResponseDTO?> GetCareerByIdAsync(Guid careerId);
        Task CreateCareerAsync(Career career);
        Task<bool> DeleteCareerAsync(Guid careerId);
        Task<bool> UpdateCareerAsync(Guid id, List<string> updates, DynamicParameters parameters);
        Task<List<CareerWhitCareerItemResponseDTO>> GetAllCareerWithCareerItem();
        Task<CareerWhitCareerItemResponseDTO> GetOneCareerWithCareerItem(Guid careerId);
        Task<bool> GetCareerByTitleAsync(string Title);
    }
}
