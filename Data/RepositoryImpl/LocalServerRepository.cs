using McLib.Extension;
using McLib.Model.Network;
using SteveLauncher.API.Repository;
using SteveLauncher.Data.Database;
using SteveLauncher.Domain.Entity;

namespace SteveLauncher.Data.RepositoryImpl;

public class LocalServerRepository : ILocalServerListRepository {
    private readonly SteveDbContext context;

    public LocalServerRepository(SteveDbContext context) {
        this.context = context;
    }

    public List<MinecraftMultiHost> GetServerList() {
        return this.context.LocalServerList.Select(x =>
                new MinecraftMultiHost(x.SRVHostName, x.SRVPort, x.HostName, x.Port))
            .ToList();
    }

    public async Task<bool> AddServer(MinecraftHost host,MinecraftHost srvHost) {
        var result = this.context.LocalServerList.FindOneMinecraft(host);
        if (result is not null)
            return false;


        try {
            this.context.LocalServerList.Add(new LocalServerListDatabase() {
                HostName = host.DoaminName,
                Port = host.Port,
                SRVHostName = srvHost is null ? host.DoaminName : srvHost.DoaminName,
                SRVPort = srvHost is null ? host.Port : srvHost.Port
            });

            this.context.SaveChanges();
            return true;
        }
        catch (Exception E) {
            //TODO: 예외 만들기
        }

        return false;
    }


    public bool RemoveServer(MinecraftHost server) {
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


    public MinecraftMultiHost? FindServer(MinecraftHost server) {
        var result = this.context.LocalServerList.FindOneMinecraft(server);
        if (result is null)
            return null;
        return new MinecraftMultiHost(result.SRVHostName, result.SRVPort, result.HostName, result.Port);
    }
}