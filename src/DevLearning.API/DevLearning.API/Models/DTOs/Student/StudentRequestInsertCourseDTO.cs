using DevLearning.API.Models.Enums.StudentCourse;
using System.ComponentModel.DataAnnotations;

namespace DevLearning.API.Models.DTOs.Student
{
    public class StudentRequestInsertCourseDTO
    {
        [Range(0, 100)]
        public byte? Progress { get; init; } = 0;
        public FavoriteType Favorite { get; set; }
        public DateTime? StartDate { get; init; } = DateTime.Now;
    }
}
