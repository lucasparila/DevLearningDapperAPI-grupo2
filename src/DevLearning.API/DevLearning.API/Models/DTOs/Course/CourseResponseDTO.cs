using DevLearning.API.Models.Enums.Course;

namespace DevLearning.API.Models.DTOs.Course
{
    public class CourseResponseDTO
    {
        public Guid CourseId { get; init; }
        public string Tag { get; init; }
        public string Title { get; init; }
        public string Summary { get; init; }
        public string Url { get; init; }
        public CourseLevel Level { get; init; }
        public string LevelLabel => Level.ToString();
        public int DurationInMinutes { get; init; }
        public DateTime CreateDate { get; init; }
        public DateTime LastUpdateDate { get; init; }
        public bool Active { get; init; }
        public bool Free { get; init; }
        public bool Featured { get; init; }
        public string AuthorName { get; init; }
        public string CategoryName { get; init; }
        public string Tags { get; init; }
    }
}
