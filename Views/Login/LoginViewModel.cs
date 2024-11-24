using McLib.Auth.API;

namespace SteveLauncher.Views.Login;

public partial class LoginViewModel : BaseViewModel {
    private readonly IMcLoginService _loginService;
    private readonly string clientId = "22253aaa-98bd-48b6-a004-f732131d6961";
    [ObservableProperty]
    private string authUrl;

    public LoginViewModel(
        IMcLoginService loginService) {
        this._loginService = loginService;
        AuthUrl = loginService.GetLoginUrl(clientId);
    }
}