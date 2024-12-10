using SteveLauncher.API.Enum;

namespace SteveLauncher.API.Exception;

/// <summary>
/// MAUI에서 제공하는 Storage관련 예외처리 클래스
/// </summary>
public class StorageException : System.Exception {
    public StorageException(string message) : base(message) { }
}

public class StorageKeyNotFoundException : StorageException {
    public StorageKeyNotFoundException(string key)
        : base($"The key '{key}' was not found in storage.") {
        this.Data.Add("key", key);
    }
}

public class StorageInvalidDataException : StorageException {
    public StorageInvalidDataException(string key, Type type)
        : base(
            $"The data associated with the key '{key}' is invalid and cannot be converted to '{nameof(type)}' type.") {
        this.Data.Add("key", key);
    }
}