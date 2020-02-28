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
                    Gender = Gender.男,
                    Email="daveyeah.net",
                    ClassName=ClassNameEnum.FirstGrade
                },
                new Student
                {
                    Id = 2,
                    FirstName = "Mary",
                    LastName="an",
                    BirthDate =new DateTime(1993,5,10),
                    Gender = Gender.女,
                    Email="marryyeah.net",
                    ClassName=ClassNameEnum.ThirdGrade
                },
                new Student
                {
                    Id = 3,
                    FirstName = "Peter",
                    LastName="chen",
                    BirthDate=new DateTime(1994,5,10),
                    Gender = Gender.男,
                    Email="peteryeah.net",
                    ClassName=ClassNameEnum.FirstGrade
                }
            };
        }

        public Student Add(Student stu)
        {
            var maxId = _student.Max(x => x.Id);
            stu.Id = maxId + 1;
            _student.Add(stu);
            return stu;
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
