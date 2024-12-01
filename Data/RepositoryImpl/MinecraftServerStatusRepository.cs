using McLib.API.Services;
using McLib.Model.Network.Dns;
using McLib.Model.Network.Mc;
using SteveLauncher.API.Repository;
using SteveLauncher.Domain.Entity;

namespace SteveLauncher.Data.RepositoryImpl;

public class MinecraftServerStatusRepository: IMinecraftServerStatusRepository {
    private readonly IMcStatusRequestService mcStatusRequestService;
    private readonly IDnsCheckService dnsCheckService;
    public MinecraftServerStatusRepository(
        IMcStatusRequestService mcStatusRequestService,
        IDnsCheckService dnsCheckService
        ) {
        this.mcStatusRequestService = mcStatusRequestService;
    }
    public Task<McServerInfo> FetchServerAsync(MinecraftURL hostname) {
        return this.mcStatusRequestService.executeWithSRVAsync(hostname);
    }
}