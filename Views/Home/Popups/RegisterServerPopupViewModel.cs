using McLib.Model.Network.Dns;
using SteveLauncher.API.Repository;
using SteveLauncher.API.Service;

namespace SteveLauncher.Views.Home.Popups;

public partial class RegisterServerPopupViewModel : BaseViewModel {
    private readonly IMinecraftServerService serverService;

    public event Action<string> RecievedAlert;

    [ObservableProperty] 
    private string hostname = "";
    
    public RegisterServerPopupViewModel(
        IMinecraftServerStatusRepository serverRepository) {
        this.serverService = serverService;
    }


    [RelayCommand]
    async Task SubmitServer() {
        MinecraftURL host = (MinecraftURL)Hostname;
        var res = await serverService.RegisterServer(host);
        if(res)
            RecievedAlert?.Invoke("등록 성공");
        else
            RecievedAlert.Invoke("등록 실패");
    }
}