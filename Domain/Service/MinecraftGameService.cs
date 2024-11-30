using System.Reflection;
using CmlLib.Core;
using CmlLib.Core.Auth;
using CmlLib.Core.FileExtractors;
using CmlLib.Core.Installers;
using CmlLib.Core.Java;
using CmlLib.Core.ProcessBuilder;
using CmlLib.Core.Rules;
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
using SteveLauncher.Utils.Minecraft;
using SteveLauncher.Views.Home.Message;

namespace SteveLauncher.Domain.Service;

public class MinecraftGameService : IMinecraftGameService {
    private readonly IStorageRepository storageRepository;
    
    private string version;
    private MinecraftLauncher launcher;
    public MinecraftGameService(
        IStorageRepository storageRepository
    ) {
        this.storageRepository = storageRepository;
    }

    
    public async Task<List<string>> GetAvailableVersions(string version) {
        launcher = new MinecraftLauncher();
        var versions =  await launcher.GetAllVersionsAsync();

        return MinecraftVersionHandler.ParsingVersions(version,versions);
    }
    
    public async Task StartGame(MinecraftURL url) { 
        try {
            var session = GetSession();
            var setting = GetSetting();

            if(string.IsNullOrEmpty(version))
                throw new NullReferenceException("version is null");
            
            var path = Path.Combine(setting.MinecraftPath, url.ToDirectoryFriendly());
            var launcher = new MinecraftLauncher(new MinecraftPath(path));
            
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
                //맥은 자바 문제로 실행이 되지 않음
                JavaPath = "/usr/bin/java",
                //DockName = "SteveLauncher-Minecraft",
                //DockIcon = "AppIcon/appicon.svg" 
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

            if (e is NullReferenceException) {
                throw new NullReferenceException(e.Message);
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
                Debug.WriteLine($"Uncatchable Exception: {e.Message}");
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


    

    public void SetGameVersion(string version) {
        this.version = version;
    }

    public MinecraftGameSetting? GetSetting() {
        try {
            return storageRepository.Get<MinecraftGameSetting>(StorageEnum.GAME_SETTING);
        }
        catch (Exception e) {
            if (e is StorageException) 
                storageRepository.Insert(StorageEnum.GAME_SETTING, MinecraftGameSetting.InitialSetting());
            else
                throw new UnreachableException();
        }

        return GetSetting();
    }

    private MSession GetSession() {
        var value = this.storageRepository.Get<McUserProfile>(StorageEnum.USER_PROFILE);
        return MSessionExtension.CreateMSession(value);
    }
}