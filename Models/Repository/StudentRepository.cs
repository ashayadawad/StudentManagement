using StudentManagement.Models.Repository;
using System.Collections.Generic;
using System.Linq;

namespace StudentManagement.Models.Repository
{
    public class StudentRepository : IRepository<Student>
    {
        private readonly StudentContext _studentContext;

        public StudentRepository(StudentContext studentContext)
        {
            _studentContext = studentContext;
        }
        public void Add(Student entity)
        {
            _studentContext.Students.Add(entity);
            _studentContext.SaveChanges();
        }

        public void Delete(Student entity)
        {
            _studentContext.Students.Remove(entity);
            _studentContext.SaveChanges();
        }

        public Student Get(int id)
        {
            return _studentContext.Students.FirstOrDefault(student => student.Id == id);
        }

        public IEnumerable<Student> GetAll()
        {
           return _studentContext.Students.ToList();
        }

        public void Update(Student dbEntity, Student entity)
        {
            dbEntity.Name = entity.Name;
            dbEntity.Email = entity.Email;
            dbEntity.Course = entity.Course;
            dbEntity.Fees = entity.Fees;
            dbEntity.Image = entity.Image;

            _studentContext.SaveChanges();
        }
       
    }
}
