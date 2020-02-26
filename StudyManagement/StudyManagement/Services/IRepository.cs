using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StudyManagement.Model;

namespace StudyManagement.Services
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T GetById(int id);
        T Add(T stu);
    }
}
