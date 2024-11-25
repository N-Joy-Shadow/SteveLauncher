using CmlLib.Core;
using SteveLauncher.API.Service;
namespace SteveLauncher.Domain.Service;

public class MinecraftGameService: IMinecraftGameService {
    public MinecraftGameService(
        ) {
        
    }

    public async void StartGame() {
        var path = new MinecraftPath();
        var launcher = new MinecraftLauncher(path);
    }

    public void SetSettings(object setting) {
        throw new NotImplementedException();
    }


    public void SetGamePath(string Path) {
        throw new NotImplementedException();
    }

    public void SetGameVersion(string version) {
        throw new NotImplementedException();
    }
}

