using SteveLauncher.API.Service;

namespace SteveLauncher.Domain.Service;

public class DirectoryLaunchService : IDirectoryLaunchService {
    public void Open(string directoryPath) {
        if (string.IsNullOrEmpty(directoryPath))
            throw new ArgumentException("Folder path cannot be null or empty.", nameof(directoryPath));

        if (!Directory.Exists(directoryPath))
            throw new DirectoryNotFoundException($"Directory '{directoryPath}' path does not exist.");
#if WINDOWS
        Process.Start("explorer.exe", directoryPath);
#elif MACCATALYST
        Process.Start("open", directoryPath);    
#endif
    }
}