using CmlLib.Core.Version;
using McLib.Model.Network.Dns;
using SteveLauncher.Domain.Entity;

namespace SteveLauncher.API.Service;

public interface IMinecraftGameService {
    Task StartGame(MinecraftURL url);
    void SetSettings(MinecraftGameSetting setting);
    void SetGamePath(string Path);
    void SetGameVersion(string version);
    Task<MinecraftGameSetting?> GetSetting();
}