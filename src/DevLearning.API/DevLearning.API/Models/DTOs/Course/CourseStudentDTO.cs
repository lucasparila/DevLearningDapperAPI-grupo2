using DevLearning.API.Models.Enums.Course;

namespace DevLearning.API.Models.DTOs.Course
{
    public class CourseStudentDTO
    {
        public Guid CourseId { get; init; }
        public string Title { get; init; }
        public string Summary { get; init; }
        public string Url { get; init; }
        public CourseLevel Level { get; init; }
        public byte Progress { get; set; }
        public string LevelLabel => Level.ToString();
        public int DurationInMinutes { get; init; }
    }
}
