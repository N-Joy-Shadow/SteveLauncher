using Maui.Plugins.PageResolver.Attributes;
using McLib.API.Services;
using McLib.Model.Network.Dns;
using SteveLauncher.API.Repository;
using SteveLauncher.API.Service;
using SteveLauncher.Domain.Entity;

namespace SteveLauncher.Domain.Service;

public class MinecraftServerService: IMinecraftServerService {
    private readonly IMinecraftServerStatusRepository serverRepository;
    private readonly ILocalServerListRepository localServerListRepository;
    private readonly IDnsCheckService dnsService;
    public MinecraftServerService(
        ILocalServerListRepository localServerListRepository,
        IMinecraftServerStatusRepository serverRepository,
        IDnsCheckService dnsService) {
        this.serverRepository = serverRepository;
        this.localServerListRepository = localServerListRepository;
        this.dnsService = dnsService;
    }

    public List<MinecraftServerInfo> GetServerStatusList() {
        var result = new List<MinecraftServerInfo>();
        var list = localServerListRepository.GetServerList();
        foreach (var host in list) {
            var info = serverRepository.FetchServer(host);
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

    public async Task<bool> RegisterServer(MinecraftURL hostname) {
        var srv = await this.dnsService.executeAsync(hostname);
        return await this.localServerListRepository.AddServer(new (hostname,srv));
    }
}