using DotNet.Meteor.HotReload.Plugin;
using McLib.API.Services;
using McLib.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Maui.LifecycleEvents;
using SteveLauncher.Data.Database;
using Microsoft.Extensions.Logging;
using SteveLauncher.API.Repository;
using SteveLauncher.Data.RepositoryImpl;
using SteveLauncher.Utils.Popups;
using SteveLauncher.Views.Home;
using SteveLauncher.Views.Home.Popups;
using SteveLauncher.Views.Login;
using UraniumUI;
using Xe.AcrylicView;

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
			.UseMauiCommunityToolkit()
			//.UseUraniumUI()
			//.UseUraniumUIMaterial()
			.UseSkiaSharp()
			.UsePageResolver(true)
			.UseAutodependencies()
#if MACCATALYST14_2_OR_GREATER || WINDOWS || ANDROID || IOS
			.UseAcrylicView()
#endif
#if DEBUG
			.EnableHotReload()
#endif

			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("Monocraft.ttf","Monocraft");
				fonts.AddFont("Monocraft-Bold.ttf","MonocraftBold");
				fonts.AddFont("Monocraft-SemiBold.ttf","MonocraftSemiBold");
				fonts.AddFont("Monocraft-Italic.ttf","MonocraftItalic");
				fonts.AddFont("Monocraft-Light.ttf","MonocraftLight");
				fonts.AddFont("MaterialIcons-Regular.ttf", "MaterialSymbol");
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				fonts.AddMaterialIconFonts();
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif
		
		
		builder.Services.AddDbContext<SteveDbContext>(options => {
#if DEBUG
			options.UseInMemoryDatabase("steveLauncher");		
#else
			options.UseSqlite($"Data Source={Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "steveLauncher.db")}");
#endif
		});

		//external services
		builder.Services.AddSingleton<IDnsCheckService,DnsCheckService>();
		builder.Services.AddSingleton<IMcStatusRequestService,McStatusRequestService>();
		
		//Views
		builder.Services.AddSingleton<Home>();
		
		
		//Popup
		builder.Services.AddTransientPopup<RegisterServerPopup,RegisterServerPopupViewModel>();
		builder.Services.AddTransientPopup<SettingPopup,SettingPopupViewModel>();
		builder.Services.AddSingleton<Login,LoginViewModel>();

		//Repositories
		builder.Services.AddSingleton<IMinecraftLoginRepository, MinecraftLoginRepository>();
		builder.Services.AddSingleton<ILocalServerListRepository, LocalServerRepository>();
		builder.Services.AddSingleton<IMinecraftServerStatusRepository, MinecraftServerStatusRepository>();
		
		//misc
		builder.Services.AddSingleton<PopupSizeConstants>();

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
					window.SystemBackdrop = new MicaBackdrop() { Kind = MicaKind.BaseAlt };
					//window.SystemBackdrop = new DesktopAcrylicBackdrop();
					#endif
					var handle = WinRT.Interop.WindowNative.GetWindowHandle(window);               
					var id = Win32Interop.GetWindowIdFromWindow(handle);

					var appWindow = AppWindow.GetFromWindowId(id);

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
