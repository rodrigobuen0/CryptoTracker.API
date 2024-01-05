using StackExchange.Redis;

namespace CryptoTracker.API.Services
{
    public interface ICacheService
    {
        Task Add(string key, string value);
        Task<string?> Get(string key);
        Task Remove(string key);
    }
    public sealed class CacheService : ICacheService
    {
        private readonly ConnectionMultiplexer _connection;
        private readonly IDatabase _cache;
        private readonly IServer _server;
        public CacheService()
        {
            _connection = ConnectionMultiplexer.Connect(new ConfigurationOptions()
            {
                EndPoints = { "localhost:6379" },
                ClientName = "CryptoTracker.Api",
            });

            _cache = _connection.GetDatabase();
            _server = _connection.GetServer("localhost:6379");
        }

        public async Task Add(string key, string value) => await _cache.StringSetAsync(string.Concat("CManager:", key), value);
        public async Task<string?> Get(string key) => await _cache.StringGetAsync(key);
        public async Task Remove(string key) => await _cache.StringGetDeleteAsync(key);
    }
}
