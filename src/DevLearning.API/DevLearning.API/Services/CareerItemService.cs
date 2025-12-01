using Dapper;
using DevLearning.API.DataBase;
using DevLearning.API.Models;
using DevLearning.API.Models.DTOs.CareerItem;
using DevLearning.API.Models.DTOs.Carrer;
using DevLearning.API.Repositories;
using DevLearning.API.Services.Interfaces;
using Microsoft.Data.SqlClient;

namespace DevLearning.API.Services
{
    public class CareerItemService : ICareerItemService
    {
        public readonly CareerItemRepository careerItemRepository;
        public readonly CareerRepository career;
        private readonly ILogger<CareerItemService> logger;
        public CareerItemService(ILogger<CareerItemService> logger, CareerItemRepository careerItemRepository, CareerRepository career)
        {
            this.careerItemRepository = careerItemRepository;
            this.logger = logger;
        }

        public async Task<bool> CreateItemCareerAsync(CareerItemRequestCreateDTO careerItemDTO)
        {
            try

            {
                var retorno = await careerItemRepository.GetCareerItemByIdAsync(careerItemDTO.CareerId, careerItemDTO.CourseId);
                if (retorno == true){
                    throw new Exception("Este curso já está cadastrado nesta carreira");
                }

                retorno = await careerItemRepository.GetCareerItemByTitleAsync(careerItemDTO.Title);
                if (retorno == true)
                {
                    throw new Exception("Já existe uma carreira com esse título");
                }

                retorno = await careerItemRepository.ExistingCarrerItemwhitOrder(careerItemDTO.Order);
                if (retorno == true)
                {
                    throw new Exception("Já existe uma carreira com essa ordem");
                }

                var careerItems = new CareerItem(
                    careerItemDTO.CareerId,
                    careerItemDTO.CourseId,
                    careerItemDTO.Title,
                    careerItemDTO.Description,
                    careerItemDTO.Order
                );
               
                await careerItemRepository.CreateCareerItemAsync(careerItems);
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Erro interno ao item carreira: {ex.Message}");
                throw;
            }
        }
       
        public async Task<bool> DeleteItemCareerAsync(Guid careerId, Guid courseId)
        {
            try
            {
                var retorno = await careerItemRepository.GetCareerItemByIdAsync(careerId, courseId);
                if (retorno == false)
                {
                    throw new Exception("Item de carreira não encontrado");
                }
                var result = await careerItemRepository.DeleteCareerItemAsync(careerId, courseId);
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Erro interno ao deletar item de carreira: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> UpdateCareerItemAsync(Guid careerId, Guid courseId, CareerItemUpdateDTO updateDTO)
        {
            try
            {
                bool retorno = await careerItemRepository.GetCareerItemByIdAsync(careerId, courseId);
                if (retorno == false)
                {
                    throw new Exception("Item de carreira não encontrado");
                }

                var updates = new List<string>();
                var parameters = new DynamicParameters();
                parameters.Add("CareerId", careerId);
                parameters.Add("CourseId", courseId);


                if (updateDTO.CourseId.HasValue)
                {
                    updates.Add("CourseId = @CourseId");
                    parameters.Add("CourseId", updateDTO.CourseId);
                }

                if (!string.IsNullOrEmpty(updateDTO.Title))
                {
                     retorno = await careerItemRepository.GetCareerItemByTitleAsync(updateDTO.Title);
                    if (retorno == true)
                    {
                        throw new Exception("Já existe uma carreira com esse título");
                    }
                    updates.Add("Title = @Title");
                    parameters.Add("Title", updateDTO.Title);
                }
                if (!string.IsNullOrEmpty(updateDTO.Description))
                {
                    updates.Add("Description = @Description");
                    parameters.Add("Description", updateDTO.Description);
                }
                if (updateDTO.Order.HasValue)
                {
                    updates.Add("[Order] = @Order");
                    parameters.Add("Order", updateDTO.Order);
                }
               
                var result = await careerItemRepository.UpdateCareerItemAsync(careerId, courseId, updates, parameters);
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
