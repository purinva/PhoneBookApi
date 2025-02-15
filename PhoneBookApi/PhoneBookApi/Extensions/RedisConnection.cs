using PhoneBookApi.Services;
using StackExchange.Redis;

namespace PhoneBookApi.Extensions
{
    public static class RedisConnection
    {
        public static void AddRedisConnection(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Получение строку подключения Redis
            var redisConnection = configuration
            .GetSection("Redis")["ConnectionString"];

            // Проверка на null или пустое значение
            if (string.IsNullOrEmpty(redisConnection))
            {
                throw new ArgumentNullException(
                    "Redis connection string is missing in appsettings.json.");
            }

            // Регистрация подключения
            services.AddSingleton<IConnectionMultiplexer>(
                ConnectionMultiplexer.Connect(redisConnection));
            services.AddSingleton<RedisService>();
        }
    }
}