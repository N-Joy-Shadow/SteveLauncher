namespace SteveLauncher.API.Exception;

public class MinecraftLauncherException : System.Exception {
    public MinecraftLauncherException(string message) : base(message) { }
}

public class MinecraftLauncherNotAuthorizedException : MinecraftLauncherException {
    public MinecraftLauncherNotAuthorizedException()
        : base("The user is not authorized to perform this action.") { }
}