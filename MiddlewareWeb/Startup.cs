using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MiddlewareWeb.Middleware;

namespace MiddlewareWeb
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<Use2Middleware>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //app.UseMiddleware(typeof(Use1Middleware));
            app.UseMiddleware<Use1Middleware>();

            //app.UseMiddleware<Use2Middleware>();
            app.UseUse2Middleware();

            app.Map("/map1", appBuilder =>
                appBuilder.Run(
                async context =>
                {
                    System.Console.WriteLine("Begin Run1");
                    await context.Response.WriteAsync("Hello from Run1!");
                    System.Console.WriteLine("End Run1");
                }
            ));

            app.Map("/map2", appBuilder =>
                Map2(appBuilder));

                
            app.MapWhen(context => context.Request.Query.TryGetValue("key", out var value) ? value == "run" : false,
            appBuilder => appBuilder.Run(async context => {
            
                    System.Console.WriteLine("Begin MapWhen Run");
                    await context.Response.WriteAsync("Hello from MapWhen Run");
                    System.Console.WriteLine("End MapWhen Run");
            }));

            app.MapWhen(context => context.Request.Query.TryGetValue("key", out var value) ? value == "use" : false,
            appBuilder => appBuilder.Use(async (context, next) => {

                    System.Console.WriteLine("Begin MapWhen Use");
                    if(context.Request.Query.TryGetValue("run", out var value) ? value == "true" : false) {
                        
                        System.Console.WriteLine("Begin MapWhen UseRun");
                        await next.Invoke();
                        System.Console.WriteLine("End MapWhen UseRun");
                    }
                    else
                        await context.Response.WriteAsync("Hello from MapWhen Use");
                    System.Console.WriteLine("End MapWhen Use");
            }));

            app.UseMiddleware<RunMiddleware>();
        }

        private static void Map2(IApplicationBuilder appBuilder)
        {
            appBuilder.Use(async (context, next) =>
            {
                System.Console.WriteLine("Begin Use3");
                await next.Invoke();
                System.Console.WriteLine("End Use3");
            });

            appBuilder.Run(
                async context =>
                {
                    System.Console.WriteLine("Begin Run2");
                    await context.Response.WriteAsync("Hello from Run2!");
                    System.Console.WriteLine("End Run2");
                });
        }
    }
}