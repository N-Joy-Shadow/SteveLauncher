using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.Messaging;
using Maui.Plugins.PageResolver.Attributes;
using Microsoft.EntityFrameworkCore.Query;
using SteveLauncher.API.Service;
using SteveLauncher.Data.Database;
using SteveLauncher.Domain.Entity;
using SteveLauncher.Utils;
using SteveLauncher.Views.Home.Message;
using SteveLauncher.Views.Home.Popups;

namespace SteveLauncher.Views.Home;

public partial class HomeViewModel : BaseViewModel {
    private readonly IMinecraftServerService serverService;
    private readonly IPopupService popupService;

    [ObservableProperty]
    private RangeObservableCollection<MinecraftServerInfo> serverStatusList = new();

    //바인딩 안해도 됨
    [ObservableProperty]
    private bool isLoading = true;

    [ObservableProperty]
    private MinecraftServerInfo? selectedServerInfo = null;

    [ObservableProperty]
    private string currentAuthState = "UnAuth";

    public event Action<MinecraftServerInfo> OnServerInfoChange;

    public HomeViewModel(
        IMinecraftServerService minecraftServerService,
        IPopupService popupService
    ) {
        this.serverService = minecraftServerService;
        this.popupService = popupService;
    }

    protected override void BindingMessageCenter() {
        WeakReferenceMessenger.Default.Register<ServerAddedMessage>(this, (r, m) => {
            if (m.Value)
                LoadServerStatusAsync();
        });
    }

    //TOOD: 나중에 커맨드로 빼기
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
        var hostname = await popupService.ShowPopupAsync<RegisterServerPopupViewModel>();
        
    }

    [RelayCommand]
    async Task Login() {
    }

    [RelayCommand]
    async Task Logout() {
    }

    [RelayCommand]
    async Task ShowSettingPopup() {
        var a = await popupService.ShowPopupAsync<SettingPopupViewModel>();
        
    }
}