using DevLearning.API.Models;
using DevLearning.API.Models.DTOs.Course;

namespace DevLearning.API.Repositories.Interfaces
{
    public interface ICourseRepository
    {
        Task CreateCourseAsync(Course course);

        Task<List<CourseResponseDTO>> GetAllCoursesAsync(string category);

        Task<CourseResponseDTO> GetOneCourseByIdAsync(Guid id);
        Task<CourseResponseDTO> GetOneCourseByTitleAsync(string title);

        Task<CourseResponseDTO> DeleteCourseByTitleAsync(string title);

        Task UpdateCourseByTitleAsync(string title, bool free, bool featured, DateTime lastUpdateDate);

        Task UpdateActiveCourseByTitleAsync(string title, bool active, DateTime lastUpdateDate);
    }
}
