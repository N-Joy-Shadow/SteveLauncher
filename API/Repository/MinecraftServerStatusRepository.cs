using McLib.Model.Network.Dns;
using McLib.Model.Network.Mc;

namespace SteveLauncher.API.Repository;

public interface IMinecraftServerStatusRepository {
     Task<McServerInfo> fetchServer(MinecraftURL hostname);
}