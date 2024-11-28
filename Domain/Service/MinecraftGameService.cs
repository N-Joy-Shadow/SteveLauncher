using System.Reflection;
using CmlLib.Core;
using CmlLib.Core.Auth;
using CmlLib.Core.Installers;
using CmlLib.Core.ProcessBuilder;
using CmlLib.Core.Version;
using CommunityToolkit.Mvvm.Messaging;
using McLib.Auth.Model.Minecraft;
using McLib.Model.Network.Dns;
using SteveLauncher.API.Enum;
using SteveLauncher.API.Exception;
using SteveLauncher.API.Repository;
using SteveLauncher.API.Service;
using SteveLauncher.Domain.Entity;
using SteveLauncher.Extension;
using SteveLauncher.Views.Home.Message;

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

            var launcher = new MinecraftLauncher(new MinecraftPath(setting.MinecraftPath));
            
            launcher.FileProgressChanged += LauncherOnFileProgressChanged;
            launcher.ByteProgressChanged += LauncherOnByteProgressChanged;
            
            var process = await launcher.InstallAndBuildProcessAsync(version, new MLaunchOption {
                Session = session,
                ScreenWidth = setting.Width,
                ScreenHeight = setting.Height,
                MaximumRamMb = setting.AllocatedMemory,
                MinimumRamMb = setting.AllocatedMemory,
                ServerPort = url.Port,
                ServerIp = url.HostName,
#if MACCATALYST
                DockName = "SteveLauncher-Minecraft",
                DockIcon = "appicon.svg"
#endif
            });
            

            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.OutputDataReceived += ProcessOnOutputDataReceived;
            
            process.Start();
            process.BeginOutputReadLine();
        }
        catch (Exception e) {
            if (e is UnauthorizedAccessException) {
                throw new UnreachableException();
            }

            if (e is StorageKeyNotFoundException) {
                var rkey = (string)e.Data["key"];
                var key = Enum.TryParse(rkey, out StorageEnum result);
                if(!key)
                    throw new StorageKeyNotFoundException(rkey);
                switch (result) {
                    case StorageEnum.USER_PROFILE:
                        throw new MinecraftLauncherNotAuthorizedException();
                }
                Debug.WriteLine(e.Message);
                return;
            }
            else {
                Debug.WriteLine(e.Message);
            }
        }
    }

    private void ProcessOnOutputDataReceived(object sender, DataReceivedEventArgs e) {
        Debug.WriteLine(e.Data);
        //WeakReferenceMessenger.Default.Send<>(e.Data);
    }

    private void LauncherOnByteProgressChanged(object? sender, ByteProgress e) {
        WeakReferenceMessenger.Default.Send<MinecraftByteProgressMessage>(new MinecraftByteProgressMessage(e.ToRatio()));
    }

    private void LauncherOnFileProgressChanged(object? sender, InstallerProgressChangedEventArgs e) {
        WeakReferenceMessenger.Default.Send<MinecraftInstallProgressMessage>(new MinecraftInstallProgressMessage(e));
    }
    
    

    public void SetSettings(MinecraftGameSetting setting) {
        this.storageRepository.Insert(StorageEnum.GAME_SETTING, setting);
    }


    public async void SetGamePath(string hostname) {
        
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