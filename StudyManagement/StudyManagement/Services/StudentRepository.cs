using StudyManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudyManagement.Services
{
    public class StudentRepository : IStudentRepository<Student>
    {
        public StudentRepository(AppDbContext context)
        {
            _context = context;
        }

        public AppDbContext _context { get; }

        public Student Add(Student stu)
        {
            _context.Students.Add(stu);
            _context.SaveChanges();
            return stu;
        }

        public Student Delete(int id)
        {
            Student student = _context.Students.Find(id);
            if (student!=null)
            {
                _context.Students.Remove(student);
                _context.SaveChanges();
            }
            return student;
        }
    
        public Student Update(Student updateStudent)
        {
            //打标签
            var student = _context.Students.Attach(updateStudent);
            student.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return updateStudent;
        }

        IEnumerable<Student> IStudentRepository<Student>.GetAll()
        {
            return _context.Students;
        }

        Student IStudentRepository<Student>.GetById(int id)
        {
            return _context.Students.Find(id);
        }
    }
}
