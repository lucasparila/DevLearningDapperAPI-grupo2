using DevLearning.API.Models.Enums.Author;

namespace DevLearning.API.Models.DTOs.Author
{
    public class AuthorResponseDTO
    {
        public Guid AuthorId { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public string Bio { get; set; }
        public string Url { get; set; }
        public string Email { get; set; }
        public string TypeLabel => Type.ToString();
        public AuthorType Type { get; set; }

 
    }
}
