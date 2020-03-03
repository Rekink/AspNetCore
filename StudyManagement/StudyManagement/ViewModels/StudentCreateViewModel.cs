using Microsoft.AspNetCore.Http;
using StudyManagement.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudyManagement.ViewModels
{
    public class StudentCreateViewModel
    {    
        [Required(ErrorMessage ="请输入名字")]
        [Display(Name = "名字")]
        [MaxLength(50,ErrorMessage ="名字长度不超过50个字符")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "请输入名字姓氏")]
        [Display(Name = "姓氏"), MaxLength(10)]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "性别")]    
        public Gender? Gender { get; set; }
      
        [Display(Name = "出生日期")]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "请输入邮箱")]
        [Display(Name = "邮箱")]
        [RegularExpression(@"^[a-zA-Z0-9_\-]{1,}@[a-zA-Z0-9_\-]{1,}\.[a-zA-Z0-9_\-.]{1,}$",
            ErrorMessage ="邮箱格式不正确")]
        public string Email { get; set; }

        [Required(ErrorMessage = "请选择班级")]
        [Display(Name = "班级")]
        public ClassNameEnum? ClassName { get; set; }

        [Display(Name = "图片")]
        public List<IFormFile> Photos { get; set; }
    }
}
