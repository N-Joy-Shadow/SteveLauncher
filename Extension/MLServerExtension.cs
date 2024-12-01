using CommunityToolkit.Maui.Core.Extensions;
using McLib.Model.Network;
using McLib.Model.Network.Mc;
using SteveLauncher.Domain.Entity;

namespace SteveLauncher.Extension;

public static class MLServerExtension {
    public static MinecraftServerInfo ToMinecraftServerInfo(this MLServerStatus info, MinecraftHost host) {
        return new() {
            isOnline = info.isOnline,
            Motd = info.Motd,
            Icon = info.Icon,
            HostName = host,
            Version = info.Version,
            PlayerInfo = new() {
                Max = info.MaxPlayer ?? 0,
                Currnet = info.CurrentPlayer ?? 0,
                Players = info.Player.Select(x => new MinecraftPlayerName { Name = x.Name }).ToHashSet().ToObservableCollection()
            }
        };
    }

    public static MinecraftServerInfo ToMinecraftServerInfo(this MLServerInfo info) {
        return info.ServerStatus.ToMinecraftServerInfo(info.Host);
    }
}