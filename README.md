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
 
#### 服务注册
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

#### 路由
* 约定路由：/控制器名称/方法名称（不区分大小写）
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









