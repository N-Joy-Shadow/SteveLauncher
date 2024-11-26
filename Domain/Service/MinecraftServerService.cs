using CommunityToolkit.Maui.Core.Extensions;
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

    public ICollection<MinecraftServerInfo> GetServerStatusList() {
        var result = new List<MinecraftServerInfo>();
        var list = localServerListRepository.GetServerList();
        foreach (var host in list) {
            var info = serverRepository.FetchServer(host);
            Debug.WriteLine($"{info.HostName}: {string.Join(", ", info.ServerUpdatable.Player.Select(x => x.Name))}");
            result.Add(new () {
                isOnline = info.ServerUpdatable.isOnline,
                Motd = info.ServerUpdatable.Motd,
                Icon = info.ServerUpdatable.Icon,
                HostName = host.Host,
                Version = info.ServerUpdatable.Version,
                PlayerInfo = new() {
                    Max = info.ServerUpdatable.MaxPlayer ?? 0,
                    Currnet = info.ServerUpdatable.CurrentPlayer ?? 0,
                    UserNames = info.ServerUpdatable.Player.Select(x => x.Name).ToHashSet().ToObservableCollection()
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

    //TODO: 나중에 전부 예외처리로 바꾸기~
    public async Task<bool> RegisterServer(MinecraftURL hostname) {
        var srv = await this.dnsService.executeAsync(hostname);
        var result = serverRepository.FetchServer(new MinecraftHost(srv,hostname));
        if(result.ServerUpdatable.isOnline || result.ServerUpdatable.Motd is not null)
            return await this.localServerListRepository.AddServer(new (hostname,srv));
        return false;
    }
}