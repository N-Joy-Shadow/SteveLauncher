using Microsoft.Maui.LifecycleEvents;
using UraniumUI;

namespace SteveLauncher;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
			.UseUraniumUI()
			.UseUraniumUIMaterial()
			.UseSkiaSharp()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("MaterialIcons-Regular.ttf", "MaterialSymbol");
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				fonts.AddMaterialIconFonts();
			});
		builder.Services.UsePageResolver();
		
		
#if WINDOWS
    builder.ConfigureLifecycleEvents(events =>
    {
        events.AddWindows(windows =>
        {
            windows.OnWindowCreated(window =>
            {
                var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
                var appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hwnd));
                
                // 타이틀 바 커스터마이징
                if (appWindow != null)
                {
                    var titleBar = appWindow.TitleBar;
                    
                    // 타이틀 바 숨기기
                    titleBar.ExtendsContentIntoTitleBar = true;
                    titleBar.ButtonBackgroundColor = Microsoft.UI.Colors.Transparent;
                    titleBar.ButtonInactiveBackgroundColor = Microsoft.UI.Colors.Transparent;
                }
            });
        });
    });
#endif

		return builder.Build();
	}
}
