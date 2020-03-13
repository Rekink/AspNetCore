using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudyManagement.Model
{
    public class Teacher
    {
        // 在Teacher中定义SchoolID和School模型，在School表中定义Teachers
        public int Id { get; set; }

        public string Name { get; set; }

        public int SchoolID { get; set; }

        public School School { get; set; }
    }
}
