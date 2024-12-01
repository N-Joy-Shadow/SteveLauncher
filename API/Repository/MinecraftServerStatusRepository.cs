using McLib.Model.Network.Dns;
using McLib.Model.Network.Mc;
using SteveLauncher.Domain.Entity;

namespace SteveLauncher.API.Repository;

public interface IMinecraftServerStatusRepository {
     Task<McServerInfo> FetchServerAsync(MinecraftURL hostname);
}