using SteveLauncher.Views.Home;

namespace SteveLauncher;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		// Uncomment the following as a quick way to test loading resources for different languages
		// CultureInfo.CurrentCulture = CultureInfo.CurrentUICulture = new CultureInfo("es");
		CultureInfo.CurrentCulture = CultureInfo.CurrentUICulture = new CultureInfo("ko");
		MainPage = new Home(new MainViewModel());
	}
}
