using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HenryKam.SlackWhereIs.Application;
using HenryKam.SlackWhereIs.Infrastructure;
using HenryKam.SlackWhereIs.Infrastructure.Exchange;
using HenryKam.SlackWhereIs.Infrastructure.Slack;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace HenryKam.SlackWhereIs
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {            
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddConsole();
                //loggingBuilder.AddSerilog();
                loggingBuilder.AddDebug();
            });

            services.AddControllers().AddNewtonsoftJson();            
            services.AddDbContext<LocationDbContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("slackwhereis")).UseSnakeCaseNamingConvention());
            services.AddScoped<ILocationRepository, PostgresLocationRepository>();
            services.AddTransient<BotProcessingService>();
            services.AddHostedService<BotProcessingService>();
            services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();
            services.AddHttpClient();


            // Slack Configuration
            SlackConfig slackConfig = new SlackConfig();
            Configuration.GetSection("Slack").Bind(slackConfig);
            services.AddScoped<SlackAuthorizationActionFilter>();
            services.AddSingleton(slackConfig);

            // Exchange Configuration
            ExchangeConfig exchangeConfig = new ExchangeConfig();
            Configuration.GetSection("Exchange").Bind(exchangeConfig);
            services.AddScoped<ExchangeProvider>();
            services.AddSingleton(exchangeConfig);

            services.Configure<IISServerOptions>(options => { options.AllowSynchronousIO = true; });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            //var builder = new ConfigurationBuilder()
            //    .SetBasePath(env.ContentRootPath)
            //    .AddJsonFile($"appsettings.json", optional: true, reloadOnChange: true)
            //    //.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
            //    .AddJsonFile($"Secrets/appsettings.secrets.json", optional: true, reloadOnChange: true)
            //    .AddEnvironmentVariables();

            //Configuration = builder.Build();

            //loggerFactory.LoggerFactory.Create(builder => builder.AddConsole()

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            //app.Use((context, next) =>
            //{
            //    context.Request.EnableBuffering();
            //    return next();
            //});

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
