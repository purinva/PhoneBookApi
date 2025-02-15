using Microsoft.EntityFrameworkCore;
using PhoneBookApi.Data;

namespace PhoneBookApi.Extensions
{
    public static class DbConnection
    {
        public static void AddDbConnection(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(
                options => options.UseNpgsql(configuration
                .GetConnectionString("DefaultConnection")));
        }
    }
}