using McLib.Model.Network;
using McLib.Model.Network.Dns;
using McMotd;

namespace SteveLauncher.Domain.Entity;

public class MinecraftServerInfo {
    public bool isOnline { get; set; }
    public Motd Motd { get; set; }
    public string Icon { get; set; }
    public MinecraftURL HostName { get; set; }
    public string Version { get; set; }
    public MinecraftPlayer PlayerInfo { get; set; }

    public List<string>? AvilableVersions { get; set; }
}

public class MinecraftPlayer {
    public int Max { get; set; }
    public int Currnet { get; set; }
    public ObservableCollection<MinecraftPlayerName> Players { get; set; }
}

public class MinecraftPlayerName {
    public string Name { get; set; }
    public string IconUrl => $"https://cravatar.eu/helmhead/{Name}/600.png";

}