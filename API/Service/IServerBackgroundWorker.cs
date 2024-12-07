using Microsoft.Extensions.Hosting;

namespace SteveLauncher.API.Service;

public interface IServerBackgroundWorker : IHostedService, IDisposable {
    Task StartAsync(CancellationToken cancellationToken);
    Task StopAsync(CancellationToken cancellationToken);
}