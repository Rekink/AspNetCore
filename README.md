## AspNetCore
Asp.Net Core 相关学习


### 默认配置、服务注册、管道、中间件

#### Web Host默认配置（Program.cs） 
AspNetCoreHostingModel属性：指定应用程序托管形式
 * 进程内托管 InProcess  使用IIS服务器托管，IIS 只做 Web 请求转(性能更好)
 * 进程外托管 OutProcess 使用Asp.Net Core自带Kestrel服务器托管
 ```c#
 <PropertyGroup>
    <TargetFramework>netcoreapp2.2</TargetFramework>
    <AspNetCoreHostingModel>InProcess</AspNetCoreHostingModel>
  </PropertyGroup>
```
 * Log 中间件
 * IConfiguration接口：通过key获取配置信息<br> 
		配置信息来源：appsettings.json、User Secrets、系统环境变量、命令行参数
 
#### 服务注册（依赖注入容器）
依赖注入:低耦合、高测试性，更加方便进行单元测试
 * AddSingleton 单例模式，整个Web生命周期，只有一个实例
 * AddTransient 每次请求IWelcomeService都会创建WelcomeService实例
 * AddScoped 一次Web请求创建一个WelcomeService实例，期间多次请求会继续使用同一个实例
 ```c#
   public void ConfigureServices(IServiceCollection services)
   {
        // 单例模式，整个Web生命周期，只有一个实例
        services.AddSingleton<IWelcomeService, WelcomeService>();
        //// 每次请求IWelcomeService都会创建WelcomeService实例
        //services.AddTransient<IWelcomeService, WelcomeService>();
        //// 一次Web请求创建一个WelcomeService实例，期间多次请求会继续使用同一个实例
        //services.AddScoped<IWelcomeService, WelcomeService>();
   }
 ```
 
#### 管道 
每一个中间件都是双向管道<br>
一个Post请求常见管道流程：<br>
* Logger日志：记录日志
* 授权：根据cookie或token权限判定
* 路由：根据请求URL确定调用哪个控制器的哪个方法

#### 中间件
* 按顺序添加执行，能够同时被访问和请求
* 处理请求后，可以将请求传递给下一个中间件或者使管道短路<br>
实例：添加参数ILogger<Startup> logger引入Asp.Net Core自带的日志组件
```c#
  public void Configure(
        IApplicationBuilder app, 
        IHostingEnvironment env,
        IWelcomeService welcomeService,
        ILogger<Startup> logger,
        IConfiguration configuration)
    {
        if (env.IsDevelopment())
        {
		    var developerExceptionPageOptions = new DeveloperExceptionPageOptions();
            // 显示代码行数
            developerExceptionPageOptions.SourceCodeLineCount = 10;
            // 开发者捕获异常页面
            app.UseDeveloperExceptionPage();
        }
		
        //app.Run(async (context) =>
        //{
        //    throw new Exception("自己抛出异常，测试开发者异常页面");
        //});

        // 设置环境变量名
        //env.EnvironmentName = "IntegrationTest";
        // 是否是IntegrationTest环境
        //env.IsEnvironment("IntegrationTest");
        // 常见环境变量：Development开发环境、 Staging演示（模拟、临时）环境、Production正式（生产）环境
        
		//app.UseWelcomePage();

        app.Use(async (context, next) =>
        {
            context.Response.ContentType = "text/plain;charset=utf-8";

            //await context.Response.WriteAsync("Hello!");

            logger.LogDebug("M1: 传入请求");
            await next();
            logger.LogDebug("M1: 传出响应");
        });

        app.Use(async (context, next) =>
        {
            context.Response.ContentType = "text/plain;charset=utf-8";

            logger.LogDebug("M2: 传入请求");
            await next();
            logger.LogDebug("M2: 传出响应");
        });

        app.Run(async (context) =>
        {
            //await context.Response.WriteAsync("你好！");
            await context.Response.WriteAsync("M3: 处理请求，生成响应");
            logger.LogDebug("M3: 处理请求，生成响应");
        });



        app.Run(async (context) =>
        {
            //var welcome = configuration["welcome"];
            var welcome = welcomeService.GetMesssge();
            await context.Response.WriteAsync(welcome);
        });
    }
```
输出日志：
```c#
“dotnet.exe”(CoreCLR: clrhost): 已加载“C:\Program Files\dotnet\shared\Microsoft.NETCore.App\2.1.15\System.Private.Uri.dll”。已跳过加载符号。模块进行了优化，并且调试器选项“仅我的代码”已启用。
“dotnet.exe”(CoreCLR: clrhost): 已加载“C:\Program Files\dotnet\shared\Microsoft.AspNetCore.App\2.1.15\Microsoft.AspNetCore.WebUtilities.dll”。已跳过加载符号。模块进行了优化，并且调试器选项“仅我的代码”已启用。
StudyManagement.Startup:Debug: M1: 传入请求
Microsoft.AspNetCore.Hosting.Internal.WebHost:Information: Request finished in 40.7372ms 200 
StudyManagement.Startup:Debug: M2: 传入请求
StudyManagement.Startup:Debug: M3: 处理请求，生成响应
StudyManagement.Startup:Debug: M2: 传出响应
StudyManagement.Startup:Debug: M1: 传出响应
Microsoft.AspNetCore.Hosting.Internal.WebHost:Information: Request finished in 57.9499ms 200 text/plain;charset=utf-8
Microsoft.AspNetCore.Hosting.Internal.WebHost:Information: Request starting HTTP/1.1 GET http://localhost:64574/favicon.ico  
StudyManagement.Startup:Debug: M1: 传入请求
StudyManagement.Startup:Debug: M2: 传入请求
StudyManagement.Startup:Debug: M3: 处理请求，生成响应
StudyManagement.Startup:Debug: M2: 传出响应
StudyManagement.Startup:Debug: M1: 传出响应
Microsoft.AspNetCore.Hosting.Internal.WebHost:Information: Request finished in 9.3604ms 200 text/plain;charset=utf-8
```

