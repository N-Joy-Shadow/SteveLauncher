using McLib.Model.Network.Dns;
using SteveLauncher.Domain.Entity;

namespace SteveLauncher.API.Repository;

public interface ILocalServerListRepository {
    public List<MinecraftHost> GetServerList();
    public Task<bool> AddServer(MinecraftURL server);
    public Task<bool> RemoveServer(MinecraftURL server);
    public Task<MinecraftHost> FindServer(string server);
}