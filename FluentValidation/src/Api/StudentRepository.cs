﻿using System.Collections.Generic;
using System.Linq;
using DomainModel;

namespace Api
{
    public class StudentRepository
    {
        private static readonly List<Student> _existingStudents = new List<Student>
        {
            Alice(),
            Bob()
        };
        private static long _lastId = _existingStudents.Max(x => x.Id);

        public IEnumerable<Student> GetAll()
        {
            // Retrieving from the database
            return _existingStudents.ToList();
        }

        public Student GetById(long id)
        {
            // Retrieving from the database
            return _existingStudents.SingleOrDefault(x => x.Id == id);
        }

        public void Save(Student student)
        {
            // Setting up the id for new students emulates the ORM behavior
            if (student.Id == 0)
            {
                _lastId++;
                SetId(student, _lastId);
            }

            // Saving to the database
            _existingStudents.RemoveAll(x => x.Id == student.Id);
            _existingStudents.Add(student);
        }

        private static void SetId(Entity entity, long id)
        {
            // The use of reflection to set up the Id emulates the ORM behavior
            entity.GetType().GetProperty(nameof(Entity.Id)).SetValue(entity, id);
        }

        private static Student Alice()
        {
            var email = Email.Create("alice@gmail.com");
            var name = StudentName.Create("Alice Alison");
            var alice = new Student(email.Value, name.Value, "202-555-1234",
                new List<Address>
                {
                    new Address("1234 Main St", "Arlington", "VA", "22201")
                }
            );
            SetId(alice, 1);
            alice.Enroll(new Course(1, "Calculus", 5), Grade.A);

            return alice;
        }

        private static Student Bob()
        {
            var email = Email.Create("bob@gmail.com");
            var name = StudentName.Create("Bob Bobson");
            var bob = new Student(email.Value, name.Value, "305-555-6688",
                new List<Address>
                {
                    new Address("2345 Second St", "Barlington", "VA", "22202")
                }
            );
            SetId(bob, 2);
            bob.Enroll(new Course(2, "History", 4), Grade.B);
            
            return bob;
        }
    }
}
