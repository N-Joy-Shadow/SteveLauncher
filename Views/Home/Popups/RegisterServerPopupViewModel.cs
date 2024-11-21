using CommunityToolkit.Mvvm.Messaging;
using McLib.Model.Network.Dns;
using SteveLauncher.API.Repository;
using SteveLauncher.API.Service;
using SteveLauncher.Views.Home.Message;

namespace SteveLauncher.Views.Home.Popups;

public partial class RegisterServerPopupViewModel : BaseViewModel {
    private readonly IMinecraftServerService serverService;

    public event Action<MinecraftURL> OnClosePopup;
    
    [ObservableProperty] 
    private string hostname = "";
    
    public RegisterServerPopupViewModel(
        IMinecraftServerStatusRepository serverRepository) {
        this.serverService = serverService;
    }


    [RelayCommand]
    async Task SubmitServer() {
        if (Hostname is null)
            return;
        MinecraftURL host = (MinecraftURL)Hostname;
        
        var res = await serverService.RegisterServer(host);
        // WeakReferenceMessenger.Default.Send(new ServerAddedMessage(res));
        //
        // if (res) {
        //     WeakReferenceMessenger.Default.Send(new ServerAddedMessage(res));
        // }
    }
}