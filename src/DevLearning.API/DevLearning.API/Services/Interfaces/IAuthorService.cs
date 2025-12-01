using DevLearning.API.Models;
using DevLearning.API.Models.DTOs.Author;
using DevLearning.API.Models.Enums.Author;

namespace DevLearning.API.Services.Interfaces
{
    public interface IAuthorService
    {
        Task<List<AuthorResponseDTO>> GetAllAuthorsAsync();
        Task<AuthorResponseDTO> GetAuthorByIdAsync(Guid id);
        Task CreateAuthorAsync(AuthorRequestDTO author);
        Task UpdatePatchAuthorAsync(Guid id, UpdateAuthorParcialDTO dto);
        Task UpdatePutAuthorAsync(Guid id, UpdateAuthorFullDTO dto);
        Task UpdateAuthorTypeAsync(Guid id, AuthorType type);
        Task<AuthorWithCoursesDTO> GetAuthorCoursesAsync(Guid authorId);

    }
}
