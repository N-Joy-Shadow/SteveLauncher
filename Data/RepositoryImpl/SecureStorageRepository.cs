using SteveLauncher.API.Enum;
using SteveLauncher.API.Exception;
using SteveLauncher.API.Repository;

namespace SteveLauncher.Data.RepositoryImpl;

public class SecureStorageRepository : ISecureStorageRepository {
    public async Task<T> GetAsync<T>(string key) {
        var value = await GetRawValueAsync(key);
        try {
            return JsonSerializer.Deserialize<T>(value);
        }catch(Exception e) {
            throw SecureStorageException.InvalidData(key);
        }
    }

    public async Task<T> GetAsync<T>(SecureStorageEnum key) {
        return await GetAsync<T>(key.ToString());
    }

    public async Task<string> GetRawValueAsync(string key) {
        var result = await SecureStorage.Default.GetAsync(key);
        if(result is null)
            throw SecureStorageException.KeyNotFound(key);
        return result;
    }

    public async Task<string> GetRawValueAsync(SecureStorageEnum key) {
        return await GetRawValueAsync(key.ToString());
    }
    public async void InsertAsync<T>(string key, T item) {
        await SecureStorage.Default.SetAsync(key,JsonSerializer.Serialize(item));
    }

    public async void InsertAsync<T>(SecureStorageEnum key, T item) {
        InsertAsync<T>(key.ToString(),item);
    }

    public bool Remove(string key) {
        return SecureStorage.Default.Remove(key);
    }

    public bool Remove(SecureStorageEnum key) {
        return Remove(key.ToString());
    }
}