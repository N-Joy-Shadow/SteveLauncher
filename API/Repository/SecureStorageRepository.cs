using SteveLauncher.API.Enum;

namespace SteveLauncher.API.Repository;

public interface ISecureStorageRepository {
    [Obsolete("This method is not recommended. Consider using 'Get<T>(SecureStorageEnum)'", false)]
    Task<T> GetAsync<T>(string key);
    Task<T> GetAsync<T>(SecureStorageEnum key);
    Task<string> GetRawValueAsync(string key);
    Task<string> GetRawValueAsync(SecureStorageEnum key);
    /// <summary>
    /// 기본적으로 덮어씌우기임
    /// </summary>
    /// <param name="key"></param>
    /// <param name="item"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    Task InsertAsync<T>(string key, T item);
    Task InsertAsync<T>(SecureStorageEnum key, T item);
    bool Remove(string key);
    bool Remove(SecureStorageEnum key);
}