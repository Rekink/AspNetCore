using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StudyManagement.Model;
using StudyManagement.Services;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StudyManagement.Controllers
{
    //public class HomeController 
    public class HomeController : Controller
    {
        private readonly IRepository<Student> _repository;

        public HomeController(IRepository<Student> repository)
        {
            _repository = repository;
        }

        //GET: /<controller>/
        public IActionResult Index()
        {
            var list = _repository.GetAll();

            return View(list);

            //// 决定做什么，不具体处理（MVC具体处理）
            //var student = new Student
            //{
            //    Id = 1,
            //    FirstName = "Dave",
            //    LastName="jian",
            //    Sex = "man"
            //};
            //// 返回视图
            //return View(student);
            //// 返回Json
            //return new ObjectResult(student); 
            ///// 返回文本
            ///return Content("Hello from HomeController (IActionResult)");

        }
        //public string Index()
        //{
        //    return "Hello from HomeController";
        //}
    }
}
