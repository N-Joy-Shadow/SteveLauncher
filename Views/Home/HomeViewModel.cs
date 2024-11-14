using CommunityToolkit.Maui.Core;
using Maui.Plugins.PageResolver.Attributes;
using SteveLauncher.API.Service;
using SteveLauncher.Data.Database;
using SteveLauncher.Domain.Entity;
using SteveLauncher.Views.Home.Popups;

namespace SteveLauncher.Views.Home;

public partial class HomeViewModel : BaseViewModel {
    private readonly IMinecraftServerService serverService;
    private readonly IPopupService popupService;
    
    [ObservableProperty]
    public ObservableCollection<MinecraftServerInfo> serverStatusList = new();

    [ObservableProperty]
    private MinecraftServerInfo? selectedServerInfo = null;
    public event Action<MinecraftServerInfo> OnServerInfoChange;
    
    public HomeViewModel(
        IMinecraftServerService minecraftServerService,
        IPopupService popupService
        ) {
        this.serverService = minecraftServerService;
        this.popupService = popupService;
    }

    //TODO: 최적화 필요 4개 있으니 ㅈㄴ 느림
    public void LoadServerStatusAsync() {
        foreach (var info in this.serverService.GetServerStatusList()) {
            ServerStatusList.Add(info);
        }
    }

    [RelayCommand]
    async Task GetServerInfo(MinecraftServerInfo serverInfo) {
        selectedServerInfo = serverInfo;
        OnServerInfoChange.Invoke(serverInfo);
    }

    [RelayCommand]
    async Task DeleteServer(MinecraftServerInfo serverInfo) {
        serverService.DeleteServer(serverInfo);
    }

    [RelayCommand]
    async Task ShowRegisterPopup() {
        this.popupService.ShowPopup<RegisterServerPopupViewModel>();
    }
}
