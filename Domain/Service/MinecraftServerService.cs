using Maui.Plugins.PageResolver.Attributes;
using SteveLauncher.API.Repository;
using SteveLauncher.API.Service;
using SteveLauncher.Domain.Entity;

namespace SteveLauncher.Domain.Service;

public class MinecraftServerService: IMinecraftServerService {
    private readonly IMinecraftServerStatusRepository serverRepository;
    private readonly ILocalServerListRepository localServerListRepository;
    public MinecraftServerService(
        ILocalServerListRepository localServerListRepository,
        IMinecraftServerStatusRepository serverRepository) {
        this.serverRepository = serverRepository;
        this.localServerListRepository = localServerListRepository;
    }

    public async Task<List<MinecraftServerInfo>> GetServerStatusList() {
        var result = new List<MinecraftServerInfo>();
        var list = localServerListRepository.GetServerList();
        foreach (var host in list) {
            var info = await serverRepository.FetchServer(host.SRVHost);
            result.Add(new () {
                isOnline = info.ServerUpdatable.isOnline,
                Motd = info.ServerUpdatable.Motd,
                Icon = info.ServerUpdatable.Icon,
                HostName = host.Host,
                Version = info.ServerUpdatable.Version,
                PlayerInfo = new() {
                    Max = info.ServerUpdatable.MaxPlayer ?? 0,
                    Currnet = info.ServerUpdatable.CurrentPlayer ?? 0
                }
            });
        }

        return result;


    }

    public bool DeleteServer(MinecraftServerInfo serverInfo) {
        try {
            this.localServerListRepository.RemoveServer(serverInfo.HostName);
        }
        catch (Exception E) {
            return false;
        }

        return true;
    }
}