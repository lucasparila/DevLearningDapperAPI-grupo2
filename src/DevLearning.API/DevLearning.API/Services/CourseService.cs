using DevLearning.API.Models;
using DevLearning.API.Models.DTOs.Course;
using DevLearning.API.Repositories;
using DevLearning.API.Repositories.Interfaces;
using DevLearning.API.Services.Interfaces;

namespace DevLearning.API.Services
{
    public class CourseService : ICourseService
    {

        private CourseRepository _courseRepository;
        private ICategoryRepository _categoryRepository;
        private AuthorRepository _authorRepository;
        private StudentRepository _studentRepository;

        public CourseService(CourseRepository courseRepository, ICategoryRepository categoryRepository, AuthorRepository authorRepository, StudentRepository studentRepository)
        {
            _courseRepository = courseRepository;
            _categoryRepository = categoryRepository;
            _authorRepository = authorRepository;
            _studentRepository = studentRepository;
        }

        public async Task CreateCourseAsync(CourseRequestDTO course)
        {
            try
            {
                var verifyTitle = await _courseRepository.GetOneCourseByTitleAsync(course.Title);
                var verifyAuthor = await _authorRepository.GetAuthorByIdAsync(course.AuthorId);
                var verifyCategory = await _categoryRepository.GetCategoryByIdAsync(course.CategoryId);
                if (verifyTitle is null)
                {
                    if (verifyAuthor is not null)
                    {
                        if (verifyCategory is not null)
                        {
                            var newCourse = new Course(Guid.NewGuid(), course.Tag, course.Title, course.Summary, course.Url, course.Level, course.DurationInMinutes, DateTime.UtcNow, DateTime.UtcNow, true, false, false, course.AuthorId, course.CategoryId, course.Tags);
                            await _courseRepository.CreateCourseAsync(newCourse);
                        }
                        else
                        {
                            throw new Exception("Categoria inexistente!");
                        }
                    }
                    else
                    {
                        throw new Exception("Autor inexistente!");
                    }
                }
                else
                {
                    throw new Exception("Título de curso já existente!");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<CourseResponseDTO> DeleteCourseByTitleAsync(string title)
        {
            try
            {
                return await _courseRepository.DeleteCourseByTitleAsync(title);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<CourseResponseDTO>> GetAllCoursesAsync(string category)
        {
            try
            {
                return await _courseRepository.GetAllCoursesAsync(category);
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
                return await _courseRepository.GetOneCourseByTitleAsync(title);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateCourseByTitleAsync(string title, CourseUpdateDTO update)
        {
            try
            {
                await _courseRepository.UpdateCourseByTitleAsync(title, update.Free, update.Featured, DateTime.UtcNow);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateActiveCourseByTitleAsync(string title, CourseActiveDTO update)
        {
            try
            {
                var courseStorage = await _courseRepository.GetOneCourseByTitleAsync(title);
                if(courseStorage is null)
                {
                    throw new Exception("Você não modificar um curso inexistente!");
                } 
                    var verifyStudentCourse = await _studentRepository.GetCountStudentCourse(courseStorage.CourseId);

                if (verifyStudentCourse > 0)
                {
                    throw new Exception("Você não pode inativar um curso com alunos nele!");
                }

                await _courseRepository.UpdateActiveCourseByTitleAsync(title, update.Active, DateTime.UtcNow);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<CourseResponseDTO> GetOneCourseByIdAsync(string id)
        {
            try
            {
                return await _courseRepository.GetOneCourseByIdAsync(Guid.Parse(id));
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
