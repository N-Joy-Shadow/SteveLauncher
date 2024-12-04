using CmlLib.Core.Version;
using McLib.Model.Network;
using SteveLauncher.Domain.Entity;

namespace SteveLauncher.API.Service;

public interface IMinecraftGameService {
    Task StartGame(MinecraftHost host);
    Task<List<string>> GetAvailableVersions(string version);
    Task GetVersions();
    void SetSettings(MinecraftGameSetting setting);
    void SetGameVersion(string version);
    MinecraftGameSetting GetSetting();
}