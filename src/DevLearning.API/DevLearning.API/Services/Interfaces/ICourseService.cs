using DevLearning.API.Models;
using DevLearning.API.Models.DTOs.Course;

namespace DevLearning.API.Services.Interfaces
{
    public interface ICourseService
    {
        Task CreateCourseAsync(CourseRequestDTO course);

        Task<List<CourseResponseDTO>> GetAllCoursesAsync(string category);

        Task<CourseResponseDTO> GetOneCourseByTitleAsync(string title);
        Task<CourseResponseDTO> GetOneCourseByIdAsync(string id);

        Task<CourseResponseDTO> DeleteCourseByTitleAsync(string title);

        Task UpdateCourseByTitleAsync(string title, CourseUpdateDTO update);
    }
}
