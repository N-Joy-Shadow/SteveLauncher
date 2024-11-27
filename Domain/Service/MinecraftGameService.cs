using System.Reflection;
using CmlLib.Core;
using CmlLib.Core.Auth;
using CmlLib.Core.Installers;
using CmlLib.Core.ProcessBuilder;
using CmlLib.Core.Version;
using McLib.Auth.Model.Minecraft;
using McLib.Model.Network.Dns;
using SteveLauncher.API.Enum;
using SteveLauncher.API.Exception;
using SteveLauncher.API.Repository;
using SteveLauncher.API.Service;
using SteveLauncher.Domain.Entity;
using SteveLauncher.Extension;

namespace SteveLauncher.Domain.Service;

public class MinecraftGameService : IMinecraftGameService {
    private readonly IMinecraftLoginRepository loginRepository;
    private readonly ISecureStorageRepository secureStorageRepository;
    private string version = "1.21.1"; //나중에 바꾸기
    public MinecraftGameService(
        IMinecraftLoginRepository loginRepository,
        ISecureStorageRepository secureStorageRepository
    ) {
        this.loginRepository = loginRepository;
        this.secureStorageRepository = secureStorageRepository;
    }

    public async Task StartGame(MinecraftURL url) {
        var session = await GetSession();
        var setting = await this.GetSetting();
        try {
            var path = new MinecraftPath(Path.Combine(FileSystem.AppDataDirectory, "mc_steve_games"));

            var launcher = new MinecraftLauncher(path);
            
            var process = await launcher.InstallAndBuildProcessAsync(version, new MLaunchOption {
                Session = session,
                ScreenWidth = setting.Width,
                ScreenHeight = setting.Height,
                MaximumRamMb = setting.AllocatedMemory,
                MinimumRamMb = setting.AllocatedMemory,
                ServerPort = url.Port,
                ServerIp = url.HostName,
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
        this.secureStorageRepository.InsertAsync(SecureStorageEnum.GAME_SETTING,setting);
    }


    public async void SetGamePath(string path) {
        this.secureStorageRepository.InsertAsync(SecureStorageEnum.GAME_PATH, path);
    }

    public async Task<MinecraftPath> GetGamePath() {
        var path = await this.secureStorageRepository.GetAsync<string>(SecureStorageEnum.GAME_PATH);
        return new MinecraftPath(path);
    }

    public void SetGameVersion(string version) {
        this.version = version;
    }

    public async Task<MinecraftGameSetting?> GetSetting() {
        try {
            return await secureStorageRepository.GetAsync<MinecraftGameSetting>(SecureStorageEnum.GAME_SETTING);
        }catch(Exception e)
        {
            if (e is SecureStorageException) {
                await secureStorageRepository.InsertAsync(SecureStorageEnum.GAME_SETTING ,new MinecraftGameSetting() {
                    AllocatedMemory = 2048,
                    Width = 1280,
                    Height = 720,
                    MinecraftPath = Path.Combine(FileSystem.AppDataDirectory, "mc_steve_games")
                });
            }
        }
#if MACCATALYST
        return new MinecraftGameSetting() {
            AllocatedMemory = 2048,
            Width = 1280,
            Height = 720,
            MinecraftPath = Path.Combine(FileSystem.AppDataDirectory, "mc_steve_games")
        };
#endif
        return await GetSetting();

    }

    private async Task<MSession> GetSession() {
        var value = await this.secureStorageRepository.GetAsync<McUserProfile>(SecureStorageEnum.USER_PROFILE);
        return MSessionExtension.CreateMSession(value);
    }
}