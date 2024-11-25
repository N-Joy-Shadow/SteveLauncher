using CommunityToolkit.Mvvm.Messaging;
using McLib.Model.Network.Dns;
using SteveLauncher.API.Repository;
using SteveLauncher.API.Service;
using SteveLauncher.Views.Home.Message;

namespace SteveLauncher.Views.Home.Popups;

public partial class RegisterServerPopupViewModel : BaseViewModel {
    private readonly IMinecraftServerService serverService;

    public event Action<bool> OnClosePopup;
    
    [ObservableProperty] 
    private string hostname = "";
    
    public RegisterServerPopupViewModel(
        IMinecraftServerService serverService) {
        this.serverService = serverService;
    }


    [RelayCommand]
    async Task SubmitServer() {
        if (string.IsNullOrEmpty(Hostname))
            return;
        var res = await serverService.RegisterServer(new MinecraftURL(Hostname));
        
        OnClosePopup?.Invoke(res);
    }
}