using DevLearning.API.Models.Enums.Author;
using System.Text.Json.Serialization;

namespace DevLearning.API.Models
{
    public class Author
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Title { get; private set; }
        public string Image {  get; private set; }
        public string Bio { get; private set; }
        public string Url { get; private set; }
        public string Email { get; private set; }
        public AuthorType Type { get; private set; } //Author ativo ou inativo
        public List<Course> Courses { get; set; } = [];

        [JsonConstructor]
        public Author(string name, string title, string image, string bio, string email)
        {
            Id = Guid.NewGuid();
            Name = name;
            Title = title;
            Image = image;
            Bio = bio;
            Url = $"www.devlearning.com.br/author/{Name.ToLower().Replace(" ", "-")}";
            Email = email;
            Type = AuthorType.Ativo;
        }

       
    }
}
