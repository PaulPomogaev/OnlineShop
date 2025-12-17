using Enyim.Caching;
using Microsoft.Extensions.Logging;
using OnlineShop.Core.Interfaces;
using System.Text.Json;

namespace OnlineShop.Db.Services
{
    public class MemcachedCacheService : ICacheService
    {
        private readonly IMemcachedClient _memcachedClient;
        private readonly JsonSerializerOptions _jsonOptions;
        private readonly ILogger<MemcachedCacheService> _logger;

        public MemcachedCacheService(
            IMemcachedClient memcachedClient,
            ILogger<MemcachedCacheService> logger)
        {
            _memcachedClient = memcachedClient;
            _logger = logger;

            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<T> GetAsync<T>(string key)
        {
            try
            {
                var result = await _memcachedClient.GetAsync<string>(key);
                if (result.Success && !string.IsNullOrEmpty(result.Value))
                {
                    return JsonSerializer.Deserialize<T>(result.Value, _jsonOptions);
                }
                return default;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Ошибка получения значения из Memcached для ключа: {Key}", key);
                return default;
            }
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan expiration)
        {
            try
            {
                var serializedValue = JsonSerializer.Serialize(value, _jsonOptions);
                await _memcachedClient.SetAsync(key, serializedValue, (int)expiration.TotalSeconds);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Ошибка в установке значения в Memcached для ключа: {Key}", key);
            }
        }

        public async Task RemoveAsync(string key)
        {
            try
            {
                await _memcachedClient.RemoveAsync(key);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Ошибка в удалении значения из Memcached для ключа: {Key}", key);
            }
        }

        public async Task<bool> ExistsAsync(string key)
        {
            try
            {
                var result = await _memcachedClient.GetAsync<string>(key);
                return result.Success;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Ошибка в проверке наличия в Memcached для ключа: {Key}", key);
                return false;
            }
        }
    }
}
