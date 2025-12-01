using DevLearning.API.Models;
using DevLearning.API.Models.DTOs.Category;

namespace DevLearning.API.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        Task<bool> CategoryTitleExistsAsync(string title);
        Task<bool> CategoryTitleExistsForOtherIdAsync(string title, Guid id);
        Task ShiftOrdersAsync(int order);
        Task ShiftOrdersForUpdateAsync(int oldOrder, int newOrder, Guid categoryId);
        Task CreateCategoryAsync(Category category);
        Task<List<CategoryResponseDTO>> GetAllCategoriesAsync();
        Task<Category> GetCategoryByIdAsync(Guid id);
        Task UpdateCategoryAsync(Category category);
        Task<bool> HasCourseAsync(Guid categoryId);
        Task DeleteCategoryAsync(Guid id);
        Task<(string CategoryTitle, List<string> Courses)> GetCategoryCoursesAsync(Guid categoryId);
    }
}
