using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudyManagement.Model
{
    public enum Gender
    {
        女=0,
        男=1
    }
    public enum ClassNameEnum
    {
        [Display(Name = "未选择")]
        None,
        [Display(Name = "一年级")]
        FirstGrade,
        [Display(Name = "二年级")]
        SecondGrade,
        [Display(Name = "三年级")]
        ThirdGrade,            
    }
}
