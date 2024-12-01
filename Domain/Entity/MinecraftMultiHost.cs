using McLib.Model.Network;

namespace SteveLauncher.Domain.Entity;

public class MinecraftMultiHost {
    public MinecraftHost SRVHost { get; set; }
    public MinecraftHost Host { get; set; }
    
    public MinecraftMultiHost(MinecraftHost SRVHost, MinecraftHost Host) {
        this.SRVHost = SRVHost;
        this.Host = Host;
    }
    
    public MinecraftMultiHost(string SRVHostName, ushort SRVPort,string HostName, ushort Port) {
        this.Host = new MinecraftHost(HostName, Port);
        this.SRVHost = new MinecraftHost(SRVHostName, SRVPort);
    }
}