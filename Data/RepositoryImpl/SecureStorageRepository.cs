using SteveLauncher.API.Enum;
using SteveLauncher.API.Exception;
using SteveLauncher.API.Repository;

namespace SteveLauncher.Data.RepositoryImpl;

public class SecureStorageRepository : ISecureStorageRepository {


    public async Task<T> GetAsync<T>(string key) {
        var value = await GetRawValueAsync(key);
        try {
            return JsonSerializer.Deserialize<T>(value);
        }
        catch (Exception e) {
            throw SecureStorageException.InvalidData(key);
        }
    }

    public Task<T> GetAsync<T>(SecureStorageEnum key) {
        return GetAsync<T>(key.ToString());
    }

    public Task<string> GetRawValueAsync(string key) {
        try {

            var result = SecureStorage.GetAsync(key);
            if (result is null)
                throw SecureStorageException.KeyNotFound(key);
            return result;
        }
        catch (Exception e) {
            Debug.WriteLine("SecureStorageRepository.GetRawValueAsync: " + e);
        }

        return null;
    }

    public async Task<string> GetRawValueAsync(SecureStorageEnum key) {
        return await GetRawValueAsync(key.ToString());
    }

    public async Task InsertAsync<T>(string key, T item) {
         SecureStorage.SetAsync(key, JsonSerializer.Serialize(item));
    }

    public async Task InsertAsync<T>(SecureStorageEnum key, T item) {
        InsertAsync<T>(key.ToString(), item);
    }

    public bool Remove(string key) {
        return SecureStorage.Remove(key);
    }

    public bool Remove(SecureStorageEnum key) {
        return Remove(key.ToString());
    }
}