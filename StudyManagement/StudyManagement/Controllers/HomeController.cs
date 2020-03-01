using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StudyManagement.Model;
using StudyManagement.Services;
using StudyManagement.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StudyManagement.Controllers
{
    //public class HomeController 
    public class HomeController : Controller
    {
        private readonly IStudentRepository<Student> _repository;

        public HomeController(IStudentRepository<Student> repository)
        {
            _repository = repository;
        }
        //public string Index()
        //{
        //    return "Hello from HomeController";
        //}

        //GET: /<controller>/
        public IActionResult Index()
        {
            var list = _repository.GetAll();

            var viewModel = list.Select(x => new StudentViewModel
            {
                Id = x.Id,
                Name = $"{x.FirstName}",
                //Name = $"{x.FirstName} {x.LastName}",
                //Age = DateTime.Now.Subtract(x.BirthDate).Days / 365,
                Email = x.Email,
                ClassName = x.ClassName
            });

            var homeModel = new HomeIndexViewModel
            {
                Students = viewModel
            };
            return View(homeModel);

            //var list = _repository.GetAll();
            //return View(list);

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

        public IActionResult Detail(int id)
        {
            var student = _repository.GetById(id);
            if (student==null)
            {
                return RedirectToAction(nameof(Index));
                //return View("NotFound");
            }
            return View(student);

            //return Content(id.ToString());
        }
        // 默认HttpGet
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        //<form>内自动添加一个隐式的input
        // 用于防止 CSRF（跨站请求伪造）
        // 验证Token
        [ValidateAntiForgeryToken]
        public IActionResult Create(StudentCreateViewModel student)
        {
            if (ModelState.IsValid)
            {
                var stu = new Student
                {
                    FirstName = student.FirstName,
                    LastName = student.LastName,
                    Gender = student.Gender,
                    BirthDate = student.BirthDate,              
                    Email = student.Email,
                    ClassName = student.ClassName,                  
                };

                var newStu = _repository.Add(stu);
                // 防止重复Post 重定向
                return RedirectToAction(nameof(Detail), new { id = newStu.Id });
            }
           
             return View();
            

            //return View("Detail", newStu);

            //return Content(JsonConvert.SerializeObject(student));
        }

    }
}
