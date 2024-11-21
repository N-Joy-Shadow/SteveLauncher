using CmlLib.Core.Auth;

namespace SteveLauncher.Utils.Files;

public class SLFileManager {
    public void CreateFile(string path, string content) {
        File.WriteAllText(path, content);
    }
}