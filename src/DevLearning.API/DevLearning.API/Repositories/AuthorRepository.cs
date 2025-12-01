using Dapper;
using DevLearning.API.DataBase;
using DevLearning.API.Models;
using DevLearning.API.Models.DTOs.Author;
using DevLearning.API.Models.Enums.Author;
using DevLearning.API.Repositories.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data.Common;

namespace DevLearning.API.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        public readonly SqlConnection _connection;

        public AuthorRepository(ConnectionDB connection)
        {
            _connection = connection.GetConnection();
        }
        public async Task<List<AuthorResponseDTO>> GetAllAuthorsAsync()
        {
            try
            {
                var sql = "SELECT Id as AuthorId, Name, Title, Image, Bio, Url, Email, Type FROM [Author]";

                var authors = (await _connection.QueryAsync<AuthorResponseDTO>(sql)).ToList();

                return authors;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao obter autores: " + ex.Message);
            }
        }
        public async Task<AuthorResponseDTO> GetAuthorByIdAsync(Guid id)
        {
            try
            {
                var sql = "SELECT Id as AuthorId, Name, Title, Image, Bio, Url, Email, Type FROM [Author] WHERE Id = @Id";
                var author = await _connection.QueryFirstOrDefaultAsync<AuthorResponseDTO>(sql, new { Id = id });
                return author;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao obter autor: " + ex.Message);
            }
        }

        public async Task<AuthorResponseDTO> GetAuthorByEmail(string email)
        {
            try
            {
                var sql = "SELECT Id as Author, Name, Title, Image, Bio, Url, Email, Type FROM [Author] WHERE Email = @Email";
                var author = await _connection.QueryFirstOrDefaultAsync<AuthorResponseDTO>(sql, new { Email = email });
                return author;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao obter autor: " + ex.Message);
            }
        }
        public async Task CreateAuthorAsync(Author author)
        {
            try
            {
                var sql = @"INSERT INTO [Author] (Id, Name, Title, Image, Bio, Url, Email, Type) 
                        VALUES (@Id, @Name, @Title, @Image, @Bio, @Url, @Email, @Type) ";

                await _connection.ExecuteAsync(sql, new { author.Id, author.Name, author.Title, author.Image, author.Bio, author.Url, author.Email, author.Type });
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao criar autor: " + ex.Message);
            }
        }

        public async Task UpdatePatchAuthorAsync(UpdateAuthorParcialDTO author, Guid id)
        {
            try
            {
                var updates = new List<string>();

                string? name = null;
                string? title = null;
                string? image = null;
                string? bio = null;
                string? url = null;
                AuthorType? type = null;

                if (!string.IsNullOrWhiteSpace(author.Name))
                {
                    name = author.Name;
                    updates.Add("Name = @Name");

                    // Atualiza URL automaticamente
                    url = $"www.devlearning.com.br/author/{author.Name.ToLower().Replace(" ", "-")}";
                    updates.Add("Url = @Url");
                }

                if (!string.IsNullOrWhiteSpace(author.Title))
                {
                    title = author.Title;
                    updates.Add("Title = @Title");
                }

                if (!string.IsNullOrWhiteSpace(author.Image))
                {
                    image = author.Image;
                    updates.Add("Image = @Image");
                }

                if (!string.IsNullOrWhiteSpace(author.Bio))
                {
                    bio = author.Bio;
                    updates.Add("Bio = @Bio");
                }
                if (author.Type.HasValue)
                {
                    type = author.Type.Value;
                    updates.Add("Type = @Type");
                }

                // Nada para atualizar
                if (!updates.Any())
                    return;

                var sql = $"UPDATE Author SET {string.Join(", ", updates)} WHERE Id = @Id";

                await _connection.ExecuteAsync(sql, new
                {
                    Name = name,
                    Title = title,
                    Image = image,
                    Bio = bio,
                    Url = url,
                    Type = type,
                    Id = id
                });
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar autor: " + ex.Message);
            }
        }

        public async Task UpdatePutAuthorAsync(UpdateAuthorFullDTO author, Guid id)
        {
            try
            {
                string url = $"www.devlearning.com.br/author/{author.Name.ToLower().Replace(" ", "-")}";

                var sql = @"  UPDATE Author SET Name = @Name, Title = @Title, Image = @Image,
                    Bio = @Bio, Url = @Url WHERE Id = @Id";

                await _connection.ExecuteAsync(sql, new
                {
                    Name = author.Name,
                    Title = author.Title,
                    Image = author.Image,
                    Bio = author.Bio,
                    Url = url,
                    Id = id

                });
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar autor: " + ex.Message);
            }
        }

        //verifica se o autor tem cursos associados antes de inativar
        public async Task<int> CountCoursesAsync(Guid id)
        {

                var sql = "SELECT COUNT(1) FROM Course WHERE AuthorId = @Id";
                var count = await _connection.ExecuteScalarAsync<int>(sql, new { Id = id });
            
                return count;

        }
        // Atualiza apenas o tipo do autor// Ativo (1) ou Inativo (2)
        public async Task UpdateAuthorTypeAsync(Guid id, AuthorType newType)
        {
            try
            {

                var sql = "UPDATE Author SET Type = @Type WHERE Id = @Id";

                await _connection.ExecuteAsync(sql, new
                {
                    Type = newType,
                    Id = id
                });

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar autor: " + ex.Message);
            }
        }


        public async Task<(string AuthorName, List<string> Courses)> GetAuthorCoursesAsync(Guid authorId)
        {
            try
            {
                var sql = @"
            SELECT a.Name AS AuthorName, c.Title AS CourseTitle
            FROM [Author] a
            LEFT JOIN [Course] c ON a.Id = c.AuthorId
            WHERE a.Id = @AuthorId";

                var rows = await _connection.QueryAsync(sql, new { AuthorId = authorId });

                if (!rows.Any())
                    return (null, new List<string>());

                string authorName = rows.First().AuthorName;
                var courses = rows.Select(r => (string)r.CourseTitle).Where(c => c != null).ToList();

                return (authorName, courses);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao listar autores e seus cursos: " + ex.Message);
            }
        }

    }
}
