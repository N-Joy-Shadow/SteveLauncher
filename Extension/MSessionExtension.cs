using CmlLib.Core.Auth;
using McLib.Auth.Model.Authentication;
using McLib.Auth.Model.Minecraft;

namespace SteveLauncher.Extension;

public class MSessionExtension {
    public MSessionExtension() {

    }

    public static MSession Create(McUserProfile profile) {
        return new MSession {
            Username = profile.UserName,
            UUID = profile.UUID,
            AccessToken = profile.AccessToken,
            UserType = "msa",
            Xuid = profile.Xuid
        };
    }
}