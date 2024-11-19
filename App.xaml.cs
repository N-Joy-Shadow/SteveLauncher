using SteveLauncher.Domain.Service;
using SteveLauncher.Views.Home;

#if MACCATALYST
using UIKit;
#endif

namespace SteveLauncher;

public partial class App : Application {
    private readonly IServiceProvider serviceProvider;

    public App(IServiceProvider serviceProvider) {
        InitializeComponent();
        //CultureInfo.CurrentCulture = CultureInfo.CurrentUICulture = new CultureInfo("ko");
        this.serviceProvider = serviceProvider;
        MainPage = serviceProvider.GetService<Home>();
    }

    protected override Window CreateWindow(IActivationState? activationState) {
        var window = new Window();
        window.HandlerChanged += WindowHandlerChanged;
        var root = serviceProvider.GetService<Home>();
        window.Title = "SteveLauncher";
        window.Page = root;
        return window;
    }

    private void WindowHandlerChanged(object? sender, EventArgs e) {
        var window = sender as Microsoft.Maui.Controls.Window;

#if MACCATALYST
        var uiWindow = window.Handler.PlatformView as UIWindow;
        if (uiWindow != null) {
            uiWindow.WindowScene.Titlebar.TitleVisibility = UITitlebarTitleVisibility.Hidden;
            uiWindow.WindowScene.Titlebar.ToolbarStyle = UITitlebarToolbarStyle.Expanded;
        }
#elif WINDOWS
#endif
    }
}