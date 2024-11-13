using McLib.Model.Network.Dns;
using SteveLauncher.API.Repository;
using SteveLauncher.Data.Database;
using SteveLauncher.Domain.Entity;

namespace SteveLauncher.Data.RepositoryImpl;

public class LocalServerRepository: ILocalServerListRepository {
    private readonly SteveDbContext context;

    public LocalServerRepository(SteveDbContext context) {
        this.context = context;
    }

    public List<MinecraftHost> GetServerList() {
        return this.context.LocalServerList.Select(x => new MinecraftHost(
            x.HostName, x.Port, x.SRVHostName, x.SRVPort)).ToList();
    }

    public async Task<bool> AddServer(MinecraftHost host) {
        try {
            this.context.LocalServerList.Add(new LocalServerListDatabase() {
                HostName = host.Host.HostName,
                Port = host.Host.Port,
                SRVHostName = host.SRVHost.HostName,
                SRVPort = host.SRVHost.Port
            });

            this.context.SaveChanges();
            return true;
        }
        catch (Exception E) {
            //TODO: 예외 만들기
        }

        return false;
    }
    

    public Task<bool> RemoveServer(MinecraftURL server) {
        throw new NotImplementedException();
    }


    public Task<MinecraftHost> FindServer(string server) {
        throw new NotImplementedException();
    }
}