using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting.Internal;
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

        public readonly HostingEnvironment hostingEnvironment;

        public HomeController(
            IStudentRepository<Student> repository,
            HostingEnvironment hostingEnvironment)
        {
            _repository = repository;
            this.hostingEnvironment = hostingEnvironment;
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
                ClassName = x.ClassName,
                //PhotoPath = x.PhotoPath
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
        public IActionResult Create(StudentCreateViewModel studentCreate)
        {
            if (ModelState.IsValid)
            {
                string uploadFileName = null;

                if (studentCreate.Photos != null&& studentCreate.Photos.Count>0)
                {          
                    uploadFileName = DealUploadFile(studentCreate);
                }

                var stu = new Student
                {
                    FirstName = studentCreate.FirstName,
                    LastName = studentCreate.LastName,
                    Gender = studentCreate.Gender,
                    BirthDate = studentCreate.BirthDate,              
                    Email = studentCreate.Email,
                    ClassName = studentCreate.ClassName, 
                    PhotoPath= uploadFileName
                };

                var newStu = _repository.Add(stu);
                // 防止重复Post 重定向
                return RedirectToAction(nameof(Detail), new { id = newStu.Id });
            }
           
             return View();
            

            //return View("Detail", newStu);

            //return Content(JsonConvert.SerializeObject(student));
        }
        /// <summary>
        /// 编辑视图
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ViewResult Edit(int id)
        {
            Student student = _repository.GetById(id);
            //if (student!=null)
            //{
                StudentEditViewModel studentEditViewModel = new StudentEditViewModel
                {
                    Id = student.Id,
                    FirstName = student.FirstName,
                    LastName = student.LastName,
                    Gender = student.Gender,
                    ClassName = student.ClassName,
                    Email = student.Email,
                    BirthDate = student.BirthDate,
                    ExistingPhotoPath=student.PhotoPath
                };
                return View(studentEditViewModel);
            //}
           
        }

        [HttpPost]
        public IActionResult Edit(StudentEditViewModel studentEdit)
        {          
            if (ModelState.IsValid)
            {
                Student student = _repository.GetById(studentEdit.Id);
                student.FirstName = student.FirstName;
                student.LastName = student.LastName;
                student.Gender = student.Gender;
                student.ClassName = student.ClassName;
                student.Email = student.Email;
                student.BirthDate = student.BirthDate;

                if (studentEdit.Photos.Count > 0)
                {
                    if (studentEdit.ExistingPhotoPath != null)
                    {
                        // 删除原先的图片
                        string filePath = Path.Combine(hostingEnvironment.WebRootPath, "images", studentEdit.ExistingPhotoPath);
                        System.IO.File.Delete(filePath);
                    }
                    
                    student.PhotoPath = DealUploadFile(studentEdit);
                }

                // 保存到数据库
                Student updataStudent=  _repository.Update(student);

                return RedirectToAction(nameof(Detail), new { id = updataStudent.Id });
            }
            return View();
        }

        private string DealUploadFile(StudentCreateViewModel student)
        {
            string uploadFileName = null;

            if (student.Photos.Count > 0)
            {
                foreach (var photo in student.Photos)
                {
                    // 获取文件存放位置wwwroot/images
                    string uploadFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                    // 保存文件目录，确保文件名唯一，文件名添加Guid
                    uploadFileName = Guid.NewGuid().ToString() + "_" + photo.FileName;
                    // 文件的完整目录
                    string filePath = Path.Combine(uploadFolder, uploadFileName);

                    // 数据流处理
                    // 使用非托管资源，需要手动释放
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        // 将文件流复制到指定文件夹下
                        // 使用IFormFile接口的CopyTo()方法将文件复制到指定文件夹
                        photo.CopyTo(fileStream);
                    }
                }
            }
            return uploadFileName;
        }


    }
}
