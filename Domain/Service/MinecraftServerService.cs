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
    private const int MaxConcurrentTasks = 5;
    public MinecraftServerService(
        ILocalServerListRepository localServerListRepository,
        IMinecraftServerStatusRepository serverRepository,
        IDnsCheckService dnsService) {
        this.serverRepository = serverRepository;
        this.localServerListRepository = localServerListRepository;
        this.dnsService = dnsService;
    }

    public async Task<ICollection<MinecraftServerInfo>> GetServerStatusListAsync()
    {
        var result = new List<MinecraftServerInfo>();
        var list = localServerListRepository.GetServerList();

        // SemaphoreSlim을 사용하여 동시에 실행 가능한 작업 수 제한
        using (var semaphore = new SemaphoreSlim(MaxConcurrentTasks))
        {
            var tasks = list.Select(async host =>
            {
                await semaphore.WaitAsync(); // 작업 시작 전에 세마포어에서 토큰을 얻음
                try
                {
                    var info = await serverRepository.FetchServerAsync(host.SRVHost);
                    lock (result) 
                    {
                        result.Add(info.ToMinecraftServerInfo());
                    }
                }
                finally
                {
                    semaphore.Release(); 
                }
            });

            // 모든 작업이 완료될 때까지 기다림
            await Task.WhenAll(tasks);
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
        var result = await serverRepository.FetchServerAsync(srv);
        if (result.ServerUpdatable.isOnline)
            return await this.localServerListRepository.AddServer(new (srv,hostname));
        else
            throw new MinecraftServerNotFoundException($"Server : '{hostname}' is not found");
    }

    public async Task<MinecraftServerInfo> FetchServerInfo(MinecraftURL hostname) {
        return (await serverRepository.FetchServerAsync(hostname)).ToMinecraftServerInfo();
    }

    public async Task<MinecraftServerInfo?> FetchTempServerInfo(MinecraftURL hostname) {
        var srv = await this.dnsService.executeAsync(hostname);
        return await FetchServerInfo(srv);

    }
}