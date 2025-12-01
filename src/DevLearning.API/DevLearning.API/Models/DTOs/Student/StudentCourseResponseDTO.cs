using DevLearning.API.Models.DTOs.Course;
using DevLearning.API.Models.Enums.StudentCourse;
using System.ComponentModel.DataAnnotations;

namespace DevLearning.API.Models.DTOs.Student
{
    public class StudentCourseResponseDTO
    {
        [Range(0, 100)]
        public byte? Progress { get; init; } = 0;
        public FavoriteType Favorite { get; set; }
        public DateTime? StartDate { get; init; } = DateTime.Now;
        public DateTime? LastUpdateDate { get; set; }
        public StudentResponseDTO Student { get; set; }
        public CourseStudentDTO Course { get; set; }
    }
}
