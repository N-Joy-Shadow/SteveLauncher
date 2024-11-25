namespace SteveLauncher.API.Service;

public interface IMinecraftGameService {
    void StartGame();
    void SetSettings(object setting);
    void SetGamePath(string Path);
    void SetGameVersion(string version);
}