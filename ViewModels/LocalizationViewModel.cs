namespace SteveLauncher.ViewModels;

public partial class LocalizationViewModel : BaseViewModel
{
	public string LocalizedText => SteveLauncher.Resources.Strings.AppResources.HelloMessage;
}
