using StudyManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudyManagement.Services
{
    public class InMemoryRepository : IStudentRepository<Student>
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
            // 查询语法，看上去和SQL的语句很相似
            var st = from g in _student
                     where g.Gender == Gender.男
                     select g;

            // 方法语法,是命令形式
            st = _student.Where(g => g.Gender == Gender.男);

            var show = _student.GroupJoin(st,
                p => p.FirstName,
                pf => pf.LastName,
                (p, pf) => new
                {
                    Name = p.FirstName,
                    Ye = pf.Max(pf1 => pf1.LastName)
                });


            var maxId = _student.Max(x => x.Id);
            stu.Id = maxId + 1;
            _student.Add(stu);
            return stu;
        }

        public Student Delete(int id)
        {
            Student student = _student.FirstOrDefault(s => s.Id == id);
            if (student!=null)
            {
                _student.Remove(student);
            }
            return student;
        }

        public IEnumerable<Student> GetAll()
        {        
            return _student;
        }

        public Student GetById(int id)
        {
            return _student.FirstOrDefault(x => x.Id == id);
        }

        public Student Update(Student updateStudent)
        {
            Student student = _student.FirstOrDefault(s=>s.Id==updateStudent.Id);
            if (student != null)
            {
                student.FirstName = updateStudent.FirstName;
                student.LastName = updateStudent.LastName;
                student.Gender = updateStudent.Gender;
                student.Email = updateStudent.Email;
                student.BirthDate = updateStudent.BirthDate;
                student.ClassName = updateStudent.ClassName;
            }
            return student;
        }
    }
}
