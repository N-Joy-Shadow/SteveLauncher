using McLib.Model.Network;
using SteveLauncher.Domain.Entity;

namespace SteveLauncher.API.Repository;

public interface ILocalServerListRepository {
    public List<MinecraftMultiHost> GetServerList();
    public Task<bool> AddServer(MinecraftHost host,MinecraftHost srvHost);
    public bool RemoveServer(MinecraftHost server);
    public MinecraftMultiHost? FindServer(MinecraftHost server);
}