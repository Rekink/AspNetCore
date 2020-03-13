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
Entity Framework的全称为ADO.NET Entity Framework，是在ADO.NET上层实现的ORM（对象关系映射）封装<br>
 ![Image text](https://github.com/Rekink/AspNetCore/raw/master/pic/EF.png) <br>

EF Core 是一个ORM（对象关系映射），它使.NET开发人员可以使用.NET对象操作数据库，<br>
避免了像ADO.NET访问数据库的代码，开发者只需要编写对象即可<br>

EF Core 支持多种数据库引擎：SQL Sever、MySQL、SQLite、Npgsql、、、

EF的三种编程模式：
* DataBase First：开发模式指以数据库设计为基础，并根据数据库自动生成实体数据模型
* Model First：是指从建立实体数据模型入手，并依据模型生成数据库，从而驱动整个开发流程
该模式为面向领域的编程模式，优点在于，可以用与设计建模相同的思维来进行代码编写，更符合面向对象的思<br>
Model First与Database First是互逆的，但最终都是输出数据库和实体数据模型
* Code First：完全通过手动编码，就可以使用Entity Framewokr技术来实现数据访问。
该模式的优点在于，支持POCO（Plain Old CLR Objects，简单传统CLR对象），代码整洁，程序员对代码的控制也更灵活自如<br>

Code First 是通过EF迁移来完成从模型生成数据库。

Entity Framework的使用分为以下四步：
* 连接配置：如何与数据库进行连接。我们可以通过连接字符串或者代码进行设置
* 关系映射:SSDL存储模型 MSL关系映射 CSDL概念模型              
* 上下文环境定义：DBContext 可以理解为是一个容器，里面有对象与数据表的映射关系以及对象本身。
* 数据持久化：数据持久化就是将内存中的概念模型转换为存储模型,以及将存储模型转换为内存中的数据模型的统称。


#### 获取EF Core
* 通过NuGet获取要使用的数据库支持。比如：Microsoft SQL Sever
* 打开NuGet程序包管理器控制台，输入：Install-Package Microsoft.EntityFrameworkCore.SqlServer


#### EF Core将类映射到数据库
EF Core通过一个模型进行数据库访问的<br>
EF Core将类映射为数据库中的表. 每一张表对应创建一个实体类,或者你已有一个数据库则需要匹配数据库表<br> 
有很多的规则和配置, 下图给出的是映射到数据库表的实体类的一般格式
 ![Image text](https://github.com/Rekink/AspNetCore/raw/master/pic/map.png)


#### 数据库上下文DbContext 
控制台应用程序的DbContext: 覆盖应用程序的DbContext的OnConfiguring方法,提供EF Core需要的数据库类型和连接字符串等其他信息<br>

![Image text](https://github.com/Rekink/AspNetCore/raw/master/pic/config.png)

这种方法的缺点是它的连接字符串是固定的,不利于单元测试，而且对于不同的环境（开发,测试,生产）我们会使用不同的数据库<br>

以下是.net core配置方法:配置文件中设置appSettings节点来配置数据库初始化上下文
```c#
 public void ConfigureServices(IServiceCollection services)
{
    // UseSqlServer代表的是SqlServer数据库 
    services.AddDbContextPool<AppDbContext>(
        options => options.UseSqlServer(_configuration.GetConnectionString("StudentDBConnection"))
        );

    services.AddScoped<IStudentRepository<Student>, StudentRepository>();
	
    services.AddMvc();
}
```

#### EF Core工作机制
![Image text](https://github.com/Rekink/AspNetCore/raw/master/pic/process.png)

* EF Core查看DbContext并找到所有公共的DbSet属性,并使用属性名为表定义初始名称.

* EF Core查看DbSet的泛型类,查看类的属性构建列名,类型等. 它还会查找类和属性用于提供额外建模配置的特殊Attribute

* EF Core查找DbSet类中引用的其他类. 在我们的例子中Book类有一个对Author类的引用,它对Author类执行与步骤2相同的操作. 同时它使用类名Author做为表名

* EF Core运行DbContext的虚方法OnModelCreating, 可以通过重写OnModelCreating方法进行更多的配置

* EF Core根据收集的信息创建数据库的内部模型,并缓存数据库模式,以便提升访问速度.

以上操作都没有涉及到数据库操作,增删改查等操作都是通过DbSet属性来进行,对DbSet采用Linq查询的时候,EFCore自动将其转换为SQL语句<br>



DbSet：对使用到的每个实体添加 DbSet<TEntity> 属性<br>

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
        //        Email = "rekink@yeah.net"
        //    });
        base.OnModelCreating(modelBuilder);
    }
}
```


#### EF Core常用指令

程序包管理控制台/PM:
* Add-Migration：添加迁移记录
* Update-Database：更新数据库

数据迁移，用于同步领域模型和数据库架构设计，使它们保持一致<br>

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
    //        Gender = Gender.男,
    //        BirthDate = new DateTime(1988, 5, 10),
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
                Gender = Gender.男,
                BirthDate = new DateTime(1991, 5, 10),
                Email = "rekink@yeah.net"
            },
            new Student
            {
                Id = 2,
                FirstName = "ke",
                Gender = Gender.男,
                BirthDate = new DateTime(1991, 5, 10),
                Email = "rekinz@qq.com"
            });
    }
}
```

#### EF Core存储过程、视图
在处理许多复杂逻辑的数据处理时，会用到SQL和存储过程的方式去处理

* DbSet<TEntity>扩展方法FromSql：
DbSet<TEntity>.FromSql();<br>
结果一定要是实体类型，就是数据库表映射的模型<br>
在执行的带参数的sqlSQL语句中返回所有列，并且列名必须与实体属性名相匹配，否则执行会出错<br>
这意味着，执行存储过程返回的结果一定是跟数据库表相关的所有字段<br>
FromSql方式的结果不能有关联关系数据,相当于不能join ，也返回不了join关联表的数据。

* DbContext有一个Database属性，其包括一个方法ExecuteSqlCommand：
DbContext.Database.ExecuteSqlCommand()；<br>
此方法返回一个整数，表示执行的SQL语句影响的行数<br>
有效的操作，非查询语句，INSERT、UPDATE和DELETE的存储过程，但不能用于返回实体。<br>


* 还可以用DbCommand简单的扩展数据框架上下文对象，使其可以执行存储过程并返回你想要的数据类型


通过EFCore多表联合查询方式，无论是通过Join或者Include方式在操作上都不是很方便，<br>
EF Core是不支持存储过程及视图的映射的,直接通过DbContext是没有办法直接调用到视图的<br>
可以为视图View创建对应的实体类。

```c#
protected override void OnModelCreating(ModelBuilder modelBuilder)
{           
    modelBuilder.Query<ViewEntrustWithJyDataTestNumMatchPile>().ToView("View_EntrustWithJyDataTestNumMatchPile");
}

/// <summary>
/// 更新委托单已测数量
/// </summary>
[UnitOfWork]
public void UpdateTestNum()
{
    var jyList = context.Query<ViewEntrustWithJyDataTestNumMatchPile>().Where(m => m.MeasuredNum != m.oldMeasuredMum).ToList();
｝

// 视图对应的实体
/// <summary>
/// 获取委托单的已测数量（匹配桩号）
/// </summary>
public class ViewEntrustWithTestDataTestNumMatchPile
{
    /// <summary>
    /// 委托ID MeasuredNum, EntrustId
    /// </summary> 
    public int EntrustId { get; set; }
    /// <summary>
    /// 统计已测数量
    /// </summary> 
    public int MeasuredNum { get; set; }
    /// <summary>
    /// 租户ID
    /// </summary> 
    public int? TenantId { get; set; }
    /// <summary>
    /// 数据库已测数量
    /// </summary> 
    public int oldMeasuredMum { get; set; } 
}
```


#### EF对象状态
当从数据库中查询出数据时，上下文就会创建实体快照，从而追踪实体<br>
在调用 SaveChanges 时，扫描上下文中所有实体，并比较当前属性值和存储在快照中的原始属性值<br>
如果被找到的属性值发生了改变，实体的任何更改都会保存到数据库中<br>

EF 跟踪对象状态有五个状态：
* Detached（游离态，脱离态） 
* Unchanged（未改变）
* Added（新增）
* Deleted（删除）
* Modified（被修改）
EF 会跟踪对象状态的改变，当执行SavaChanged()方法期间，会查看当前对象的EntityState的值，<br>
决定是去新增、修改、删除或者什么也不做


#### 优化
* Model实体字段类型（byte）
* ViewModel代替实体Model
* 对于只读操作，查询出来的对象只是供显示使用，不需要修改、删除后保存，那么可以使用AsNoTracking()<br>
AsNoTracking查询出来的对象是Detached状态，这样对对象的修改也还是 Detached状态，EF将不再跟踪这个对象状态的改变，能够提升性能。
同时由于没有受到上下文的跟踪缓存，因此取得的数据也是及时最新的，更利于某些对数据及时性要求高的数据查询。<br>
AsNoTracking()是 DbQuery 类（DbSet的父类）的方法，要先在DbSet后，调用AsNoTracking()方法
```c#
var p1 = ctx.Persons.Where(p => p.Name == "baidu.com").FirstOrDefault();
Console.WriteLine(ctx.Entry(p1).State);

//改成：
var p1 = ctx.Persons.AsNoTracking().Where(p => p.Name == "baidu.com").FirstOrDefault();
Console.WriteLine(ctx.Entry(p1).State);
```
如果确实还想再更新，使用ctx.Entry().State=System.Data.Entity.EntityState.Unchanged;进行强制状态转换后，即可再更新。


#### LINQ
LINQ是一组语言特性和API，可以使用统一的方式编写各种查询。用于保存和检索来自不同数据源的数据，<br>
从而消除了编程语音和数据库之间的不匹配，以及为不同类型的数据源提供单个查询接口。
* 是面向对象的SQL，SQL是关系型数据库查询，LINQ是对内存里的数据的查询
* 通过表达式分析与实体到关系的映射，把linq转换为sql语句或是对xml的查询
* 数据库到对象结构的一个中间层，将关系数据的管理转换为对象的操作，专心于对象模型的处理

LINQ主要包含三个部分：
* LINQ to Objects  主要负责对象的查询。
* LINQ to XML      主要负责XML的查询。
* LINQ to ADO.NET  主要负责数据库的查询，包括，LINQ to SQL、LINQ to DataSet、LINQ to Entities


常见的语法：
* from 指定范围变量和数据源
* where 根据bool表达式从数据源中筛选数据
* select 从指定查询结果中的元素中，筛选出符合条件的数据
```c#
// 查询语法，看上去和SQL的语句很相似
var st = from g in _student
         where g.Gender == Gender.男
         select g;

// 方法语法,是命令形式
st = _student.Where(g => g.Gender == Gender.男);
```

* orderby/OrderBy() 对查询出的元素进行排序（默认升序）
* orderby descending/OrderByDescending() 对查询出的元素进行降序排列
* group/GroupBy() 可产生按照指定的键组织的组序列,元素分组后的结果

* Join()按照两个指定匹配条件来Equals连接两个数据源
在表关系中有一对一关系，一对多关系，多对多关系等
```c#
// Customers与Orders是一对多关系,使用外键导航选择伦敦客户的所有订单
var query =
    from c in db.Customers
    from o in c.Orders
    where c.City == "London"
    select o;
```

多对多关系
```c#
// 使用外键导航筛选在WH的雇员，同时列出其所在地区
var query =
    from e in db.Employees
    from et in e.EmployeeAddress
    where e.City == "WH"
    select new
    {
        e.FirstName,
        e.LastName,
        et.Address.AddressDescription
    };
```
在Join操作中，分别为Join(Join查询), SelectMany(Select一对多选择)和GroupJoin(分组Join查询)<br>

一对多关系：
```c#
// Customers与Orders是一对多关系。即Orders在Customers类中以EntitySet形式出现
// 所以第二个from是从c.Orders而不是db.Orders里进行筛选
// 在From子句中使用外键导航选择伦敦客户的所有订单
var query =
    from c in db.Customers
    from o in c.Orders
    where c.City == "London"
    select o;
```

多对多关系：
```c#
// 多对多关系一般会涉及三个表(如果有一个表是自关联的，那有可能只有2个表)
// 这一句语句涉及Employees, EmployeeTerritories, Territories三个表。
// 它们的关系是1：M：1。Employees和Territories没有很明确的关系。
// 在From子句中使用外键导航筛选在西雅图的雇员，同时列出其所在地区
var query =
    from e in db.Employees
    from et in e.EmployeeTerritories
    where e.City == "Seattle"
    select new
    {
        e.FirstName,
        e.LastName,
        et.Territory.TerritoryDescription
    };
```

* Union()用于将两个输入序列中的元素合并成一个新的序列，且新序列中自动去除重复的序列
* Intersect()求两个序列的交集，将两个序列中相同的元素挑选出来组成一个新的序列
* Except()现有A、B两序列，返回仅在A序列中的元素所组成的序列，相当于求差集
* Contact()联接两个序列
* Distinct检测每一个输入元素是否有相同的，如果有相同的元素则作为一个元素添加到结果序列中，相当于去除重复




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


#### 视图View

* _Layout.cshtml：外层模版，页面共用固定区域

* _ViewStart.cshtml：存放所有视图文件的公共代码,<br>
_ViewStart.cshtml文件所在目录下的每个视图文件开始渲染，先执行这个文件的内容<br>
一般直接放在Views目录下，全局生效，可以放在各个子文件夹下，这样可以覆盖全局的_ViewStart.cshtml

* _ViewImports.cshtml：引用命名空间、注册模型、引入TagHelper等等<br>
作用于其所在文件夹及子文件夹内的视图文件。

* 视图节点 Section
在布局视图里渲染节点
```c#
 @*布局视图里渲染节点*@
 @*false代表可有可无*@
 @RenderSection("Scripts", required: false)
```
在一般视图里定义节点
```c#
@section Scripts{ 
    <script type="text/javascript">
        $(document).ready(function () {
            //绑定事件
            $('.custom-file-input').on("change", function () {
                //单文件上传
                //C: \fakepath\wallhaven - 710582.jpg
                //console.log($(this).val());
                //var fileName = $(this).val().split("\\").pop();
                //$(this).next(".custom-file-label").html(fileName);
                //多文件上传
                var fileLable = $(this).next(".custom-file-label");
                var files = $(this)[0].files;
                if (files.length > 1) {
                    fileLable.html('你已选择 ' + files.length + ' 个文件');
                } else if (files.length == 1) {
                    fileLable.html(files[0].name);
                }
            });
        });
    </script>
```

















