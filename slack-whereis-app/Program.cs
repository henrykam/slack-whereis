using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HenryKam.SlackWhereIs
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args)
                .Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.ConfigureKestrel((c, o) => { o.AllowSynchronousIO = true; });
                    webBuilder.ConfigureAppConfiguration((builderContext, config) =>
                    {
                        IWebHostEnvironment env = builderContext.HostingEnvironment;

                        config
                            .SetBasePath(env.ContentRootPath)
                            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                            .AddJsonFile($"Secrets/appsettings.secrets.json", optional: true, reloadOnChange: true)
                            .AddEnvironmentVariables();
                    }).UseUrls("http://*:5000");
                })
            ;


        
    }
}
