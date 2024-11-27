using SteveLauncher.API.Enum;

namespace SteveLauncher.API.Repository;

public interface IStorageRepository {
    [Obsolete("This method is not recommended. Consider using 'Get<T>(SecureStorageEnum)'", false)]
    T Get<T>(string key);
    T Get<T>(StorageEnum key);
    string GetRawValue(string key);
    string GetRawValue(StorageEnum key);
    
    void Insert<T>(string key, T item);
    void Insert<T>(StorageEnum key, T item);
    bool Remove(string key);
    bool Remove(StorageEnum key);
}