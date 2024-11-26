using System.Reflection;
using CmlLib.Core;
using CmlLib.Core.Auth;
using CmlLib.Core.ProcessBuilder;
using CmlLib.Core.Version;
using McLib.Auth.Model.Minecraft;
using SteveLauncher.API.Enum;
using SteveLauncher.API.Repository;
using SteveLauncher.API.Service;
using SteveLauncher.Domain.Entity;
using SteveLauncher.Extension;

namespace SteveLauncher.Domain.Service;

public class MinecraftGameService : IMinecraftGameService {
    private readonly IMinecraftLoginRepository loginRepository;
    private readonly ISecureStorageRepository secureStorageRepository;

    public MinecraftGameService(
        IMinecraftLoginRepository loginRepository,
        ISecureStorageRepository secureStorageRepository
    ) {
        this.loginRepository = loginRepository;
        this.secureStorageRepository = secureStorageRepository;
    }

    public async Task StartGame(string version) {
        var session = await GetSession();

        try {
            var path = new MinecraftPath(Path.Combine(FileSystem.AppDataDirectory, "mc_steve_games"));

            var launcher = new MinecraftLauncher(path);

            var process = await launcher.InstallAndBuildProcessAsync(version, new MLaunchOption {
                Session = session
            });

            process.Start();
        }
        catch (Exception e) {
            if (e is UnauthorizedAccessException) {
                Debug.WriteLine(e.Message);
                return;
            }
            else {
                Debug.WriteLine(e.Message);
                return;
            }
            
            
        }
    }

    public void SetSettings(MinecraftGameSetting setting) {
        throw new NotImplementedException();
    }


    public async void SetGamePath(string path) {
        this.secureStorageRepository.InsertAsync(SecureStorageEnum.GAME_PATH, path);
    }

    public async Task<MinecraftPath> GetGamePath() {
        var path = await this.secureStorageRepository.GetAsync<string>(SecureStorageEnum.GAME_PATH);
        return new MinecraftPath(path);
    }

    public void SetGameVersion(string version) {
        throw new NotImplementedException();
    }

    public async Task<MinecraftGameSetting?> GetSetting() {
        try {
            return await secureStorageRepository.GetAsync<MinecraftGameSetting>(SecureStorageEnum.GAME_SETTING);
        }catch(Exception e)
        {
            Debug.WriteLine(e.Message);
            return null;
        }
    }

    private async Task<MSession> GetSession() {
        var value = await this.secureStorageRepository.GetAsync<McUserProfile>(SecureStorageEnum.USER_PROFILE);
        return MSessionExtension.CreateMSession(value);
    }
}