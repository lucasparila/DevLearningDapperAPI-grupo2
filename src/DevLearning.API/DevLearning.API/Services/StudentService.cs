using DevLearning.API.Models;
using DevLearning.API.Models.DTOs.Student;
using DevLearning.API.Repositories;
using DevLearning.API.Services.Interfaces;

namespace DevLearning.API.Services
{
    public class StudentService : IStudentService
    {
        private StudentRepository _studentRepository;
        private CourseRepository _courseRepository;
        public StudentService(StudentRepository studentRepository, CourseRepository courseRepository)
        {
            _studentRepository = studentRepository;
            _courseRepository = courseRepository;
        }
        public async Task CreateStudent(StudentRequestDTO student)
        {
            try
            {
                var studentStorage = await _studentRepository.GetStudentByEmailAndDocument(student.Email, student.Document);
                if(studentStorage is not null)
                    throw new Exception("Estudante com email ou documento já cadastrado.");
                var newStudent = new Student(student.Name, student.Email, student.Document, student.Phone, student.Birthdate);
                await _studentRepository.CreateStudent(newStudent);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task InsertStudentCourse(Guid studentId, Guid courseId, StudentRequestInsertCourseDTO studentCourse)
        {
            try
            {
                if (await _studentRepository.GetStudentById(studentId) is null)
                    throw new Exception("Estudante não encontrado");
                var course = await _courseRepository.GetOneCourseByIdAsync(courseId);
                if (course is null)
                    throw new Exception("Curso não encontrado");
                if (course.Active == false)
                    throw new Exception("Curso inativo, não pode ocorrer mátricula");
                if(await _studentRepository.GetStudentCourse(studentId, courseId) is not null)
                    throw new Exception("Estudante já está matriculado nesse curso");
                await _studentRepository.InsertStudentCourse(studentId, courseId, studentCourse);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<StudentResponseDTO>> GetAllStudents()
        {
            try
            {
                return await _studentRepository.GetAllStudents();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<StudentResponseDTO> GetStudentByEmail(string email)
        {
            try
            {
                return await _studentRepository.GetStudentByEmail(email);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<StudentResponseDTO> GetStudentByDocument(string document)
        {
            try
            {
                return await _studentRepository.GetStudentByDocument(document);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<StudentResponseDTO> GetStudentById(string id)
        {
           try
            {
                return await _studentRepository.GetStudentById(Guid.Parse(id));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task UpdateStudent(StudentRequestUpdateDTO student, string id)
        {
            try
            {
                var studentStorage = await _studentRepository.GetStudentById(Guid.Parse(id));
                if (studentStorage is null)
                    throw new Exception("Estudante não encontrado");
                if (await _studentRepository.GetStudentByDocument(student.Document) is not null)
                    throw new Exception("O documento informado já está cadastrado.");
                if (student.Email is not null && await _studentRepository.GetStudentByEmail(student.Email) is not null)
                    throw new Exception("O email informado já está cadastrado.");

                var newStudent = new Student(
                    student.Name is not null ? student.Name : studentStorage.Name,
                    student.Email is not null ? student.Email : studentStorage.Email,
                    student.Phone is not null ? student.Phone : studentStorage.Phone,
                    student.Document is not null ? student.Document : studentStorage.Document,
                    student.Birthdate is not null ? (DateTime)student.Birthdate : studentStorage.Birthdate
                    );
                await _studentRepository.UpdateStudent(newStudent, Guid.Parse(id));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task UpdateStudentCourse(Guid studentId, Guid courseId, StudentCourseRequestUpdateDTO studentCourse)
        {
            try
            {

                if (await _studentRepository.GetStudentById(studentId) is null)
                    throw new Exception("Estudante não encontrado");
                if (await _courseRepository.GetOneCourseByIdAsync(courseId) is null)
                    throw new Exception("Curso não encontrado");
                await _studentRepository.UpdateStudentCourse(studentId, courseId, studentCourse);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}