using SteveLauncher.API.Enum;
using SteveLauncher.API.Exception;
using SteveLauncher.API.Repository;

namespace SteveLauncher.Data.RepositoryImpl;

public class StorageRepository : IStorageRepository {
    public T Get<T>(string key) {
        var value = GetRawValue(key);
        try {
            return JsonSerializer.Deserialize<T>(value);
        }
        catch (Exception e) {
            throw new StorageInvalidDataException(key, typeof(T));
        }
    }

    public T Get<T>(StorageEnum key) {
        return Get<T>(key.ToString());
    }

    public string GetRawValue(string key) {
        string result = Preferences.Default.Get(key, "");
        if (string.IsNullOrEmpty(result))
            throw new StorageKeyNotFoundException(key);
        return result;
    }

    public string GetRawValue(StorageEnum key) {
        return GetRawValue(key.ToString());
    }

    public void Insert<T>(string key, T item) {
        Preferences.Default.Set(key, JsonSerializer.Serialize(item));
    }

    public void Insert<T>(StorageEnum key, T item) {
        Insert<T>(key.ToString(), item);
    }

    public bool Remove(string key) {
        return SecureStorage.Remove(key);
    }

    public bool Remove(StorageEnum key) {
        return Remove(key.ToString());
    }
}