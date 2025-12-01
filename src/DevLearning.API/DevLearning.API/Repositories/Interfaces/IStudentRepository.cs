using DevLearning.API.Models;
using DevLearning.API.Models.DTOs.Student;

namespace DevLearning.API.Repositories.Interfaces
{
    public interface IStudentRepository
    {
        public Task CreateStudent(Student student);
        public Task UpdateStudent(Student student, Guid id);
        public Task<List<StudentResponseDTO>> GetAllStudents();
        public Task<StudentResponseDTO> GetStudentByDocument(string document);
        public Task<StudentResponseDTO> GetStudentByEmail(string email);
        public Task<StudentResponseDTO> GetStudentById(Guid id);
        public Task<int> GetCountStudentCourse(Guid courseId);
        public Task InsertStudentCourse(Guid studentId, Guid courseId, StudentRequestInsertCourseDTO studentCourse);
        public Task UpdateStudentCourse(Guid studentId, Guid courseId, StudentCourseRequestUpdateDTO studentCourse);
    }
}
