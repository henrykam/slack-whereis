using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HenryKam.SlackWhereIs.Application;
using HenryKam.SlackWhereIs.Infrastructure;
using HenryKam.SlackWhereIs.Infrastructure.EFCore;
using HenryKam.SlackWhereIs.Infrastructure.Exchange;
using HenryKam.SlackWhereIs.Infrastructure.Slack;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
            services.AddDbContext<SlackWhereIsDbContext>(options => {
                options.UseNpgsql(Configuration.GetConnectionString("slackwhereis")).UseSnakeCaseNamingConvention();
                //options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                });
                
            services.AddScoped<ILocationRepository, EntityFrameworkLocationRepository>();
            services.AddTransient<BotProcessingService>();
            services.AddHostedService<BotProcessingService>();
            services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();
            services.AddHttpClient();

            // 1. Add Authentication Services
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = "https://absoluteps.auth0.com/";
                options.Audience = "WhereIs Slack Bot API on AWS";
            });

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

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            // 2. Enable authentication middleware
            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
