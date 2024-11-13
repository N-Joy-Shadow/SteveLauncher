using Maui.Plugins.PageResolver.Attributes;
using SteveLauncher.API.Service;
using SteveLauncher.Data.Database;
using SteveLauncher.Domain.Entity;

namespace SteveLauncher.Views.Home;

[Singleton]
public partial class HomeViewModel : BaseViewModel {
    
    [ObservableProperty]
    public ObservableCollection<MinecraftServerInfo> serverStatusList = new();
    
    private readonly IMinecraftServerService serverService;
    public HomeViewModel(
        IMinecraftServerService minecraftServerService
        ) {
        this.serverService = minecraftServerService;
        
    }

    //TODO: 최적화 필요 4개 있으니 ㅈㄴ 느림
    public async Task LoadServerStatusAsync() {
        foreach (var info in await this.serverService.GetServerStatusList()) {
            ServerStatusList.Add(info);
        }

    }

    [RelayCommand]
    async Task GetServerInfo(MinecraftServerInfo serverInfo) {
        Debug.WriteLine(serverInfo.Motd.ToString());
    }

    [RelayCommand]
    async Task DeleteServer(MinecraftServerInfo serverInfo) {
        Debug.WriteLine(serverInfo.Motd.ToString());
    }
}
