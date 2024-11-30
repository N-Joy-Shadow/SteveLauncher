using CmlLib.Core.Version;
using McLib.Model.Network.Dns;
using SteveLauncher.Domain.Entity;

namespace SteveLauncher.API.Service;

public interface IMinecraftGameService {
    Task StartGame(MinecraftURL url);
    Task<List<string>> GetAvailableVersions(string version);
    void SetSettings(MinecraftGameSetting setting);
    void SetGameVersion(string version);
    MinecraftGameSetting GetSetting();
}