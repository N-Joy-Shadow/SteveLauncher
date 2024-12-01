using McLib.Model.Network.Dns;
using SteveLauncher.Domain.Entity;

namespace SteveLauncher.API.Service;

public interface IMinecraftServerService {
    Task<ICollection<MinecraftServerInfo>> GetServerStatusListAsync();
    bool DeleteServer(MinecraftServerInfo serverInfo);
    Task<bool> RegisterServer(MinecraftURL hostname);
    Task<MinecraftServerInfo?> FetchServerInfo(MinecraftURL hostname);
    
    Task<MinecraftServerInfo?> FetchTempServerInfo(MinecraftURL hostname);
}