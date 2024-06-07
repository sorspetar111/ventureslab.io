using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace UserTaskApi.Data
{
    public static class DatabaseProviderFactory
    {
        public static void ConfigureDatabase(IServiceCollection services, IConfiguration configuration)
        {
            var useInMemory = configuration.GetValue<bool>("UseInMemoryDatabase");

            if (useInMemory)
            {
                services.AddDbContext<UserTaskContext>(options =>
                    options.UseInMemoryDatabase("UserTasksDB"));
            }
            else
            {
                var connectionString = configuration.GetConnectionString("DefaultConnection");
                services.AddDbContext<UserTaskContext>(options =>
                    options.UseSqlServer(connectionString));
            }
        }
    }
}
