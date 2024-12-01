using McLib.Model.Network;
using SteveLauncher.Domain.Entity;

namespace SteveLauncher.API.Service;

public interface IMinecraftServerService {
    Task<ICollection<MinecraftServerInfo>> GetServerStatusListAsync();
    bool DeleteServer(MinecraftServerInfo serverInfo);
    Task<bool> RegisterServer(MinecraftHost hostname);
    Task<MinecraftServerInfo?> FetchServerInfo(MinecraftHost hostname);
    
    Task<MinecraftServerInfo?> FetchTempServerInfo(MinecraftHost hostname);
}