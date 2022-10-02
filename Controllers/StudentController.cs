using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentManagement.Models;
using StudentManagement.Models.Repository;
using System;
using System.IO;
using System.Reflection;

namespace StudentManagement.Controllers
{
    public class StudentController : Controller
    {
        private readonly StudentContext _studentContext;
        StudentRepository _studentRepository = null;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public StudentController(IWebHostEnvironment hostingEnvironment, StudentContext studentContext)
        {
            _studentContext = studentContext;
            _studentRepository = new StudentRepository(_studentContext);
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {

            return View();
        }
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Create(Student student)
        //{
        //    StudentViewModel studentViewModel = null;
        //    try
        //    {
        //      _studentRepository.Add(student);
        //       studentViewModel = new StudentViewModel(student);

        //        ViewBag.message = "The user " + student.Name + " is saved successfully";
        //        return View("~/Views/Student/ViewStudent.cshtml", studentViewModel);
        //    }
        //    catch (Exception ex)
        //    {

        //    }

        //    return View("~/Views/Student/Index.cshtml",studentViewModel);

        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([FromForm]StudentViewModel studentViewModel)
        {
            try
            {

                var uniqueFileName = UploadAndGetUniqueFileName(studentViewModel);
                var imageDirectoryPath = "\\images\\";

                var student = new Student()
                {
                    Name = studentViewModel.Name,
                    Email = studentViewModel.Email,
                    Gender = studentViewModel.Gender,
                    Course = studentViewModel.Course,
                    Fees = studentViewModel.Fees,
                    Image = imageDirectoryPath + uniqueFileName
                };
                _studentRepository.Add(student);
                studentViewModel = new StudentViewModel(student);

                ViewBag.message = "The user " + student.Name + " is saved successfully";
                return View("~/Views/Student/ViewStudent.cshtml", studentViewModel);
            }
            catch (Exception ex)
            {
                throw;
            }

            return View("~/Views/Student/Index.cshtml", studentViewModel);

        }

        [HttpGet]
        public IActionResult StudentsList()
        {
            StudentViewModel studentViewModel = null;
            try
            {
               var students =  _studentRepository.GetAll();
                studentViewModel = new StudentViewModel(students);

                return View("~/Views/Student/StudentsList.cshtml", studentViewModel);
            }
            catch (Exception ex)
            {
                throw;
            }

            return View("~/Views/Student/Index.cshtml", studentViewModel);

        }
        
        public IActionResult EditStudent(int Id)
        {
            StudentViewModel studentViewModel = null;
            try
            {
               var student = _studentRepository.Get(Id);
                studentViewModel = new StudentViewModel(student);
            }
            catch (Exception ex)
            {

                throw;
            }
            return View("~/Views/Student/EditStudent.cshtml", studentViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditStudent(Student model)
        {
            StudentViewModel studentViewModel = null;
            try
            {
                if(ModelState.IsValid)
                {
                    var existingStudent = _studentRepository.Get(model.Id);

                    Student student = new Student()
                    {
                        Name = model.Name,
                        Email = model.Email,
                        Gender = model.Gender,
                        Course = model.Course,
                        Fees = model.Fees
                    };

                    _studentRepository.Update(existingStudent, student);

                    return RedirectToAction("StudentsList");
                }
                
            }
            catch (Exception ex)
            {
                throw;
            }

            return View("~/Views/Student/Index.cshtml", studentViewModel);
        }

        public IActionResult ViewStudent(int Id)
        {
            StudentViewModel studentViewModel = null;
            try
            {
                var student = _studentRepository.Get(Id);
                studentViewModel = new StudentViewModel(student);
            }
            catch (Exception ex)
            {

                throw;
            }
            return View("~/Views/Student/ViewStudent.cshtml", studentViewModel);
        }
        public IActionResult DeleteStudent(int Id)
        {
            try
            {
                var student = _studentRepository.Get(Id);
                _studentRepository.Delete(student);
            }
            catch (Exception ex)
            {
                throw;
            }
            return RedirectToAction("StudentsList");
        }

        private string UploadAndGetUniqueFileName(StudentViewModel studentViewModel)
        {
            string uniqueFileName = null;

            if (studentViewModel.ImageFile != null)
            {
                uniqueFileName = Guid.NewGuid().ToString() + "_" + studentViewModel.ImageFile.FileName;
                string uploadToFolder = _hostingEnvironment.WebRootPath + "/images/";
                string filePath = Path.Combine(uploadToFolder, uniqueFileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    studentViewModel.ImageFile.CopyToAsync(stream);
                }
            }
            return uniqueFileName;
        }
    }
}