### MVC服务

#### Controller、Model、View关注点分离
* Controller：接收请求，构建 Model，选择 View
* Model：携带数据信息类和管理该数据的逻辑信息
* View：将 Model 转换为 HTML 页面<br>
 
 （快速添加类属性prop+Tab+tab）
 
#### 路由
* 约定路由：/控制器名称/方法名称（不区分大小写）MapRoute
```c#
app.UseMvc(builder=>
{
    // /home/Index/1
    //builder.MapRoute("Default", "{controller}/{action}/{id?}");
    builder.MapRoute("Default","{controller=home}/{action=index}/{id?}");

});
```
* 特性路由(API常用)
```c#
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
```
#### Controller
Controller父类,提供l 很多上下文相关信息及封装方法，例如this.File() 返回文件<br>
返回值：
* 返回sting，int时，会立即返回
* 返回IActionResult时,不会立即返回值，只会决定返回什么值<br>
  具体的返回值操作由 MVC 框架来做
```c#
 public IActionResult Index()
    {

        // 决定做什么，不具体处理（MVC具体处理）
        var student = new Student
        {
            Id = 1,
            FirstName = "Dave",
            LastName = "jian",
            Sex = "man"
        };
		//// 返回视图
        //return View(student);
        // 返回Json
        return new ObjectResult(student);
		//// 返回文本
        ///return Content("Hello from HomeController (IActionResult)");

    }
```

#### TagHelper
添加TagHelper，View文件夹中添加Razor视图导入（import）
```c#
@addTagHelper * , Microsoft.AspNetCore.Mvc.TagHelpers
```

TagHelper使用
```c#
@foreach (var i in Model.Students)
{
    @*<li>@i.Name (@i.Age) <a href="/Home/Detail/@i.Id">Detail</a> </li>*@
    <li>
        @i.Name (@i.Age)
        <a asp-controller="Home" 
           asp-action="Detail" 
           asp-Route-id="@i.Id">Detail
       </a>        
    </li>
}
```

#### 模型绑定、验证
* Input Model
在View填写并提交的Model不包含的Id属性<br>
如果提交方法使用Student类型作为Model<br>
MVC框架会想尽办法在提交的信息里面找Id属性<br>
最后导致出现意想不到的状况<br>
因此需要创建与View 提交的 Model 属性一致的 Input Model（StudentCreateViewModel）

* 防止重复Post
使用AddSingleton注册服务时，<br>
提交Model可能会出现重复提交的情况，例如，刷新提交页面，会重复Post请求<br>
Post-Redirect-Get模式重定向防止重复提交（RedirectToAction）

```c#
[HttpPost]
public IActionResult Create(StudentCreateViewModel student)
{
    var stu = new Student
    {
        FirstName = student.FirstName,
        LastName = student.LastName,
        BirthDate = student.BirthDate,
        Gender = student.Gender
    };

    var newStu = _repository.Add(stu);

    return RedirectToAction(nameof(Detail), new { id = newStu.Id });

    //return View("Detail", newStu);

    //return Content(JsonConvert.SerializeObject(student));
}
```

提交URL：
```c#
http://localhost:64574/home/Create
```

重定向后URL：
```c#
http://localhost:64574/home/Detail/5
```
刷新只会请求学生信息页面

* Data Annotations数据注解
在模型绑定的同时，会做验证<br>
验证信息可以在ModelState中查看
```c#
[Required]
[Display(Name = "名"), MaxLength(10)]
[DataType(DataType.Password)]
[Display(Name = "邮箱")]
[RegularExpression(@"\w + ([-+.]\w +)*@\w+([-.]\w+)*\.\w+([-.]\w+)*",
ErrorMessage ="邮箱格式不正确")]
```


### EF Core

