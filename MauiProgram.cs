using McLib.API.Services;
using McLib.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Maui.LifecycleEvents;
using SteveLauncher.Data.Database;
using Microsoft.Extensions.Logging;
using SteveLauncher.API.Repository;
using SteveLauncher.Data.RepositoryImpl;
using SteveLauncher.Views.Home;
using SteveLauncher.Views.Login;
using SteveLauncher.Views.Setting;
using UIKit;
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
			.UsePageResolver(true)
			.UseAutodependencies()
			.ConfigureFonts(fonts =>
			{
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

		builder.Services.AddSingleton<IDnsCheckService,DnsCheckService>();
		builder.Services.AddSingleton<IMcStatusRequestService,McStatusRequestService>();
		
		builder.Services.AddSingleton<Home>();
		builder.Services.AddSingleton<Setting>();
		builder.Services.AddSingleton<Login>();

		builder.Services.AddSingleton<IMinecraftLoginRepository, MinecraftLoginRepository>();
		builder.Services.AddSingleton<ILocalServerListRepository, LocalServerRepository>();
		builder.Services.AddSingleton<IMinecraftServerStatusRepository, MinecraftServerStatusRepository>();


		builder.ConfigureLifecycleEvents(events => {
#if MACCATALYST
			events.AddiOS(osx => {
				osx.SceneWillConnect(SceneWillConnectDelegate);
			});
#elif WINDOWS

#endif

		});
		
		return builder.Build();
	}

	private static void SceneWillConnectDelegate(UIScene scene, UISceneSession session, UISceneConnectionOptions connectionoptions) {
		if (scene is UIWindowScene windowScene) {
			if (windowScene.Titlebar != null) {
				windowScene.Titlebar.TitleVisibility = UITitlebarTitleVisibility.Hidden;
				windowScene.Titlebar.Toolbar = null;
			}
		}
	}
}
