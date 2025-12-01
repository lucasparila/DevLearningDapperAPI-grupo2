using System.Reflection.Metadata.Ecma335;
using System.Text.Json.Serialization;
using DevLearning.API.Models.Enums.Course;

namespace DevLearning.API.Models
{
    public class Course
    {
        public Guid Id { get; private set; }
        public string Tag { get; private set; }
        public string Title { get; private set; }
        public string Summary { get; private set; }
        public string Url { get; private set; }
        public CourseLevel Level { get; private set; }
        public int DurationInMinutes { get; private set; }
        public DateTime CreateDate { get; private set; }
        public DateTime LastUpdateDate { get; private set; }
        public bool Active { get; private set; }
        public bool Free { get; private set; }
        public bool Featured { get; private set; }
        public Guid AuthorId { get; private set; }
        public Guid CategoryId { get; private set; }
        public string Tags { get; private set; }

        [JsonConstructor]
        public Course() { }

        [JsonConstructor]
        public Course(
            Guid id,
            string tag,
            string title,
            string summary,
            string url,
            CourseLevel level,
            int durationInMinutes,
            DateTime createDate,
            DateTime lastUpdateDate,
            bool active,
            bool free,
            bool featured,
            Guid authorId,
            Guid categoryId,
            string tags)
        {
            Id = id;
            Tag = tag;
            Title = title;
            Summary = summary;
            Url = url;
            Level = level;
            DurationInMinutes = durationInMinutes;
            CreateDate = createDate;
            LastUpdateDate = lastUpdateDate;
            Active = active;
            Free = free;
            Featured = featured;
            AuthorId = authorId;
            CategoryId = categoryId;
            Tags = tags;
        }

        //[JsonConstructor]
        //public Course(
        //    string tag,
        //    string title,
        //    string summary,
        //    string url,
        //    CourseLevel level,
        //    int durationInMinutes,
        //    Guid authorId,
        //    Guid categoryId,
        //    string tags)
        //{
        //    Id = Guid.NewGuid();
        //    Tag = tag;
        //    Title = title;
        //    Summary = summary;
        //    Url = url;
        //    Level = level;
        //    DurationInMinutes = durationInMinutes;
        //    CreateDate = DateTime.UtcNow;
        //    LastUpdateDate = DateTime.UtcNow;
        //    Active = true;
        //    Free = false;
        //    Featured = false;
        //    AuthorId = authorId;
        //    CategoryId = categoryId;
        //    Tags = tags;
        //}
    }
}
