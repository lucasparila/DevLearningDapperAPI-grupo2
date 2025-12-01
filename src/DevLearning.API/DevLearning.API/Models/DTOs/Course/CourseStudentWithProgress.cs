using DevLearning.API.Models.Enums.Course;

namespace DevLearning.API.Models.DTOs.Course
{
    public class CourseStudentWithProgress
    {
        public Guid CourseId { get; init; }
        public string Title { get; init; }
        public string Summary { get; init; }
        public string Url { get; init; }
        public byte Progress { get; set; }
        public CourseLevel Level { get; init; }
        public string LevelLabel => Level.ToString();
        public int DurationInMinutes { get; init; }
    }
}