#### 实现DbContext
* 将应用程序的配置传递给DbContext
```c#
public void ConfigureServices(IServiceCollection services)
{
    services.AddDbContextPool<AppDbContext>(
        options => options.UseSqlServer(_configuration.GetConnectionString("StudentDBConnection"))
        );

    services.AddScoped<IStudentRepository<Student>, StudentRepository>();

    // 一般用这个，功能多
    services.AddMvc();
}
```
* DbSet：对使用到的每个实体添加 DbSet<TEntity> 属性<br>
通过DbSet属性来进行增删改查操作,对DbSet采用Linq查询的时候<br>
EFCore自动将其转换为SQL语句
```c#
public class AppDbContext : DbContext
{
    /// <summary>
    /// 将应用程序的配置传递给DbContext
    /// </summary>
    /// <param name="option"></param>
    public AppDbContext(DbContextOptions<AppDbContext> option) : base(option)
    {

    }

    // 对要使用到的每个实体都添加 DbSet<TEntity> 属性
    // 通过DbSet属性来进行增删改查操作
    // 对DbSet采用Linq查询的时候，EFCore自动将其转换为SQL语句
    public DbSet<Student> Students { get; set; }

    // 重写
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // 添加种子数据
        modelBuilder.Seed();

        //modelBuilder.Entity<Student>().HasData(
        //    new Student
        //    {
        //        Id = 1,
        //        FirstName = "han",
        //        LastName = "zhou",
        //        Gender = Gender.男,
        //        BirthDate = new DateTime(1991, 5, 10),
        //        ClassName = ClassNameEnum.FirstGrade,
        //        Email = "rekink@yeah.net"
        //    });
        base.OnModelCreating(modelBuilder);
    }
}
```
* 将应用程序的配置传递给DbContext，主要是数据库连接字符串
```c#
 services.AddDbContextPool<AppDbContext>(
            options => options.UseSqlServer(_configuration.GetConnectionString("StudentDBConnection"))
            );

services.AddScoped<IStudentRepository<Student>, StudentRepository>();


```
#### 实现仓储
```c#
//定义一个IStudentRepository的接口，并定义一个泛型
//泛型规定了必须是:class 这样的类或者子类 
public interface IStudentRepository<T> where T : class
{
    IEnumerable<T> GetAll();
    T GetById(int id);
    T Add(T stu);
    T Update(T updateStudent);
    T Delete(int id);
}

public class StudentRepository : IStudentRepository<Student>
{
    public StudentRepository(AppDbContext context)
    {
        _context = context;
    }

    public AppDbContext _context { get; }

    public Student Add(Student stu)
    {
        _context.Students.Add(stu);
        _context.SaveChanges();
        return stu;
    }

    public Student Delete(int id)
    {
        Student student = _context.Students.Find(id);
        if (student!=null)
        {
            _context.Students.Remove(student);
            _context.SaveChanges();
        }
        return student;
    }

    public Student Update(Student updateStudent)
    {
        //打标签
        var student = _context.Students.Attach(updateStudent);
        student.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        _context.SaveChanges();
        return updateStudent;
    }

    IEnumerable<Student> IStudentRepository<Student>.GetAll()
    {
        return _context.Students;
    }

    Student IStudentRepository<Student>.GetById(int id)
    {
        return _context.Students.Find(id);
    }
}
```
#### EFCore常用指令
* Add-Migration：添加迁移记录
* Update-Database：更新数据库
数据迁移，用于同步领域模型和数据库架构设计，使它们保持一致<br>
程序包管理控制台/PM:
* 创建新的迁移记录Add-Migration SeedData添加种子数据
* 更新数据库架构Update-DataBase
* 删除未应用到数据库迁移记录(未执行update操作)  Remove-Migration
* 数据库迁移记录信息保存在EFMigrationHistory文件中行
* 模型快照 ModelSnapshot 用于确定将在下一次迁移发生的变化

添加种子数据，重写DbContext的OnModelCreating方法
```c#
 protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    // 添加种子数据
    modelBuilder.Seed();

    //modelBuilder.Entity<Student>().HasData(
    //    new Student
    //    {
    //        Id = 1,
    //        FirstName = "han",
    //        LastName = "zhou",
    //        Gender = Gender.男,
    //        BirthDate = new DateTime(1988, 5, 10),
    //        ClassName = ClassNameEnum.FirstGrade,
    //        Email = "rekink@yeah.net"
    //    });
    base.OnModelCreating(modelBuilder);
}

// 扩展方法Seed
public static class ModelBuilderExtensions
{
    public static void Seed(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Student>().HasData(
            new Student
            {
                Id = 1,
                FirstName = "han",
                LastName = "zhou",
                Gender = Gender.男,
                BirthDate = new DateTime(1991, 5, 10),
                ClassName = ClassNameEnum.FirstGrade,
                Email = "rekink@yeah.net"
            },
            new Student
            {
                Id = 2,
                FirstName = "ke",
                LastName = "zhou",
                Gender = Gender.男,
                BirthDate = new DateTime(1991, 5, 10),
                ClassName = ClassNameEnum.ThirdGrade,
                Email = "rekinz@qq.com"
            });
    }
}
```




















