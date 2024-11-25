namespace SteveLauncher.API.Exception;

public class SecureStorageException : System.Exception {
    public SecureStorageException(string message) : base(message) {
        
    }
    
    public static SecureStorageException KeyNotFound(string keyName)
    {
        return new SecureStorageException($"The key '{keyName}' was not found in secure storage.");
    }
    
    public static SecureStorageException InvalidData(string keyName)
    {
        return new SecureStorageException($"The data associated with the key '{keyName}' is invalid and cannot be converted to the desired type.");
    }

}