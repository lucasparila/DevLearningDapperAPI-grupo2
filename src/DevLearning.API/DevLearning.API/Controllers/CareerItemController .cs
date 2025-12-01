using DevLearning.API.Models;
using DevLearning.API.Models.DTOs.CareerItem;
using DevLearning.API.Models.DTOs.Carrer;
using DevLearning.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DevLearning.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]


    public class CareerItemController : ControllerBase
    {

        public readonly CareerItemService careerItemService;
        private readonly ILogger<CareerItemController> logger;

        public CareerItemController(ILogger<CareerItemController> logger, CareerItemService careerItemService)
        {
            this.careerItemService = careerItemService;
            this.logger = logger;
        }


        [HttpPost]
        public async Task<ActionResult> CreateCareerItem([FromBody] CareerItemRequestCreateDTO careerItemDTO)
        {
            try
            {

                var retorno = await careerItemService.CreateItemCareerAsync(careerItemDTO);
                if (retorno is false)
                {
                    return Conflict("Item de carreira já existe.");
                }
                return Created();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Erro ao criar item de carreira: {ex.Message}");
                return StatusCode(500, $"{ex.Message}");
            }
        }

        [HttpPut("{careerId}/{courseId}")]
        public async Task<ActionResult> UpdateCareer(Guid careerId, Guid courseId, [FromBody]CareerItemUpdateDTO careerItemDTO)
        {
            try
            {
                var result = await careerItemService.UpdateCareerItemAsync(careerId, courseId, careerItemDTO);
                if (result is false)
                {
                    return NotFound();
                }
                return NoContent();

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Erro ao atualizar item de carreira por ID: {ex.Message}");
                return StatusCode(500, $"{ex.Message}");
            }
        }

        [HttpDelete("{careerId}/{courseId}")]
        public async Task<ActionResult> DeleteCareer(Guid careerId, Guid courseId)
        {
            try
            {
                var result =  await careerItemService.DeleteItemCareerAsync(careerId, courseId);
                if (result is false)
                {
                    return NotFound();
                }
                return NoContent();

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Erro ao buscar deletar item de carreira por ID: {ex.Message}");
                return StatusCode(500, $"{ex.Message}");
            }
        }
    }
}
