using Dapper;
using DevLearning.API.Models;

namespace DevLearning.API.Repositories.Interfaces
{
    public interface ICareerItemRepository
    {
        Task CreateCareerItemAsync(CareerItem career);
        Task<bool> GetCareerItemByIdAsync(Guid CareerId, Guid CourseId);
        Task<bool> UpdateCareerItemAsync(Guid CareerId, Guid CourseId, List<string> updates, DynamicParameters parameters);
        Task<bool> DeleteCareerItemAsync(Guid CareerId, Guid CourseId);
        Task<bool> GetCareerItemByTitleAsync(string title);
        Task<bool> ExistingCarrerItemwhitOrder(byte order);
    }


}
