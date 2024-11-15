using McLib.Model.Network.Dns;
using SteveLauncher.Domain.Entity;

namespace SteveLauncher.API.Service;

public interface IMinecraftServerService {
    List<MinecraftServerInfo> GetServerStatusList();
    bool DeleteServer(MinecraftServerInfo serverInfo);
    Task<bool> RegisterServer(MinecraftURL hostname);
}