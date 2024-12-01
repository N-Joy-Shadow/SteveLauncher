using CommunityToolkit.Maui.Core.Extensions;
using Maui.Plugins.PageResolver.Attributes;
using McLib.API.Services;
using McLib.Model.Network;
using SteveLauncher.API.Exception;
using SteveLauncher.API.Repository;
using SteveLauncher.API.Service;
using SteveLauncher.Domain.Entity;
using SteveLauncher.Extension;

namespace SteveLauncher.Domain.Service;

public class MinecraftServerService : IMinecraftServerService {
    private readonly ILocalServerListRepository localServerListRepository;
    private readonly IMLDnsService mlDnsService;
    private readonly IMLServerStatusService serverStatusService;
    private const int MaxConcurrentTasks = 5;

    public MinecraftServerService(
        ILocalServerListRepository localServerListRepository,
        IMLServerStatusService serverRepository,
        IMLDnsService imlDnsService) {
        this.localServerListRepository = localServerListRepository;
        this.serverStatusService = serverRepository;
        this.mlDnsService = imlDnsService;
    }

    public async Task<ICollection<MinecraftServerInfo>> GetServerStatusListAsync() {
        var result = new List<MinecraftServerInfo>();
        var list = localServerListRepository.GetServerList();

        // SemaphoreSlim을 사용하여 동시에 실행 가능한 작업 수 제한
        using (var semaphore = new SemaphoreSlim(MaxConcurrentTasks)) {
            var tasks = list.Select(async host => {
                await semaphore.WaitAsync(); // 작업 시작 전에 세마포어에서 토큰을 얻음
                try {
                    var info = await serverStatusService.FetchServerStatus(host.SRVHost);
                    lock (result) {
                        result.Add(info.ToMinecraftServerInfo(host.Host));
                    }
                }
                finally {
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
    public async Task<bool> RegisterServer(MinecraftHost hostname) {
        var existServer = this.localServerListRepository.FindServer(hostname);
        
        if (existServer is not null)
            throw new MinecraftServerAlreadyExistException($"Server : '{hostname}' is already exist");
        
        try {
            var result = await serverStatusService.FetchServerInfo(hostname);
            if (result.ServerStatus.isOnline)
                return await this.localServerListRepository.AddServer(result.Host,result.SrvHost);
            else
                throw new MinecraftServerNotFoundException($"Server : '{hostname}' is not found");
        }
        catch (Exception E) {
            throw new MinecraftServerNotFoundException(E.Message);
        }
    }

    public async Task<MinecraftServerInfo> FetchServerInfo(MinecraftHost hostname) {
        return (await serverStatusService.FetchServerStatus(hostname)).ToMinecraftServerInfo(hostname);
    }

    public async Task<MinecraftServerInfo?> FetchTempServerInfo(MinecraftHost hostname) {
        return (await serverStatusService.FetchServerInfo(hostname)).ToMinecraftServerInfo();
    }
}