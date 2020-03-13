using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudyManagement.Model
{
    public class Desk
    {
        // 学生和桌子的一对一关系
        // 每个学生需要对应一个桌位信息，桌位信息不用包含学生信息
        public int Id { get; set; }

        public string Name { get; set; }

        //public Student Student { get; set; }
    }
}
