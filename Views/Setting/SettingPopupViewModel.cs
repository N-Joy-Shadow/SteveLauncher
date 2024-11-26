
using CommunityToolkit.Maui.Storage;
using SteveLauncher.API.Service;

namespace SteveLauncher.Views.Setting;

public partial class SettingPopupViewModel : BaseViewModel {
    private readonly IMinecraftGameService minecraftGameService;
    
    [ObservableProperty]
    private int allocatedMemory = 2048;
    
    [ObservableProperty]
    private string minecraftPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "minecraft");
    
    [ObservableProperty]
    private int minecraftWidth = 1280;
    
    [ObservableProperty]
    private int minecraftHeight = 720;
    public SettingPopupViewModel(IMinecraftGameService minecraftGameService) {
        this.minecraftGameService = minecraftGameService;
    }
    public event Action<object?> OnClosePopup;
    [RelayCommand]
    async Task ClosePopup() {
         OnClosePopup?.Invoke(null);
    }

    [RelayCommand]
    async Task OnPropertyChange() {
        //minecraftGameService.SetSettings();
    }
    
    [RelayCommand]
    async Task ShowFileExplorer() {
        var result = await FolderPicker.Default.PickAsync(CancellationToken.None);
        if (result.IsSuccessful) {
            MinecraftPath = result.Folder.Path;
        }
    }


    partial void OnMinecraftHeightChanged(int value) {
        minecraftGameService.SetSettings();
    }
    
    partial void OnMinecraftWidthChanged(int value) {
        
    }
    
    partial void OnAllocatedMemoryChanged(int value) {
        
    }
    
    partial void OnMinecraftPathChanged(string value) {
        
    }
}