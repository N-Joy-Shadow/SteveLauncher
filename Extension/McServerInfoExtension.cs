using CommunityToolkit.Maui.Core.Extensions;
using McLib.Model.Network.Dns;
using McLib.Model.Network.Mc;
using SteveLauncher.Domain.Entity;

namespace SteveLauncher.Extension;

public static class McServerInfoExtension {
    public static MinecraftServerInfo ToMinecraftServerInfo(this McServerInfo info) {
        return new() {
            isOnline = info.ServerUpdatable.isOnline,
            Motd = info.ServerUpdatable.Motd,
            Icon = info.ServerUpdatable.Icon,
            HostName = new MinecraftURL(info.HostName,info.Port),
            Version = info.ServerUpdatable.Version,
            PlayerInfo = new() {
                Max = info.ServerUpdatable.MaxPlayer ?? 0,
                Currnet = info.ServerUpdatable.CurrentPlayer ?? 0,
                UserNames = info.ServerUpdatable.Player.Select(x => x.Name).ToHashSet().ToObservableCollection()
            }
        };
    }
}