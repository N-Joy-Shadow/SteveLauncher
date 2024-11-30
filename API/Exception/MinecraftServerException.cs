namespace SteveLauncher.API.Exception;

public class MinecraftServerException : System.Exception {
    public MinecraftServerException(string message) : base(message) {
    }
}

public class MinecraftServerAlreadyExistException : MinecraftServerException {
    public MinecraftServerAlreadyExistException(string message) : base(message) {
    }
}

public class MinecraftServerNotFoundException : MinecraftServerException {
    public MinecraftServerNotFoundException(string message) : base(message) {
    }
}