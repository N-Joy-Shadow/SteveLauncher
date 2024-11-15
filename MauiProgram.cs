﻿using McLib.API.Services;
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
using SteveLauncher.Views.Setting;
using UraniumUI;

#if MACCATALYST
using UIKit;
#endif

#if WINDOWS
    using Microsoft.UI;
    using Microsoft.UI.Windowing;
    using Windows.Graphics;
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

		//external services
		builder.Services.AddSingleton<IDnsCheckService,DnsCheckService>();
		builder.Services.AddSingleton<IMcStatusRequestService,McStatusRequestService>();
		
		//Views
		builder.Services.AddSingleton<Home>();
		
		
		//Popup
		builder.Services.AddTransientPopup<RegisterServerPopup,RegisterServerPopupViewModel>();
		builder.Services.AddTransientPopup<Setting,SettingViewModel>();
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
				osx.SceneWillConnect(SceneWillConnectDelegate);
			});
		});
#elif WINDOWS
		builder.ConfigureLifecycleEvents(events => {
			events.AddWindows(windowEvent => {
				windowEvent.OnWindowCreated(window => {
					window.ExtendsContentIntoTitleBar = true;
					window.Title = "SteveLauncher";
					window.AppWindow.TitleBar.ExtendsContentIntoTitleBar = true;
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
