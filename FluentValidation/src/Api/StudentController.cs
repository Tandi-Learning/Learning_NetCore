using System;
using System.Collections.Generic;
using System.Linq;
using Api.Validations;
using DomainModel;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using CSharpFunctionalExtensions;

namespace Api
{
    [Route("api/students")]
    public class StudentController : FluentApiControllerBase
    {
        private readonly StudentRepository _studentRepository;
        private readonly CourseRepository _courseRepository;

        public StudentController(StudentRepository studentRepository, CourseRepository courseRepository)
        {
            _studentRepository = studentRepository;
            _courseRepository = courseRepository;
        }

        [HttpPost]
        public IActionResult Register(StudentDto request)
        {
            var addresses = request.Addresses.Select(a => new Address(a.Street, a.City, a.State, a.ZipCode)).ToList();

            Result<Email, Error> email = Email.Create(request.Email);
            Result<StudentName> name = StudentName.Create(request.Name);
            if (email.IsFailure) return BadRequest(email.Error);
            if (name.IsFailure) return BadRequest(name.Error);

            var student = new Student(email.Value, name.Value, request.Phone, addresses);
            _studentRepository.Save(student);

            var response = new RegisterResponse
            {
                Id = student.Id
            };
            return Ok(response);
        }

        [HttpPut("{id}")]
        public IActionResult EditPersonalInfo(long id, StudentDto request)
        {
            Student student = _studentRepository.GetById(id);

            var validator = new StudentValidator();
            validator.Validate(request);

            var addresses = request.Addresses.Select(a => new Address(a.Street, a.City, a.State, a.ZipCode)).ToList();
            //student.EditPersonalInfo(new StudentName(request.Name), addresses);
            _studentRepository.Save(student);

            return Ok();
        }

        [HttpPost("{id}/enrollments")]
        public IActionResult Enroll(long id, EnrollRequest request)
        {
            Student student = _studentRepository.GetById(id);

            foreach (CourseEnrollmentDto enrollmentDto in request.Enrollments)
            {
                Course course = _courseRepository.GetByName(enrollmentDto.Course);
                var grade = Enum.Parse<Grade>(enrollmentDto.Grade);
                
                student.Enroll(course, grade);
            }

            return Ok();
        }

        [HttpGet]
        public IActionResult Get()
        {
            IEnumerable<Student> students = _studentRepository.GetAll();
            List<GetResonse> resonses = new List<GetResonse>();

            foreach(var student in students)
            {
                resonses.Add(new GetResonse
                {
                    Addresses = student.Addresses.Select(a => new AddressDto
                    {
                        Street = a.Street,
                        City = a.City,
                        State = a.State,
                        ZipCode = a.ZipCode,
                    }).ToList(),
                    Email = student.Email.Value,
                    Name = student.Name.Value,
                    Phone = student.Phone,
                    Enrollments = student.Enrollments.Select(x => new CourseEnrollmentDto
                    {
                        Course = x.Course.Name,
                        Grade = x.Grade.ToString()
                    }).ToArray()
                });
            }
            
            return Ok(resonses);
        }

        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            Student student = _studentRepository.GetById(id);

            var resonse = new GetResonse
            {
                Addresses = student.Addresses.Select(a => new AddressDto
                {
                    Street = a.Street,
                    City = a.City,   
                    State = a.State, 
                    ZipCode =a.ZipCode
                }).ToList(),
                Email = student.Email.Value,
                Name = student.Name.Value,
                Enrollments = student.Enrollments.Select(x => new CourseEnrollmentDto
                {
                    Course = x.Course.Name,
                    Grade = x.Grade.ToString()
                }).ToArray()
            };
            return Ok(resonse);
        }
    }
}
