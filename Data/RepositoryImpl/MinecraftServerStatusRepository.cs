using McLib.API.Services;
using McLib.Model.Network.Dns;
using McLib.Model.Network.Mc;
using SteveLauncher.API.Repository;

namespace SteveLauncher.Data.RepositoryImpl;

public class MinecraftServerStatusRepository: IMinecraftServerStatusRepository {
    private readonly IMcStatusRequestService mcStatusRequestService;
    private readonly IDnsCheckService dnsCheckService;
    public MinecraftServerStatusRepository(
        IMcStatusRequestService mcStatusRequestService,
        IDnsCheckService dnsCheckService
        ) {
        this.mcStatusRequestService = mcStatusRequestService;
        this.dnsCheckService = dnsCheckService;
    }
    public async Task<McServerInfo> FetchServer(MinecraftURL hostname) {
        return await this.mcStatusRequestService.excuteAsync(hostname);
    }
}