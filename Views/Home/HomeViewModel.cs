using CommunityToolkit.Maui.Core;
using Maui.Plugins.PageResolver.Attributes;
using SteveLauncher.API.Service;
using SteveLauncher.Data.Database;
using SteveLauncher.Domain.Entity;
using SteveLauncher.Utils;
using SteveLauncher.Views.Home.Popups;

namespace SteveLauncher.Views.Home;

public partial class HomeViewModel : BaseViewModel {
    private readonly IMinecraftServerService serverService;
    private readonly IPopupService popupService;

    [ObservableProperty] private RangeObservableCollection<MinecraftServerInfo> serverStatusList = new();

    //바인딩 안해도 됨
    [ObservableProperty] private bool isLoading = true;

    [ObservableProperty] private MinecraftServerInfo? selectedServerInfo = null;
    public event Action<MinecraftServerInfo> OnServerInfoChange;

    public HomeViewModel(
        IMinecraftServerService minecraftServerService,
        IPopupService popupService
    ) {
        this.serverService = minecraftServerService;
        this.popupService = popupService;
    }

    public async void LoadServerStatusAsync() {
        try {
            var serverStatusList = await Task.Run(() => serverService.GetServerStatusList());
            this.ServerStatusList.Clear();
            this.ServerStatusList.AddRange(serverStatusList);

        }
        catch (Exception e) {
            Debug.WriteLine(e);
        }
        finally {
            IsLoading = false;
        }
    }

    [RelayCommand]
    async Task GetServerInfo(MinecraftServerInfo serverInfo) {
        SelectedServerInfo = serverInfo;
        OnServerInfoChange.Invoke(serverInfo);
    }

    [RelayCommand]
    async Task DeleteServer(MinecraftServerInfo serverInfo) {
        serverService.DeleteServer(serverInfo);
    }

    [RelayCommand]
    async Task ShowRegisterPopup() {
        popupService.ShowPopup<RegisterServerPopupViewModel>();
    }
}