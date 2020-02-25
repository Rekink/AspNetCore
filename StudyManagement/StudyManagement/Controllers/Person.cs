using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudyManagement.Controllers
{
    //[Route("person")]
    //[Route("[controller]")]
    [Route("[controller]/[action]")]
    public class PersonController
    {
        //[Route("")]
        public string IsDave()
        {
            return "Dave";
        }

        //[Route("IsMarry")]
        public string IsMarry()
        {
            return "Marry";
        }
    }
}
