using Finances.Core.Contexts.SharedContext.Data;
using Finances.Core.Contexts.SharedContext.Entities;

namespace Finances.Infra.Data;

public class CacheRepository : ICacheRepository, IDisposable
{
    private readonly IConnectionMultiplexer _client;

    public CacheRepository(IOptions<Settings> settings)
    {
        var connection = settings.Value?.ConnectionStrings?.RedisConnection;
        this._client = ConnectionMultiplexer.Connect(connection);
    }

    public async Task<T> GetFirstAsync<T>(string key)
    {
        var redisValue = await this._client.GetDatabase().StringGetAsync(key);

        if (redisValue.HasValue)
            return JsonSerializer.Deserialize<T>(redisValue);

        return default;
    }

    public async Task SetAsync<T>(string key, T value, int expiresInSeconds = 600)
    {
        var expiresInTimeSpan = DateTime.UtcNow.AddSeconds(expiresInSeconds).Subtract(DateTime.UtcNow);
        await this._client.GetDatabase().StringSetAsync(key, JsonSerializer.Serialize(value), expiresInTimeSpan);
    }

    public async Task<bool> HasKeyAsync(string key)
        => await this._client.GetDatabase().KeyExistsAsync(key);

    public async Task RemoveAsync(string key)
        => await this._client.GetDatabase().KeyDeleteAsync(key);

    public async Task PushAsync<T>(string key, T value)
        => await this._client.GetDatabase().ListLeftPushAsync(key, JsonSerializer.Serialize(value));

    public async Task<T> PopAsync<T>(string key)
    {
        var redisValue = await this._client.GetDatabase().ListLeftPopAsync(key);

        if (redisValue.HasValue)
            return JsonSerializer.Deserialize<T>(redisValue);

        return default;
    }

    public void Dispose()
    {
        this._client.Dispose();
        GC.SuppressFinalize(this);
    }
}
