using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace StudyManagement
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // 单例模式，整个Web生命周期，只有一个实例
            services.AddSingleton<IWelcomeService, WelcomeService>();
            //// 每次请求IWelcomeService都会创建WelcomeService实例
            //services.AddTransient<IWelcomeService, WelcomeService>();
            //// 一次Web请求创建一个WelcomeService实例，期间多次请求会继续使用同一个实例
            //services.AddScoped<IWelcomeService, WelcomeService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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
    }
}
