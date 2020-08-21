using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using NorthwindApiDemo.EFModels;
using NorthwindApiDemo.Models;
using Microsoft.EntityFrameworkCore;
using NorthwindApiDemo.Services;

namespace NorthwindApiDemo
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(option => option.EnableEndpointRouting = false).AddNewtonsoftJson();
            services.AddDbContext<NorthwindContext>(options=>
            {
                options.UseSqlServer("Server=.\\SQLEXPRESS;Database=Northwind;Trusted_Connection=True;");
            });

            services.AddScoped<ICustomerRepository, CustomerRepository>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            else
            {
                app.UseExceptionHandler("/statusCodePath");
            }

            app.UseStatusCodePages();

            AutoMapper.Mapper.Initialize(config =>
            {
                config.CreateMap<Customers, CustomerWithoutOrders>();
                config.CreateMap<Customers, CustomerDTO>();
                config.CreateMap<Orders, OrdersDTO>();
            });
            app.UseMvc();



            //app.UseMvc(config => 
            //{
            //    config.MapRoute(name: "Default",
            //                    template: "{controller}/{action}/{id}",
            //                    defaults: new
            //                    {
            //                        controller = "Home",
            //                        action = "Index"
            //                    });
            //});

            //app.UseMvcWithDefaultRoute();

            //app.UseRouting();

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapGet("/", async context =>
            //    {
            //        throw new Exception("Testing exception");
            //    });
            //});


            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapGet("/", async context =>
            //    {
            //        await context.Response.WriteAsync("Hello World!");
            //    });
            //});
        }
    }
}
