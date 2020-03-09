using HenryKam.SlackWhereIs.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HenryKam.SlackWhereIs
{
    public static class IWebHostExtensions
    {

        public static IHost Seed(this IHost webhost)
        {
            using (var scope = webhost.Services.GetService<IServiceScopeFactory>().CreateScope())
            {
                // alternatively resolve UserManager instead and pass that if only think you want to seed are the users     
                using (var dbContext = scope.ServiceProvider.GetRequiredService<LocationDbContext>())
                {
                    SeedData.SeedAsync(dbContext).GetAwaiter().GetResult();
                    return webhost;
                }
            }
        }

        public static class SeedData
        {
            public static async Task SeedAsync(LocationDbContext dbContext)
            {
                //dbContext.Users.Add(new User { Id = 1, Username = "admin", PasswordHash = ... });
            }
        }
    }
}
