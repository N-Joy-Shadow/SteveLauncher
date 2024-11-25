﻿using McLib.Model.Network.Dns;

namespace SteveLauncher.Domain.Entity;

public class MinecraftHost {
    public MinecraftURL SRVHost { get; set; }
    public MinecraftURL Host { get; set; }
    
    public MinecraftHost(MinecraftURL SRVHost, MinecraftURL Host) {
        this.SRVHost = SRVHost;
        this.Host = Host;
    }
    
    public MinecraftHost(string SRVHostName, ushort SRVPort,string HostName, ushort Port) {
        this.Host = new MinecraftURL(HostName, Port);
        this.SRVHost = new MinecraftURL(SRVHostName, SRVPort);
    }
}