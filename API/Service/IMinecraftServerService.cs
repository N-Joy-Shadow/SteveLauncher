using SteveLauncher.Domain.Entity;

namespace SteveLauncher.API.Service;

public interface IMinecraftServerService {
    Task<List<MinecraftServerInfo>> GetServerStatusList();
    bool DeleteServer(MinecraftServerInfo serverInfo);
}