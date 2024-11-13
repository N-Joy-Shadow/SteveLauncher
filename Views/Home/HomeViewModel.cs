using Maui.Plugins.PageResolver.Attributes;
using SteveLauncher.API.Service;
using SteveLauncher.Data.Database;
using SteveLauncher.Domain.Entity;

namespace SteveLauncher.Views.Home;

[Singleton]
public partial class HomeViewModel : BaseViewModel {
    
    [ObservableProperty]
    public ObservableCollection<MinecraftServerInfo> serverStatusList = new();
    
    private readonly IMinecraftServerService serverService;
    private readonly SteveDbContext context;
    public HomeViewModel(
        IMinecraftServerService minecraftServerService,
        SteveDbContext context
        ) {
        this.context = context;
        this.serverService = minecraftServerService;
        
    }

    public async Task LoadServerStatusAsync() {
        foreach (var info in await this.serverService.GetServerStatusList()) {
            serverStatusList.Add(info);
            
        }
    }

    [RelayCommand]
    async Task GetServerInfo() {
        
    }

    [RelayCommand]
    async Task DeleteServer() {
    //    this.serverService.DeleteServer(serverInfo);
    //    serverStatusList.Remove(serverInfo);
    }
}
