using CommunityToolkit.Mvvm.Messaging;
using McLib.Exceptions;
using McLib.Model.Network.Dns;
using SteveLauncher.API.Enum;
using SteveLauncher.API.Repository;
using SteveLauncher.API.Service;
using SteveLauncher.Domain.Entity;
using SteveLauncher.Views.Home.Message;

namespace SteveLauncher.Views.Home.Popups;

public partial class RegisterServerPopupViewModel : BaseViewModel {
    private readonly IMinecraftServerService serverService;

    public event Action<bool> OnClosePopup;

    [ObservableProperty]
    private string hostname = "";

    [ObservableProperty]
    private ServerRegisterStateEnum serverState = ServerRegisterStateEnum.None;

    [ObservableProperty]
    private MinecraftServerInfo? serverInfo = null;

    [ObservableProperty]
    private bool isLoading = false;
    public RegisterServerPopupViewModel(
        IMinecraftServerService serverService) {
        this.serverService = serverService;
    }


    [RelayCommand]
    async Task ClosePopup() {
        OnClosePopup?.Invoke(false);
    }

    [RelayCommand]
    async Task Dummy(MinecraftServerInfo info) {
    }

    [RelayCommand]
    async Task SubmitServer() {
        IsLoading = true;
        if (string.IsNullOrEmpty(Hostname))
            return;

        ServerState = ServerRegisterStateEnum.Loading;
        var res = await serverService.FetchServerInfo(new MinecraftURL(Hostname));
        ServerInfo = res;
        ServerState = ServerRegisterStateEnum.Loaded;
        
        IsLoading = false;
    }

    [RelayCommand]
    async Task RegisterServer() {
        IsLoading = true;
        if (ServerInfo is null)
            return;
        try {
            Task.Run(() => {
                var res = serverService.RegisterServer(new MinecraftURL(Hostname)).Result;

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    if (!res)
                        ServerState = ServerRegisterStateEnum.Error;
                    WeakReferenceMessenger.Default.Send<ServerListUpdateMessage>(new(res));
                    IsLoading = false;

                });

            });
        }
        catch (Exception e) {
            if (e is NotFoundSRVNameException) {
                ServerState = ServerRegisterStateEnum.Error;
            }

            ServerState = ServerRegisterStateEnum.Error;
        }
        IsLoading = false;

    }
    
}