using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudyManagement.Model
{
    public class School
    {
        // 学校和老师的一对多关系：一个学校对应多个老师，一个老师对应一个学校
        public int Id { get; set; }

        public string Name { get; set; }

        public List<Teacher> Teachers { get; set; }
    }
}
