using Microsoft.Maui.Platform;
using SteveLauncher.Data.Database;
using SteveLauncher.Domain.Service;
using SteveLauncher.Views.Home;

#if MACCATALYST
using UIKit;
#endif

namespace SteveLauncher;

public partial class App : Application {
    private readonly IServiceProvider serviceProvider;

    private const string LastX = "LastWindowLeft";
    private const string LastY = "LastWindowTop";

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

#if WINDOWS
        window.X = Preferences.Get(LastX, 100);
        window.Y = Preferences.Get(LastY, 100);
#endif
        var context = serviceProvider.GetRequiredService<SteveDbContext>();
        context.Database.EnsureCreated();
        
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
#endif
    }

    public override void CloseWindow(Window window) {
#if WINDOWS
        var appWindow = Application.Current.Windows[0].Handler.PlatformView as Microsoft.UI.Xaml.Window;
        Preferences.Set(LastX, window.X);
        Preferences.Set(LastY, window.Y);
#endif
        base.CloseWindow(window);
    }
    
}