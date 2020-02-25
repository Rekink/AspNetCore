using StudyManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudyManagement.Services
{
    public class InMemoryRepository : IRepository<Student>
    {
        private readonly List<Student> _student;

        public InMemoryRepository()
        {
            _student = new List<Student>
            {
                new Student
                {
                    Id = 1,
                    FirstName = "Dave",
                    LastName="jian",
                    BirthDate=new DateTime(1991,5,10),
                    Sex = "man"
                },
                new Student
                {
                    Id = 2,
                    FirstName = "Mary",
                    LastName="an",
                    BirthDate =new DateTime(1993,5,10),
                    Sex = "female"
                },
                new Student
                {
                    Id = 3,
                    FirstName = "Peter",
                    LastName="chen",
                    BirthDate=new DateTime(1994,5,10),
                    Sex = "man"
                }
            };
        }

        public IEnumerable<Student> GetAll()
        {        
            return _student;
        }

        public Student GetById(int id)
        {
            return _student.FirstOrDefault(x => x.Id == id);
        }
    }
}
