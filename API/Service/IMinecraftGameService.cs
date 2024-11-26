using CmlLib.Core.Version;
using SteveLauncher.Domain.Entity;

namespace SteveLauncher.API.Service;

public interface IMinecraftGameService {
    Task StartGame(string version);
    void SetSettings(MinecraftGameSetting setting);
    void SetGamePath(string Path);
    void SetGameVersion(string version);
    Task<MinecraftGameSetting?> GetSetting();
}