
using Dapper;
using DevLearning.API.DataBase;
using DevLearning.API.Models;
using DevLearning.API.Models.DTOs.CareerItem;
using DevLearning.API.Models.DTOs.Carrer;
using DevLearning.API.Repositories.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data.Common;
using System.Transactions;

namespace DevLearning.API.Repositories
{
    public class CareerItemRepository : ICareerItemRepository
    {
        private readonly SqlConnection connection;
        private readonly ILogger<CareerItemRepository> logger;
        public CareerItemRepository(ConnectionDB connectionDB, ILogger<CareerItemRepository> logger)
        {
            this.connection = connectionDB.GetConnection();
            this.logger = logger;
        }


        public async Task CreateCareerItemAsync(CareerItem career)
        {

            try
            {

                var sqlCareerItem = @"INSERT INTO CareerItem (CareerId, CourseId, Title, Description, [Order])
                                         VALUES (@CareerId, @CourseId, @Title, @Description, @Order)";

                await connection.ExecuteAsync(sqlCareerItem, new
                {
                    career.CareerId,
                    career.CourseId,
                    career.Title,
                    career.Description,
                    career.Order
                });
            }
            catch (SqlException ex)
            {

                logger.LogError(ex, "Erro ao criar nova item de carreira");
                throw;
            }

        }

        public async Task<bool> GetCareerItemByIdAsync(Guid CareerId, Guid CourseId)
        {
            try
            {
                var sql = @"SELECT *
                            FROM CareerItem
                            WHERE CareerId = @CareerId AND CourseId = @CourseId";
                var careerItem = await connection.QueryFirstOrDefaultAsync<CareerItemResponseDTO>(sql, new { CareerId = CareerId, CourseId = CourseId });
                return careerItem != null ? true : false;
            }
            catch (SqlException ex)
            {
                logger.LogError($"Erro ao buscar item da carrereira com o id de carreira {CareerId} : {ex.Message}");
                throw;
            }
        }

        public async Task<bool> GetCareerItemByTitleAsync(string title)
        {
            try
            {
                var sql = @"SELECT *
                            FROM CareerItem
                            WHERE Title = @Title";
                var careerItem = await connection.QueryFirstOrDefaultAsync<CareerItemResponseDTO>(sql, new { Title = title });
                return careerItem != null ? true : false;
            }
            catch (SqlException ex)
            {
                logger.LogError($"Erro ao buscar item da carrereira com titulo {title} : {ex.Message}");
                throw;
            }
        }
        public async Task<bool> ExistingCarrerItemwhitOrder(byte order)
        {
            try
            {
                var sql = @"SELECT *
                            FROM CareerItem
                            WHERE [Order] = @Order";
                var careerItem = await connection.QueryFirstOrDefaultAsync<CareerItemResponseDTO>(sql, new { Order = order });
                return careerItem != null ? true : false;
            }
            catch (SqlException ex)
            {
                logger.LogError($"Erro ao buscar item da carrereira com order {order} : {ex.Message}");
                throw;
            }

        }

        public async Task<bool> UpdateCareerItemAsync(Guid CareerId, Guid CourseId, List<string> updates, DynamicParameters parameters)
        {
            try
            {
                var sql = $"UPDATE CareerItem SET {string.Join(", ", updates)} WHERE CareerId = @CareerId AND CourseId = @CourseId";
                
                var rows = await connection.ExecuteAsync(sql, parameters);

                return rows > 0 ? true : false;

            }
            catch (SqlException ex)
            {
                logger.LogError($"Erro ao atualizar item da carrereira com o id de carreira {CareerId} : {ex.Message}");
                throw;
            }

        }

        public async Task<bool> DeleteCareerItemAsync(Guid CareerId, Guid CourseId)
        {
            try
            {
                var sql = @"DELETE FROM CareerItem WHERE CareerId = @CareerId AND CourseId = @CourseId";
                var rows = await connection.ExecuteAsync(sql, new { CareerId = CareerId, CourseId = CourseId });
                return rows > 0 ? true : false;
            }
            catch (SqlException ex)
            {
                logger.LogError($"Erro ao deletar item da carrereira com o id de carreira {CareerId}: {ex.Message}");
                throw;
            }

        }
    }
}
