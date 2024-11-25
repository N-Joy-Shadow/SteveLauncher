using CommunityToolkit.Maui.Core;
using McLib.Auth.API;
using McLib.Auth.Model.Minecraft;

namespace SteveLauncher.Views.Login;

public partial class LoginViewModel : BaseViewModel {
    private readonly IMcLoginService _loginService;
    private readonly string clientId = "22253aaa-98bd-48b6-a004-f732131d6961";


    public McUserProfile userProfile;
    
    public LoginViewModel(
        IPopupService popupService,
        IMcLoginService loginService) {
        this._loginService = loginService;
    }

    public async Task<bool> LoginAsync(string redirectUrl) {
        if (!redirectUrl.Contains("code=")) {
            return false;
        }
        string code = redirectUrl.Split("=")[1].Split("&")[0];
        try {
            userProfile = await _loginService.LoginAsync(code, clientId);
            return true;
        }catch (Exception e) {
            return false;
        }
        return false;
    }
    
    public string GetAuthUrl() {
        return _loginService.GetLoginUrl(clientId);
    }
}