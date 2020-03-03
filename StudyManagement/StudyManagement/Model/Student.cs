using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudyManagement.Model
{
    public class Student
    {
        public int Id { get; set; }
       
        [Required(ErrorMessage = "请输入名字")]
        [MaxLength(50, ErrorMessage = "名字长度不超过50个字符")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "请输入名字姓氏")]
        public string LastName { get; set; }
     
        public Gender? Gender { get; set; }

        public DateTime BirthDate { get; set; }

        public ClassNameEnum? ClassName { get; set; }

        [Required(ErrorMessage = "请输入邮箱")]
        public string Email { get; set; }

        public string PhotoPath { get; set; }
        
    }
}
