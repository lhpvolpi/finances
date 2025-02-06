namespace Finances.Core.Contexts.SharedContext.Data;

public interface ICacheRepository
{
    Task SetAsync<T>(string key, T value, int expiresInSeconds = 600);

    Task<T> GetFirstAsync<T>(string key);

    Task<bool> HasKeyAsync(string key);

    Task RemoveAsync(string key);

    Task PushAsync<T>(string key, T value);

    Task<T> PopAsync<T>(string key);
}
