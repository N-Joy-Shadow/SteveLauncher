using CommunityToolkit.Maui.Core.Extensions;
using Maui.Plugins.PageResolver.Attributes;
using McLib.API.Services;
using McLib.Model.Network.Dns;
using SteveLauncher.API.Exception;
using SteveLauncher.API.Repository;
using SteveLauncher.API.Service;
using SteveLauncher.Domain.Entity;
using SteveLauncher.Extension;

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
            result.Add(info.ToMinecraftServerInfo());
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
        var existServer =  this.localServerListRepository.FindServer(hostname);
        if (existServer is not null)
            throw new MinecraftServerAlreadyExistException($"Server : '{hostname}' is already exist");
        var srv = await this.dnsService.executeAsync(hostname);
        var result = serverRepository.FetchServer(new MinecraftHost(srv,hostname));
        if (result.ServerUpdatable.isOnline || result.ServerUpdatable.Motd is not null)
            return await this.localServerListRepository.AddServer(new(hostname, srv));
        else
            throw new MinecraftServerNotFoundException($"Server : '{hostname}' is not found");
    }

    public async Task<MinecraftServerInfo> FetchServerInfo(MinecraftURL hostname) {
        var srv = await dnsService.executeAsync(hostname);
        var host = new MinecraftHost(srv, hostname);
        return serverRepository.FetchServer(host).ToMinecraftServerInfo();
    }
}