namespace DevLearning.API.Models
{
    public class Student
    {
        public Student(string name, string email, string? document, string? phone, DateTime birthdate)
        {
            Id = Guid.NewGuid();
            Name = name;
            Email = email;
            Document = document;
            Phone = phone;
            Birthdate = birthdate;
            CreateDate = DateTime.Now;
        }

        public Student()
        {
            
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string? Document { get; private set; }
        public string? Phone { get; private set; }
        public DateTime Birthdate { get; private set; }
        public DateTime CreateDate { get; private set; }
    }
}
