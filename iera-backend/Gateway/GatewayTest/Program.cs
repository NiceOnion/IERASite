using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace Gateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .UseKestrel()
                        .UseContentRoot(Directory.GetCurrentDirectory())
                        .ConfigureAppConfiguration((hostingContext, config) =>
                        {
                            config
                                .SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
                                .AddJsonFile("appsettings.json", true, true)
                                .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", true, true)
                                .AddJsonFile("ocelot.json")
                                .AddEnvironmentVariables();
                        })
                        .ConfigureServices((hostContext, services) =>
                        {
                            services.AddControllers();
                            services.AddEndpointsApiExplorer();
                            services.AddSwaggerGen();
                            services.AddOcelot();
                        })
                        .Configure(app =>
                        {
                            var env = app.ApplicationServices.GetService<IWebHostEnvironment>();
                            if (env.IsDevelopment())
                            {
                                app.UseSwagger();
                                app.UseSwaggerUI();
                            }

                            app.UseHttpsRedirection();
                            app.UseRouting();
                            app.UseAuthorization();
                            app.UseEndpoints(endpoints =>
                            {
                                endpoints.MapControllers();
                            });
                            app.UseOcelot().Wait();
                        });
                })
                .Build()
                .Run();
        }
    }
}
