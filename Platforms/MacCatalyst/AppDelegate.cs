using Foundation;
using UIKit;

namespace SteveLauncher;

[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate
{
	protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
	
	private void SetMacCatalystTitlebarOptions()
	{
            if (UIApplication.SharedApplication.Windows.FirstOrDefault()?.WindowScene is UIWindowScene windowScene)
            {
                if (windowScene.Titlebar != null)
                {
                    windowScene.Titlebar.TitleVisibility = UITitlebarTitleVisibility.Hidden;
                    windowScene.Titlebar.Toolbar = null;
                }
            }
	}
}
