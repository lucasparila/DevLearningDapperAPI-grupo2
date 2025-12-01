using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Azure;
using Dapper;
using DevLearning.API.DataBase;
using DevLearning.API.Models;
using DevLearning.API.Models.DTOs.Course;
using DevLearning.API.Repositories.Interfaces;
using Microsoft.Data.SqlClient;

namespace DevLearning.API.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly SqlConnection _connection;

        public CourseRepository(ConnectionDB connection)
        {
            _connection = connection.GetConnection();
        }

        public async Task CreateCourseAsync(Course course)
        {
            try
            {
                var sql = @"INSERT INTO Course(Id, Tag, Title, Summary, [Url], [Level], DurationInMinutes,
                      CreateDate, LastUpdateDate, Active, Free, Featured, AuthorId, CategoryId, Tags) 
                      VALUES(@Id, @Tag, @Title, @summary, @Url, @Level, @DurationInMinutes, @CreateDate, @LastUpdateDate,
                      @Active, @Free, @Featured, @AuthorId, @CategoryId, @Tags)";

                await _connection.ExecuteAsync(sql, new { course.Id, course.Tag, course.Title, course.Summary, course.Url, course.Level, course.DurationInMinutes, course.CreateDate, course.LastUpdateDate, course.Active, course.Free, course.Featured, course.AuthorId, course.CategoryId, course.Tags });
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<CourseResponseDTO> DeleteCourseByTitleAsync(string title)
        {
            throw new NotImplementedException();
        //    try
        //    {

        //        var sqlGetId = "SELECT Id FROM Course WHERE Title = @title";
        //        var courseId = await _connection.ExecuteScalarAsync<int?>(sqlGetId, new { title });

        //        var sqlCareer = @"DELETE FROM CouseCareer WHERE CourseId = @IdCurso";
        //        await _connection.ExecuteAsync(sqlCareer, new { CourseId = courseId });

        //        var sqlStudent = @"DELETE FROM CouseCareer WHERE CourseId = @IdCurso";
        //        await _connection.ExecuteAsync(sqlCareer, new { CourseId = courseId });

        //        var sql = @"DELETE FROM Course WHERE title = @title";
        //        return (await _connection.QueryFirstOrDefaultAsync<CourseResponseDTO>(sql, new { title = title}));

        //    }
        //    catch (SqlException ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        }

        public async Task<List<CourseResponseDTO>> GetAllCoursesAsync(string category)
        {
            try
            {
                var sql = @"SELECT c.Id as CourseId, c.Tag, c.Title, c.Summary, c.[Url], c.[Level], c.DurationInMinutes,
                       c.CreateDate, c.LastUpdateDate, c.Active, c.Free, c.Featured, a.[Name] AS authorName, 
                       ca.Title AS categoryName, c.Tags FROM Course c
                       JOIN Author a ON a.Id = c.AuthorId
                       JOIN Category ca ON ca.Id = c.CategoryId
                       WHERE (@categoria IS NULL OR ca.Title = @categoria) 
                       ORDER BY c.Level ASC;";

                return (await _connection.QueryAsync<CourseResponseDTO>(sql, new { categoria = category })).ToList();
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<CourseResponseDTO> GetOneCourseByTitleAsync(string title)
        {
            try
            {
                var sql = @"SELECT c.Id AS CourseId, c.Tag, c.Title, c.Summary, c.[Url], c.[Level], c.DurationInMinutes,
                       c.CreateDate, c.LastUpdateDate, c.Active, c.Free, c.Featured, a.[Name] AS authorName, 
                       ca.Title AS categoryName, c.Tags FROM Course c
                       JOIN Author a ON a.Id = c.AuthorId
                       JOIN Category ca ON ca.Id = c.CategoryId WHERE c.title = @Titulo";

                return (await _connection.QueryFirstOrDefaultAsync<CourseResponseDTO>(sql, new { Titulo = title }));
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<CourseResponseDTO> GetOneCourseByIdAsync(Guid id)
        {
            var sql = @"SELECT c.Tag, c.Title, c.Summary, c.[Url], c.[Level], c.DurationInMinutes,
                       c.CreateDate, c.LastUpdateDate, c.Active, c.Free, c.Featured, a.[Name] AS authorName, 
                       ca.Title AS categoryName, c.Tags FROM Course c
                       JOIN Author a ON a.Id = c.AuthorId
                       JOIN Category ca ON ca.Id = c.CategoryId WHERE c.Id = @Id";

            return (await _connection.QueryFirstOrDefaultAsync<CourseResponseDTO>(sql, new { Id = id }));
        }

        public async Task UpdateCourseByTitleAsync(string title, bool free, bool featured, DateTime lastUpdateDate)
        {
            try
            {
                var sql = @"UPDATE Course SET Free = @free, Featured = @featured, LastUpdateDate = @lastUpdateDate WHERE Title = @Title";

                await _connection.ExecuteAsync(sql, new { Free = free, Featured = featured, LastUpdateDate = lastUpdateDate, Title = title });
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateActiveCourseByTitleAsync(string title, bool active, DateTime lastUpdateDate)
        {
            try
            {
                var sql = @"UPDATE Course SET Active = @active, LastUpdateDate = @lastUpdateDate WHERE Title = @Title";
                await _connection.ExecuteAsync(sql, new { active = active, LastUpdateDate = lastUpdateDate, Title = title });
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
