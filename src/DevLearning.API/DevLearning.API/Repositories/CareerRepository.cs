
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
    public class CareerRepository : ICareerRepository
    {
        private readonly SqlConnection connection;
        private readonly ILogger<CareerRepository> logger;
        public CareerRepository(ConnectionDB connectionDB, ILogger<CareerRepository> logger)
        {
            this.connection = connectionDB.GetConnection();
            this.logger = logger;
        }


        public async Task CreateCareerAsync(Career career)
        {
            await connection.OpenAsync();
            using var transaction = connection.BeginTransaction();
            try
            {
                var sql = @"INSERT INTO Career (Id, Title, Summary, url, DurationInMinutes, Active, Featured, Tags)
                        VALUES (@Id, @Title, @Summary, @url, @DurationInMinutes, @Active, @Featured, @Tags)";
                var parameters = new
                {
                    career.Id,
                    career.Title,
                    career.Summary,
                    career.url,
                    career.DurationInMinutes,
                    career.Active,
                    career.Featured,
                    career.Tags
                };
                await connection.ExecuteAsync(sql, parameters, transaction);

                if (career.items is not null && career.items.Count > 0)
                {
                    var sqlCareerItem = @"INSERT INTO CareerItem (CareerId, CourseId, Title, Description, [Order])
                                         VALUES (@CareerId, @CourseId, @Title, @Description, @Order)";

                    foreach (var item in career.items)
                    {
                        await connection.ExecuteAsync(sqlCareerItem, new
                        {
                            CareerId = career.Id,
                            item.CourseId,
                            item.Title,
                            item.Description,
                            item.Order
                        }, transaction);
                    }
                }
                transaction.Commit();
            }
            catch (SqlException ex)
            {
                transaction.Rollback();
                logger.LogError(ex, "Erro ao criar nova carreira e itens da carreira");
                throw;
            }
            
        }


        public async Task<List<CareerResponseDTO>> GetAllCareersAsync()
        {
            try
            {
                var sql = @"SELECT 
                            Id, Title, Summary, url, DurationInMinutes, Active, Featured, Tags
                            FROM Career";
                var careers = (await connection.QueryAsync<CareerResponseDTO>(sql)).ToList();
                return careers;

            }
            catch (SqlException ex)
            {
                logger.LogError(ex, "Erro ao buscar todas as carreiras");
                throw;
            }
           
        }

        public async Task<CareerResponseDTO> GetCareerByIdAsync(Guid Id)
        {
            try
            {
                var sql = @"SELECT 
                       Id, Title, Summary, url, DurationInMinutes, Active, Featured, Tags
                      FROM Career
                      WHERE Id = @Id";
                var career = await connection.QueryFirstOrDefaultAsync<CareerResponseDTO>(sql, new { Id = Id });
                return career;
            }
            catch (SqlException ex)
            {
                logger.LogError(ex, $"Erro ao buscar carreira com Id {Id}");
                throw;
            }
               
        }

        public async Task<bool> GetCareerByTitleAsync(string Title)
        {
            try
            {
                var sql = @"SELECT 
                          Id, Title, Summary, url, DurationInMinutes, Active, Featured, Tags
                          FROM Career
                          WHERE Title = @Title";
                var career = await connection.QuerySingleOrDefaultAsync<CareerResponseDTO>(sql, new { Title = Title });
                return career == null ? false: true;
            }
            catch (SqlException ex)
            {
                logger.LogError(ex, $"Erro ao buscar carreira com: {Title}");
                throw;
            }
        }

        public async Task <bool> UpdateCareerAsync(Guid id, List<string> updates, DynamicParameters parameters)
        {
            try
            {


                var sql = $"UPDATE Career SET {string.Join(", ", updates)} WHERE Id = @Id";
                var rows = await connection.ExecuteAsync(sql, parameters);

                return rows > 0 ? true : false;
            }
            catch (SqlException ex)
            {
                logger.LogError($"Erro ao atualizar usuário com Id {id}: {ex.Message}");
                throw;
            }
            
        }

        public async Task<bool> DeleteCareerAsync(Guid Id)
        {
            try
            {
                var sql = @"DELETE FROM CareerItem WHERE CareerId = @CareerId";
                Guid CareerId = Id;
                await connection.ExecuteAsync(sql, new { CareerId = CareerId });
                sql = @"DELETE FROM Career WHERE Id = @Id";
                var rows = await connection.ExecuteAsync(sql, new { Id = Id });
                
                return rows > 0 ? true : false;
            }
            catch (SqlException ex)
            {
                logger.LogError($"Erro ao deletar carreira com Id {Id}: {ex.Message}");
                throw;
            }

        }

        public async Task<CareerWhitCareerItemResponseDTO> GetOneCareerWithCareerItem(Guid careerId)
        {
            try
            {
                CareerWhitCareerItemResponseDTO? career = null;

                var sql = @"SELECT 
                          c.Id, c.Title, c.Summary, c.Url, c.DurationInMinutes, c.Active, c.Featured, c.Tags,
                          ci.CourseId, ci.Title, ci.Description, ci.[Order],
                          crs.Title AS CourseTitle
                          FROM Career c
                         LEFT JOIN CareerItem ci ON ci.CareerId = c.Id
                         LEFT JOIN Course crs ON ci.CourseId = crs.Id
                         WHERE c.Id = @CareerId
                         ORDER BY ci.[Order];";

                await connection.QueryAsync<CareerWhitCareerItemResponseDTO, CareerItemResponseDTO, CareerWhitCareerItemResponseDTO>(
                    sql,
                    (c, item) =>
                    {
                        if (career == null)
                        {
                            career = c;
                            career.Items = new List<CareerItemResponseDTO>();
                        }

                        
                        if (item != null && item.CourseId != Guid.Empty)
                        {
                            career.Items.Add(item);
                        }

                        return career;
                    },
                    param: new { CareerId = careerId },
                    splitOn: "CourseId"
                );

                return career;
            }
            catch (SqlException ex)
            {
                logger.LogError($"Erro ao buscar carreira com itens com o id da carreira {careerId}: {ex.Message}");
                throw;
            }
        }


        public async Task<List<CareerWhitCareerItemResponseDTO>> GetAllCareerWithCareerItem()
        {
            try
            {
                var careerDict = new Dictionary<Guid, CareerWhitCareerItemResponseDTO>();

                var sql = @"SELECT 
                c.Id,
                c.Title,
                c.Summary,
                c.Url,
                c.DurationInMinutes,
                c.Active,
                c.Featured,
                c.Tags,
                ci.CourseId,
                ci.Title,
                ci.Description,
                ci.[Order],
                crs.Title AS CourseTitle
            FROM Career c
            LEFT JOIN CareerItem ci ON ci.CareerId = c.Id
            LEFT JOIN Course crs ON ci.CourseId = crs.Id
            ORDER BY c.Title, ci.[Order];";

                var careers = await connection.QueryAsync<CareerWhitCareerItemResponseDTO, CareerItemResponseDTO, CareerWhitCareerItemResponseDTO>(
                    sql,
                    (career, item) =>
                    {
                        if (!careerDict.TryGetValue(career.Id, out var currentCareer))
                        {
                            currentCareer = career;
                            careerDict.Add(currentCareer.Id, currentCareer);
                        }

                        if (item != null && item.CourseId != Guid.Empty)
                            currentCareer.Items.Add(item);

                        return currentCareer;
                    },
                    splitOn: "CourseId"
                );

                return careerDict.Values.ToList();
            }
            catch (SqlException ex)
            {
                logger.LogError(ex, "Erro ao buscar todas as carreiras com itens de carreira");
                throw;
            }
        }

    }
}
