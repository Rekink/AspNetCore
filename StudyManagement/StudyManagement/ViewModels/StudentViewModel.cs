﻿using StudyManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudyManagement.ViewModels
{
    public class StudentViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ClassNameEnum? ClassName { get; set; }

        public string Email { get; set; }

        public string PhotoPath { get; set; }

    }
}
