using StudyManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudyManagement.Services
{
    public class InMemoryRepository : IRepository<Student>
    {
        public IEnumerable<Student> GetAll()
        {
            return new List<Student>
            {
                new Student
                {
                    Id = 1,
                    FirstName = "Dave",
                    LastName="jian",
                    Sex = "man"
                },
                new Student
                {
                    Id = 2,
                    FirstName = "Mary",
                    LastName="an",
                    Sex = "female"
                },
                new Student
                {
                    Id = 3,
                    FirstName = "Peter",
                    LastName="chen",
                    Sex = "man"
                }
            };
        }
    }
}
