using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StudyManagement.Model;

namespace StudyManagement.Services
{
    //定义一个IStudentRepository的接口，并定义一个泛型
    //泛型规定了必须是:class 这样的类或者子类 
    public interface IStudentRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T GetById(int id);
        T Add(T stu);
        T Update(T updateStudent);
        T Delete(int id);

    }
}
