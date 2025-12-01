using DevLearning.API.Models;
using DevLearning.API.Models.DTOs.Author;
using DevLearning.API.Models.Enums.Author;
using DevLearning.API.Services;
using DevLearning.API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DevLearning.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private AuthorService _authorService;

        public AuthorController(AuthorService authorService)
        {
            _authorService = authorService;
        }

        //Listar todos os autores
        [HttpGet]
        public async Task<ActionResult<List<AuthorResponseDTO>>> GetAllAuthors()
        {
            try
            {
                var authors = await _authorService.GetAllAuthorsAsync();
                return Ok(authors);
            }
            catch (Exception ex)
            {
                return StatusCode(404, new { error = $"Lista de autores não encontrada. {ex.Message}" });
            }
        }

        //Listar autor por Id
        [HttpGet("{id}")]
        public async Task<ActionResult> GetAuthorById(Guid id)
        {
            try
            {
                var author = await _authorService.GetAuthorByIdAsync(id);
                if (author is null)
                    return StatusCode(404, new { message = "Autor não encontrado" });
                else
                    return Ok(author);
            }
            catch (Exception ex)
            {
                return StatusCode(404, new { error = $"Autor não encontrado. {ex.Message}" });
            }
           
        }

        //Criar autor
        [HttpPost]
        public async Task<ActionResult> CreateAuthor(AuthorRequestDTO author)
        {
            try
            {
                await _authorService.CreateAuthorAsync(author);
                return Created();
            }
            catch (Exception ex)
            {
                return StatusCode(400, new { error = $"Erro ao criar autor. {ex.Message}" });
            }
        }

        //Atualizar autor
        // PATCH 
        [HttpPatch("{id}")]
        public async Task<ActionResult> UpdatePatchAuthor(Guid id, [FromBody] UpdateAuthorParcialDTO dto)
        {
            try
            {
                await _authorService.UpdatePatchAuthorAsync(id, dto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(400, new { error = $"Erro ao atualizar autor. {ex.Message}" });
            }
        }

        // PUT 
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdatePutAuthor(Guid id, [FromBody] UpdateAuthorFullDTO dto)
        {
            try
            {
                await _authorService.UpdatePutAuthorAsync(id, dto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(400, new { error = $"Erro ao atualizar autor. {ex.Message}" });
            }
        }

        // Atualiza apenas o tipo do autor// Ativo (1) ou Inativo (2)
        [HttpPut("type/{id}")]
        public async Task<ActionResult> UpdateType(Guid id, [FromBody] AuthorType type)
        {
            try
            {
                await _authorService.UpdateAuthorTypeAsync(id, type);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(400, new { error = $"Erro ao atualizar tipo do autor. {ex.Message}" });
            }
        }

        //Listar cursos do autor
        [HttpGet("{id}/courses")]
        public async Task<ActionResult> GetAuthorCourses(Guid id)
        {
            var result = await _authorService.GetAuthorCoursesAsync(id);

            if (result == null)
                return NotFound("Autor não encontrado.");

            return Ok(result);
        }
    }
}
