using DevLearning.API.Models.DTOs.Carrer;
using DevLearning.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DevLearning.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]


    public class CareerController : ControllerBase
    {

        public readonly CareerService careerService;
        private readonly ILogger<CareerController> logger;

        public CareerController(ILogger<CareerController> logger, CareerService careerService)
        {
            this.careerService = careerService;
            this.logger = logger;
        }


        [HttpPost]
        public async Task<ActionResult> CreateCareer([FromBody] CareerRequestDTO careerDTO)
        {
            try
            {
                await careerService.CreateCareerAsync(careerDTO);
                return Created();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Erro ao criar carreira: {ex.Message}");
                return StatusCode(500, $"{ex.Message}");
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<CareerResponseDTO>>> GetAllCareers()
        {
            try
            {
                var careers = await careerService.GetAllCareerAsync();
                return Ok(careers);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Erro ao listar todas as carreiras: {ex.Message}");
                return StatusCode(500, $"{ex.Message}");
            }
        }

        [HttpGet("{careerId}")]
        public async Task<ActionResult<CareerResponseDTO>> GetCareerById(Guid careerId)
        {
            try
            {
                var careerData = await careerService.GetCareerByIdAsync(careerId);
                if (careerData == null)
                {
                    return NotFound();
                }
                return Ok(careerData);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Erro ao buscar carreira por ID: {ex.Message}");
                return StatusCode(500, $"{ex.Message}");
            }
        }

        [HttpPut("{careerId}")]
        public async Task<ActionResult> UpdateCareer(Guid careerId, [FromBody] CareerUpdateDTO careerDTO)
        {
            try
            {
                var result = await careerService.UpdateCareerAsync(careerId, careerDTO);
                if (result is false)
                {
                    return NotFound();
                }
                return NoContent();

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Erro ao atualizar carreira por ID: {ex.Message}");
                return StatusCode(500, $"{ex.Message}");
            }
        }

        [HttpDelete("{careerId}")]
        public async Task<ActionResult> DeleteCareer(Guid careerId)
        {
            try
            {
                var result =  await careerService.DeleteCareerAsync(careerId);
                if (result is false)
                {
                    return NotFound();
                }
                return NoContent();

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Erro ao buscar deletar carreira por ID: {ex.Message}");
                return StatusCode(500, $"{ex.Message}");
            }
        }
    }
}
