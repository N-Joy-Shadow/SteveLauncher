using McLib.Extension;
using McLib.Model.Network.Dns;
using SteveLauncher.API.Repository;
using SteveLauncher.Data.Database;
using SteveLauncher.Domain.Entity;

namespace SteveLauncher.Data.RepositoryImpl;

public class LocalServerRepository : ILocalServerListRepository {
    private readonly SteveDbContext context;

    public LocalServerRepository(SteveDbContext context) {
        this.context = context;
    }

    public List<MinecraftHost> GetServerList() {
        return this.context.LocalServerList.Select(x =>
                new MinecraftHost(x.SRVHostName, x.SRVPort, x.HostName, x.Port))
            .ToList();
    }

    public async Task<bool> AddServer(MinecraftHost host) {
        var result = this.context.LocalServerList.FindOneMinecraft(host.Host);
        if (result is not null)
            return false;


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


    public bool RemoveServer(MinecraftURL server) {
        var result = this.context.LocalServerList.FindOneMinecraft(server);
        if (result is null)
            return false;
        try {
            this.context.Remove(result);
            this.context.SaveChanges();
        }
        catch (Exception E) {
            Debug.WriteLine(E.Message);
            return false;
        }
        return true;
    }


    public MinecraftHost? FindServer(MinecraftURL server) {
        var result = this.context.LocalServerList.FindOneMinecraft(server);
        if (result is null)
            return null;
        return new MinecraftHost(result.SRVHostName, result.SRVPort, result.HostName, result.Port);
    }
}