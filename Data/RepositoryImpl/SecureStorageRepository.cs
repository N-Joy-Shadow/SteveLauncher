using SteveLauncher.API.Enum;
using SteveLauncher.API.Exception;
using SteveLauncher.API.Repository;

namespace SteveLauncher.Data.RepositoryImpl;

public class SecureStorageRepository : ISecureStorageRepository {
    public async Task<T> Get<T>(string key) {
        var value = await GetRawValue(key);
        try {
            return JsonSerializer.Deserialize<T>(value);
        }
        catch (Exception e) {
            if(e is JsonException)
                throw new StorageInvalidDataException(key, typeof(T));

            throw new UnreachableException();
        }
    }

    public Task<T> Get<T>(SecureStorageEnum key) {
        return Get<T>(key.ToString());
    }

    public async Task<string> GetRawValue(string key) {
        var value = await SecureStorage.Default.GetAsync(key);
        if (value is null)
            throw new StorageKeyNotFoundException(key);

        return value;
    }

    public Task<string> GetRawValue(SecureStorageEnum key) {
        return GetRawValue(key.ToString());
    }

    public Task Insert<T>(string key, T item) {
        return SecureStorage.Default.SetAsync(key, JsonSerializer.Serialize(item));
    }

    public Task Insert<T>(SecureStorageEnum key, T item) {
        return Insert(key.ToString(), item);
    }

    public bool Remove(string key) {
        return SecureStorage.Default.Remove(key);
    }

    public bool Remove(SecureStorageEnum key) {
        return Remove(key.ToString());
    }
}