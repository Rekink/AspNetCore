using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StudyManagement.Model;
using StudyManagement.Services;

namespace StudyManagement
{
    public class Startup
    {
        public IConfiguration _configuration { get; }

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextPool<AppDbContext>(
                options => options.UseSqlServer(_configuration.GetConnectionString("StudentDBConnection"))
                );

            services.AddScoped<IStudentRepository<Student>, StudentRepository>();


            //services.AddSingleton<IStudentRepository<Student>, InMemoryRepository>();

            // 单例模式，整个Web生命周期，只有一个实例
            services.AddSingleton<IWelcomeService, WelcomeService>();
            //// 每次请求IWelcomeService都会创建WelcomeService实例
            //services.AddTransient<IWelcomeService, WelcomeService>();
            //// 一次Web请求创建一个WelcomeService实例，期间多次请求会继续使用同一个实例
            //services.AddScoped<IWelcomeService, WelcomeService>();

            // 单纯引入核心MVC服务，只有核心功能
            //services.AddMvcCore();
            // 一般用这个，功能多
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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

            // 支持静态文件
            app.UseStaticFiles();

            // 配置默认路由
            //app.UseMvcWithDefaultRoute();

            app.UseMvc(builder=>
            {
                // /home/Index/1
                //builder.MapRoute("Default", "{controller}/{action}/{id?}");
                builder.MapRoute("Default","{controller=home}/{action=index}/{id?}");

            });

            app.Run(async (context) =>
            {
                var welcome = welcomeService.GetMesssge();
                await context.Response.WriteAsync(welcome);
            });

            //app.Run(async (context) =>
            //{
            //    throw new Exception("自己抛出异常，测试开发者异常页面");
            //});

            // 设置环境变量名
            //env.EnvironmentName = "IntegrationTest";
            // 是否是IntegrationTest环境
            //env.IsEnvironment("IntegrationTest");
            // 常见环境变量：Development开发环境、 Staging演示（模拟、临时）环境、Production正式（生产）环境

            // 欢迎页面
            //app.UseWelcomePage();

            // 管道流程
            //app.Use(async (context, next) =>
            //{
            //    context.Response.ContentType = "text/plain;charset=utf-8";

            //    //await context.Response.WriteAsync("Hello!");

            //    logger.LogDebug("M1: 传入请求");
            //    await next();
            //    logger.LogDebug("M1: 传出响应");
            //});

            //app.Use(async (context, next) =>
            //{
            //    context.Response.ContentType = "text/plain;charset=utf-8";

            //    logger.LogDebug("M2: 传入请求");
            //    await next();
            //    logger.LogDebug("M2: 传出响应");
            //});

            //app.Run(async (context) =>
            //{
            //    //await context.Response.WriteAsync("你好！");
            //    await context.Response.WriteAsync("M3: 处理请求，生成响应");
            //    logger.LogDebug("M3: 处理请求，生成响应");
            //});

            //app.Run(async (context) =>
            //{
            //    //var welcome = configuration["welcome"];
            //    var welcome = welcomeService.GetMesssge();
            //    await context.Response.WriteAsync(welcome);
            //});

        }
    }
}
