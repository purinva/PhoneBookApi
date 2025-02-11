using StackExchange.Redis;

namespace PhoneBookApi.Services
{
    public class RedisService
    {
        private readonly IDatabase _database;
        private readonly IConnectionMultiplexer _redis;

        public RedisService(IConnectionMultiplexer redis)
        {
            _redis = redis;
            _database = redis.GetDatabase();
        }

        public async Task<string?> GetAsync(string key)
        {
            return await _database.StringGetAsync(key);
        }

        public async Task SetAsync(string key, string value, TimeSpan expiration)
        {
            await _database.StringSetAsync(key, value, expiration);
        }

        public async Task DeleteAsync(string key)
        {
            await _database.KeyDeleteAsync(key);
        }

        public async Task ClearByPatternAsync(string pattern)
        {
            var server = _redis.GetServer(_redis.GetEndPoints()[0]);
            foreach (var key in server.Keys(pattern: pattern + "*"))
            {
                await _database.KeyDeleteAsync(key);
            }
        }
    }
}