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
    private readonly IStorageRepository storageRepository;
    private string version = "1.21.1"; //나중에 바꾸기

    public MinecraftGameService(
        IMinecraftLoginRepository loginRepository,
        IStorageRepository storageRepository
    ) {
        this.loginRepository = loginRepository;
        this.storageRepository = storageRepository;
    }

    public async Task StartGame(MinecraftURL url) {
        try {
            var session = GetSession();
            var setting = GetSetting();

            var launcher = new MinecraftLauncher(GetGamePath());

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
                throw new UnreachableException();
            }

            if (e is StorageKeyNotFoundException) {
                var key = (string)e.Data["key"];
                if(Enum.TryParse(key, out StorageEnum result) && result == StorageEnum.USER_PROFILE) {
                    throw new MinecraftLauncherNotAuthorizedException();
                }
                return;
            }
            else {
                Debug.WriteLine(e.Message);
            }
        }
    }

    public void SetSettings(MinecraftGameSetting setting) {
        this.storageRepository.Insert(StorageEnum.GAME_SETTING, setting);
    }


    public async void SetGamePath(string path) {
        this.storageRepository.Insert(StorageEnum.GAME_PATH, path);
    }

    public MinecraftPath GetGamePath() {
        var path = this.storageRepository.Get<string>(StorageEnum.GAME_PATH);
        return new MinecraftPath(Path.Combine(path, MinecraftGameSetting.GameDirectoryName));
    }

    public void SetGameVersion(string version) {
        this.version = version;
    }

    public MinecraftGameSetting? GetSetting() {
        try {
            return storageRepository.Get<MinecraftGameSetting>(StorageEnum.GAME_SETTING);
        }
        catch (Exception e) {
            if (e is StorageException) {
                storageRepository.Insert(StorageEnum.GAME_SETTING, MinecraftGameSetting.InitialSetting());
            }
        }

        return GetSetting();
    }

    private MSession GetSession() {
        var value = this.storageRepository.Get<McUserProfile>(StorageEnum.USER_PROFILE);
        return MSessionExtension.CreateMSession(value);
    }
}