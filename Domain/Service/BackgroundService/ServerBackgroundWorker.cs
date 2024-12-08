using SteveLauncher.API.Service;

namespace SteveLauncher.Domain.Service.BackgroundService;

public class ServerBackgroundWorker(IMinecraftServerService minecraftServerService) : IServerBackgroundWorker {
    private readonly IMinecraftServerService minecraftServerService = minecraftServerService;


    private CancellationToken cancellationToken;
    
    public void Dispose() {
        // TODO release managed resources here
    }

    public Task StartAsync(CancellationToken cancellationToken) {
        DoWork();
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken) {
        
        return Task.CompletedTask;
    }

    private void DoWork() {
        Console.WriteLine($"Worker running at: {DateTimeOffset.Now}");
    }
    
}