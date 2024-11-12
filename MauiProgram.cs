using McLib.API.Services;
using McLib.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Maui.LifecycleEvents;
using SteveLauncher.Data.Database;
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

		builder.Services.AddDbContext<SteveDbContext>(options => {
			options.UseSqlite($"Data Source={Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "steveLauncher.db")}");
		});

		builder.Services.AddScoped<IDnsCheckService,DnsCheckService>();
		builder.Services.AddScoped<IMcStatusRequestService,McStatusRequestService>();
		
		return builder.Build();
	}
}
