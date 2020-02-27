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
   
        public string FirstName { get; set; }
      
        public string LastName { get; set; }
     
        public Gender Gender { get; set; }

        public DateTime BirthDate { get; set; }

        public string Email { get; set; }

        public string ClassName { get; set; }

    }
}
