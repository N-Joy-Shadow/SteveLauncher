using McLib.Model.Network.Dns;
using SteveLauncher.Domain.Entity;

namespace SteveLauncher.API.Repository;

public interface ILocalServerListRepository {
    public List<MinecraftHost> GetServerList();
    public Task<bool> AddServer(MinecraftHost host);
    public bool RemoveServer(MinecraftURL server);
    public MinecraftHost? FindServer(MinecraftURL server);
}