using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // Добавление сервисов для работы Сессии
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.Cookie.Name = "MySession"; // Меняем имя Сессиии
                options.Cookie.Path = "/index"; // Задаем путь, по которому будет доступна сессия
                options.Cookie.IsEssential = true; 
                options.IdleTimeout = TimeSpan.FromSeconds(10); // Время (в секундах), в течение которого истечект сессия
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            #region HttpContext.Items
            //app.Use(async (context, next) =>
            //{
            //    // HttpContext.Items
            //    context.Items["itemName"] = "ITEM";
            //    context.Items.Add("itemAge", 18);
            //    await next.Invoke();
            //});

            //app.Use(async (context, next) =>
            //{
            //    //context.Items.Clear();              
            //    if (context.Items["itemName"] != null)
            //    {
            //        await context.Response.WriteAsync($"Hello {context.Items["itemName"]}, Age - {context.Items["itemAge"]}, Items = {context.Items.Count} \n");
            //    }
            //    else
            //        await context.Response.WriteAsync("null \n");
            //});
            #endregion

            #region Cook
            //app.Map("/delete", deleteApp =>
            //{
            //    deleteApp.Run(async context =>
            //    {
            //        context.Response.Cookies.Delete("cookName"); // по пути host/index удаляем кук cookName и выводим сообщение
            //        await context.Response.WriteAsync("delete cookName");
            //    });
            //});

            //app.Use(async (context, next) =>
            //{
            //    if (context.Request.Cookies.ContainsKey("cookName"))
            //    {
            //        await context.Response.WriteAsync(context.Request.Cookies["cookName"]);
            //    }
            //    else
            //    {
            //        context.Response.Cookies.Append("cookName", "COOK");
            //        await context.Response.WriteAsync("add cookName");
            //    }
            //});
            #endregion

            #region Session
            app.UseSession(); // механизм работ с сессиями встраивается в конвейер обработки запроса
            app.Run(async context =>
            {
                if (context.Session.Keys.Contains("session"))
                {
                    await context.Response.WriteAsync($"Hello {context.Session.GetString("session")}");
                }
                else
                {
                    context.Session.SetString("session", "UFA");
                    await context.Response.WriteAsync("Hello Russia");
                }
            });
            #endregion
        }
    }
}