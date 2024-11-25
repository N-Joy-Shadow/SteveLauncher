
namespace SteveLauncher.Views.Home.Popups;

public partial class SettingPopupViewModel : BaseViewModel {
    
    public SettingPopupViewModel() {
        
    }
    public event Action<object?> OnClosePopup;
    [RelayCommand]
    async Task ClosePopup() {
         OnClosePopup?.Invoke(null);
    }
}