using SteveLauncher.Domain.Service;
using SteveLauncher.Views.Home;

namespace SteveLauncher;

public partial class App : Application
{
	public App(IServiceProvider serviceProvider)
	{
		InitializeComponent();
		CultureInfo.CurrentCulture = CultureInfo.CurrentUICulture = new CultureInfo("ko");
		MainPage = serviceProvider.GetService<Home>();

	}
}
