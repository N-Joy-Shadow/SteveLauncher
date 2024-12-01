using McLib.API.Services;
using McLib.Auth.API;
using McLib.Auth.Service;
using McLib.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Maui.LifecycleEvents;
using SteveLauncher.Data.Database;
using Microsoft.Extensions.Logging;
using SteveLauncher.API.Repository;
using SteveLauncher.API.Service;
using SteveLauncher.Data.RepositoryImpl;
using SteveLauncher.Domain.Service;
using SteveLauncher.Utils.Popups;
using SteveLauncher.Views.GameLog;
using SteveLauncher.Views.Home;
using SteveLauncher.Views.Setting;
using SteveLauncher.Views.Login;
using SteveLauncher.Views.RegisterServer;
using UraniumUI;

#if MACCATALYST							
using UIKit;
#endif

#if WINDOWS
using SteveLauncher.WinUI;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Windows.Graphics;
using Microsoft.UI.Composition.SystemBackdrops;
using Microsoft.UI.Xaml.Media;
using Colors = Microsoft.UI.Colors;
#endif
namespace SteveLauncher;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit(options => {
				options.SetShouldEnableSnackbarOnWindows(true);
			})
			.UseUraniumUI()
			.UseUraniumUIMaterial()
			.UseUraniumUIBlurs()
			.UseSkiaSharp()
			.UsePageResolver(true)
			.UseAutodependencies()
			
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("Monocraft.ttf","Monocraft");
				fonts.AddFont("Monocraft-Bold.ttf","MonocraftBold");
				fonts.AddFont("Monocraft-SemiBold.ttf","MonocraftSemiBold");
				fonts.AddFont("Monocraft-Italic.ttf","MonocraftItalic");
				fonts.AddFont("Monocraft-Light.ttf","MonocraftLight");
				fonts.AddMaterialIconFonts();
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif
		
		
		builder.Services.AddDbContext<SteveDbContext>(options => {
#if DEBUG
			options.UseInMemoryDatabase("steveLauncher");		
#else
			options.UseSqlite($"Data Source={Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "steveLauncher.db")}");
#endif
		});

		//external services
		builder.Services.AddSingleton<IMLDnsService,MLDnsService>();
		builder.Services.AddSingleton<IMLServerStatusService,MLServerStatusService>();

		builder.Services.AddSingleton<IMcLoginService, McLoginService>();
		
		//Views
		builder.Services.AddSingleton<Home>();
		builder.Services.AddSingleton<GameLog>();
		
		//Popup
		builder.Services.AddTransientPopup<RegisterServerPopup,RegisterServerPopupViewModel>();
		builder.Services.AddTransientPopup<SettingPopup,SettingPopupViewModel>();
		builder.Services.AddTransientPopup<Login,LoginViewModel>();

		//Repositories
		builder.Services.AddSingleton<ILocalServerListRepository, LocalServerRepository>();
		builder.Services.AddSingleton<IStorageRepository, StorageRepository>();
		//misc
		builder.Services.AddSingleton<PopupSizeConstants>();
		builder.Services.AddSingleton<IDeviceDisplay>(DeviceDisplay.Current);
		builder.Services.AddSingleton<IDirectoryLaunchService, DirectoryLaunchService>();
		builder.Services.AddHttpClient();
#if MACCATALYST
		builder.ConfigureLifecycleEvents(events => {
			events.AddiOS(osx => {
				osx.SceneWillConnect((scene, session, options) => {
					if (scene is UIWindowScene) {
						var windowScene = scene as UIWindowScene;
						if (windowScene.Titlebar != null) {
							windowScene.Titlebar.TitleVisibility = UITitlebarTitleVisibility.Hidden;
							windowScene.Titlebar.Toolbar = null;
						}
					}
					else {
						Debug.WriteLine("Scene is not UIWindowScene");
					}
				});
			});			
		});
#elif WINDOWS
		builder.ConfigureLifecycleEvents(events => {
			events.AddWindows(windowEvent => {
				windowEvent.OnWindowCreated(window => {
					#if WINDOWS10_0_17763_0_OR_GREATER
					window.SystemBackdrop = new DesktopAcrylicBackdrop();
					#endif
					var nativeWindow = window as Microsoft.UI.Xaml.Window;
					nativeWindow.AppWindow.Resize(new Windows.Graphics.SizeInt32 { Width = 1600, Height = 900 });
					
					var handle = WinRT.Interop.WindowNative.GetWindowHandle(window);               
					var id = Win32Interop.GetWindowIdFromWindow(handle);

					var appWindow = AppWindow.GetFromWindowId(id);
					
					//appWindow.Resize(new SizeInt32() {Width = 1280, Height = 720});
					var titleBar = appWindow.TitleBar;
					appWindow.TitleBar.ExtendsContentIntoTitleBar = true;

                });
			});
		});
#endif
		
		return builder.Build();
	}
#if MACCATALYST
	private static void SceneWillConnectDelegate(UIScene scene, UISceneSession session, UISceneConnectionOptions connectionoptions) {
		if (scene is UIWindowScene windowScene) {
			if (windowScene.Titlebar != null) {
				windowScene.Titlebar.TitleVisibility = UITitlebarTitleVisibility.Hidden;
				windowScene.Titlebar.Toolbar = null;
			}
		}
	}
#endif
}
