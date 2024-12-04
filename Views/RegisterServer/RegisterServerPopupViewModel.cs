using CommunityToolkit.Mvvm.Messaging;
using McLib.Exceptions;
using McLib.Model.Network;
using SteveLauncher.API.Enum;
using SteveLauncher.API.Repository;
using SteveLauncher.API.Service;
using SteveLauncher.Domain.Entity;
using SteveLauncher.Views.Home.Message;

namespace SteveLauncher.Views.RegisterServer;

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
        var url = new MinecraftHost(Hostname);
        if (string.IsNullOrEmpty(Hostname))
            return;
        Task.Run(async () => {
            MainThread.BeginInvokeOnMainThread(() => { ServerState = ServerRegisterStateEnum.Loading; });
            var res = await serverService.FetchTempServerInfo(url);
            MainThread.BeginInvokeOnMainThread(() => {
                if (res.isOnline) {
                    ServerState = ServerRegisterStateEnum.Error;
                }

                res.HostName = url;
                ServerInfo = res;
                ServerState = ServerRegisterStateEnum.Loaded;
                IsLoading = false;
            });

        });
    }

    [RelayCommand]
    async Task RegisterServer() {
        IsLoading = true;
        if (ServerInfo is null)
            return;
        try {
            Task.Run(async () => {
                try {
                    MainThread.BeginInvokeOnMainThread(() => { ServerState = ServerRegisterStateEnum.Loading; });
                    var res = await serverService.RegisterServer(new MinecraftHost(Hostname));

                    MainThread.BeginInvokeOnMainThread(() => {
                        if (!res)
                            ServerState = ServerRegisterStateEnum.Error;
                        WeakReferenceMessenger.Default.Send<ServerListUpdateMessage>(new(res));
                        IsLoading = false;
                        OnClosePopup.Invoke(true);
                    });
                }
                catch (Exception e) {
                    MainThread.BeginInvokeOnMainThread(() => {
                        ServerState = ServerRegisterStateEnum.Error;
                        IsLoading = false;
                    });
                }
            });
            return;
        }
        catch (Exception e) {
            if (e is NotFoundSRVNameException) {
                ServerState = ServerRegisterStateEnum.Error;
            }

            ServerState = ServerRegisterStateEnum.Error;
        }

        ServerState = ServerRegisterStateEnum.None;
    }
}