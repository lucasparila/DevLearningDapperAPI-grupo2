using Dapper;
using DevLearning.API.DataBase;
using DevLearning.API.Models;
using DevLearning.API.Models.DTOs.Carrer;
using DevLearning.API.Repositories;
using DevLearning.API.Services.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;

namespace DevLearning.API.Services
{
    public class CareerService : ICareerService
    {
        public readonly CareerRepository careerRepository;
        private readonly ILogger<CareerService> logger;
        public CareerService(ILogger<CareerService> logger, CareerRepository careerRepository)
        {
            this.careerRepository = careerRepository;
            this.logger = logger;
        }

        public async Task CreateCareerAsync(CareerRequestDTO careerDTO)
        {
            try

            {
                bool retorno = await careerRepository.GetCareerByTitleAsync(careerDTO.Title);
                if(retorno is true)
                {
                    throw new Exception("Já existe uma carreira com esse título.");
                }

                var career = new Career(
                   careerDTO.Title,
                   careerDTO.Summary,
                   careerDTO.Title.ToLower().Replace(" ", "-"),
                   careerDTO.DurationInMinutes,
                   careerDTO.Tags
                );
                var careerItems = careerDTO.careerItems.Select(itemDTO => new CareerItem(
                    career.Id,
                    itemDTO.CourseId,
                    itemDTO.Title,
                    itemDTO.Description,
                    itemDTO.Order
                )).ToList();
                foreach (var item in careerItems)
                {
                    career.AddItem(item);
                }
                await careerRepository.CreateCareerAsync(career);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Erro interno ao criar carreira e item carreira: {ex.Message}");
                throw;
            }
        }
        public async Task<List<CareerWhitCareerItemResponseDTO>> GetAllCareerAsync()
        {
            try { 
                
                var careers = await careerRepository.GetAllCareerWithCareerItem();
                if (careers.Count == 0)
                {
                    throw new Exception("Ainda não há nenhuma carreira cadastrada");
                }

                return careers;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Erro interno ao listar todas as carreiras: {ex.Message}");
                throw;
            }
        }

        public async Task<CareerWhitCareerItemResponseDTO?> GetCareerByIdAsync(Guid careerId)
        {
            try
            {
                var career = await careerRepository.GetOneCareerWithCareerItem(careerId);
                if (career == null)
                {
                    throw new Exception("Carreira não encontrada");
                }
                return career;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Erro interno ao buscar carreira por ID: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> DeleteCareerAsync(Guid careerId)
        {
            try
            {
                var career = await careerRepository.GetOneCareerWithCareerItem(careerId);
                if (career == null)
                {
                    throw new Exception("Carreira não encontrada");
                }

                var result = await careerRepository.DeleteCareerAsync(careerId);
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Erro interno ao deletar carreira: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> UpdateCareerAsync(Guid id, CareerUpdateDTO updateDTO)
        {
            try
            {

                var existingCareer = await careerRepository.GetOneCareerWithCareerItem(id);
                if(existingCareer == null)
                {
                    throw new Exception("Carreira não encontrada.");
                }

                
                var updates = new List<string>();
                var parameters = new DynamicParameters();
                parameters.Add("Id", id);

                if (!string.IsNullOrEmpty(updateDTO.Title))
                {
                    bool retorno = await careerRepository.GetCareerByTitleAsync(updateDTO.Title);
                    if (retorno is true)
                    {
                        throw new Exception("Já existe uma carreira com esse título.");
                    }

                    updates.Add("Title = @Title");
                    parameters.Add("Title", updateDTO.Title);
                    updates.Add("Url = @Url");
                    parameters.Add("Url", updateDTO.Title.ToLower().Replace(" ", "-"));
                }

                if (!string.IsNullOrEmpty(updateDTO.Summary))
                {
                    updates.Add("Summary = @Summary");
                    parameters.Add("Summary", updateDTO.Summary);
                }

                if (updateDTO.DurationInMinutes.HasValue)
                {
                    updates.Add("DurationInMinutes = @DurationInMinutes");
                    parameters.Add("DurationInMinutes", updateDTO.DurationInMinutes.Value);
                }

                if (updateDTO.Active.HasValue)
                {
                    updates.Add("Active = @Active");
                    parameters.Add("Active", updateDTO.Active.Value);
                }

                if (updateDTO.Featured.HasValue)
                {
                    updates.Add("Featured = @Featured");
                    parameters.Add("Featured", updateDTO.Featured.Value);
                }

                if (!string.IsNullOrEmpty(updateDTO.Tags))
                {
                    updates.Add("Tags = @Tags");
                    parameters.Add("Tags", updateDTO.Tags);
                }



                var result = await careerRepository.UpdateCareerAsync(id, updates, parameters);
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Erro interno ao atualizar carreira: {ex.Message}");
                throw;
            }
        }


    }
}
