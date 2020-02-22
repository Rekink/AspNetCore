## AspNetCore
Asp.Net Core 相关学习


### 默认配置、服务注册、管道
Web Host默认配置（Program.cs）
AspNetCoreHostingModel属性：指定应用程序托管形式

 * 进程内托管 InProcess  使用IIS服务器托管(性能更好)
 * 进程外托管 OutProcess 使用Asp.Net Core自带Kestrel服务器托管
 ```c#
 <PropertyGroup>
    <TargetFramework>netcoreapp2.2</TargetFramework>
    <AspNetCoreHostingModel>InProcess</AspNetCoreHostingModel>
  </PropertyGroup>
```
 * Log 中间件
 * IConfiguration接口：通过key获取配置信息<br> 
		appsettings.json、User Secrets、系统环境变量、命令行参数
 
 服务注册
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
 管道<br> 
	一个Post请求常见管道流程：Logger日志、授权、路由、、、
	每一个中间件都是双向管道
 
### 中间件

*按顺序添加执行，能够同时被访问和请求
*处理请求后，可以将请求传递给下一个中间件或者使管道短路
实例：
添加参数ILogger<Startup> logger引入Asp.Net Core自带的日志组件
```c#
  public void Configure(
        IApplicationBuilder app, 
        IHostingEnvironment env,
        IWelcomeService welcomeService,
        ILogger<Startup> logger,
        IConfiguration configuration)
    {
        // 开发环境
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

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

### TagHelpers
