using McLib.Model.Network;

namespace SteveLauncher.Extension;

public static class MinecraftHostExtension {
    public static string ToDirectoryFriendly(this MinecraftHost host) {
        return host.ToString().Replace(".", "_");
    }
}