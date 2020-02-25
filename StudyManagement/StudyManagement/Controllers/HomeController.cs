using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StudyManagement.Model;
using StudyManagement.Services;
using StudyManagement.ViewModels;

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
                Id=x.Id,
                Name = $"{x.FirstName} {x.LastName}",
                Age = DateTime.Now.Subtract(x.BirthDate).Days/365
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
                return RedirectToAction("Index");
                //return View("NotFound");
            }
            return View(student);

            //return Content(id.ToString());
        }


    }
}
