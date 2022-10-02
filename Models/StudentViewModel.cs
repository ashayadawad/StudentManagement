using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Models
{
    public class StudentViewModel
    {
        
        public int Id {  get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public string Course { get; set; }
        public int Fees { get; set; }
        public IFormFile ImageFile { get; set; }
        public string ImageFilePath { get; set; }
        public IEnumerable<Student> students { get; set; }

        public StudentViewModel()
        {

        }
        public StudentViewModel(Student student)
        {
            Name = student.Name;
            Email = student.Email;
            Gender = student.Gender;
            Course = student.Course;
            Fees = student.Fees;
            ImageFilePath = student.Image;
        }
        public StudentViewModel(IEnumerable<Student> studentsList)
        {
            students = studentsList;
        }
    }
}
