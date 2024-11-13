using CmlLib.Core.Auth.Microsoft;
using Maui.Plugins.PageResolver.Attributes;
using XboxAuthNet.Game;

namespace SteveLauncher.Domain.Service;

public class MinecraftLoginService {
    private readonly HttpClient client;
    public MinecraftLoginService(
        HttpClient httpClient) {
        
        

    }
    async Task Login() {
        var loginHandler = JELoginHandlerBuilder.BuildDefault();
        var session = await loginHandler.Authenticate();       
        
    }
}