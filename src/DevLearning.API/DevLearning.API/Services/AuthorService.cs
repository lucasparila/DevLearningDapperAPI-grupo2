using Azure;
using DevLearning.API.Models;
using DevLearning.API.Models.DTOs.Author;
using DevLearning.API.Models.Enums.Author;
using DevLearning.API.Repositories;
using DevLearning.API.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;

namespace DevLearning.API.Services
{
    public class AuthorService : IAuthorService
    {
        private AuthorRepository _authorRepository;
        public AuthorService(AuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }
        public async Task<List<AuthorResponseDTO>> GetAllAuthorsAsync()
        {
            return await _authorRepository.GetAllAuthorsAsync();
        }
        public async Task<AuthorResponseDTO> GetAuthorByIdAsync(Guid id)
        {
            return await _authorRepository.GetAuthorByIdAsync(id);
        }

        public async Task CreateAuthorAsync(AuthorRequestDTO author)
        {
            try
            {
                var findAuthor = await _authorRepository.GetAuthorByEmail(author.Email);
                if (findAuthor is null)
                {
                    var newAuthor = new Author(author.Name, author.Title, author.Image, author.Bio, author.Email);
                    await _authorRepository.CreateAuthorAsync(newAuthor);

                }
                else
                {
                    throw new Exception("O autor já existe.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task UpdatePatchAuthorAsync(Guid id, UpdateAuthorParcialDTO dto)
        {
            await _authorRepository.UpdatePatchAuthorAsync(dto, id);
        }
        public async Task UpdatePutAuthorAsync(Guid id, UpdateAuthorFullDTO dto)
        {
            await _authorRepository.UpdatePutAuthorAsync(dto, id);
        }

        public async Task UpdateAuthorTypeAsync(Guid id, AuthorType newType)
        {
            // valida enum
            if (!Enum.IsDefined(typeof(AuthorType), newType))
                throw new ArgumentOutOfRangeException(nameof(newType), "Tipo inválido. Use 1 ou 2.");

            // regra: se inativo (2), precisa verificar cursos
            if (newType == AuthorType.Inativo)
            {
                var count = await _authorRepository.CountCoursesAsync(id);
                if (count > 0)
                    throw new InvalidOperationException("Não é possível inativar o autor pois ele possui cursos.");
            }

            await _authorRepository.UpdateAuthorTypeAsync(id, newType);
        }


        public async Task<AuthorWithCoursesDTO> GetAuthorCoursesAsync(Guid authorId)
        {
            var (authorName, courses) = await _authorRepository.GetAuthorCoursesAsync(authorId);

            if (authorName == null)
                return null;

            return new AuthorWithCoursesDTO
            {
                AuthorName = authorName,
                Courses = courses
            };
        }
    }
}