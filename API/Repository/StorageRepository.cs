using SteveLauncher.API.Enum;

namespace SteveLauncher.API.Repository;

public interface IStorageRepository {
    [Obsolete("This method is not recommended. Consider using 'Get<T>(StorageEnum)'", false)]
    T Get<T>(string key);
    /// <summary>
    /// Storage에서 Key에 해당하는 값을 가져오는 함수
    /// </summary>
    /// <param name="key">찾을 Storage Key</param>
    /// <typeparam name="T">return으로 받을 Object Type</typeparam>
    /// <returns>Deserailized Json Storage Value</returns>
    /// <exception cref="StorageKeyNotFoundException">Key에 해당하는 값을 찾을 수 없을 경우 일어나는 에러</exception>
    /// <exception cref="StorageInvalidDataException">Type T로 Deserialize가 실패했을 경우 일어나는 에러</exception>
    /// <seealso cref="ISecureStorage.GetAsync"/>
    /// <seealso cref="GetRawValue(SecureStorageEnum)"/>
    T Get<T>(StorageEnum key);
    string GetRawValue(string key);
    /// <summary>
    /// key에 해당하는 json string 값을 가져오는 함수
    /// </summary>
    /// <param name="key">찾을 Storage Key</param>
    /// <returns>json string value</returns>
    /// <seealso cref="ISecureStorage.GetAsync"/>
    string GetRawValue(StorageEnum key);
    void Insert<T>(string key, T item);
    /// <summary>
    /// Storage에 값을 삽입하거나 업데이트하는 함수
    /// </summary>
    /// <param name="key">삽입할 Storage Key</param>
    /// <param name="item">저장할 데이터</param>
    /// <typeparam name="T">저장할 데이터의 타입</typeparam>
    /// <seealso cref="ISecureStorage.SetAsync"/>
    void Insert<T>(StorageEnum key, T item);
    bool Remove(string key);
    /// <summary>
    /// Storage key값에 해당하는 값을 삭제하는 함수
    /// </summary>
    /// <param name="key">찾을 Storage Key</param>
    /// <see cref="ISecureStorage.Remove"/>
    bool Remove(StorageEnum key);
}