using McLib.Model.Network.Dns;

namespace SteveLauncher.Extension;

public static class MinecraftURLExtension {
    public static string ToDirectoryFriendly(this MinecraftURL url) {
        return url.ToString().Replace(".", "_");
    }
}