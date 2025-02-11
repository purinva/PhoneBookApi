using PhoneBookApi.Services;
using StackExchange.Redis;

namespace PhoneBookApi.Extensions
{
    public static class RedisConnection
    {
        public static void AddRedisConnection(this WebApplicationBuilder builder)
        {
            // Получаем строку подключения Redis
            var redisConnection = builder.Configuration.GetSection("Redis")["ConnectionString"];

            // Проверяем на null или пустое значение
            if (string.IsNullOrEmpty(redisConnection))
            {
                throw new ArgumentNullException("Redis connection string is missing in appsettings.json.");
            }

            // Регистрируем подключение
            builder.Services.AddSingleton<IConnectionMultiplexer>(
                ConnectionMultiplexer.Connect(redisConnection));
            builder.Services.AddSingleton<RedisService>();
        }
    }
}