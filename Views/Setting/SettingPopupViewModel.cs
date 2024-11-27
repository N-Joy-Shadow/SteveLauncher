using CommunityToolkit.Maui.Storage;
using SteveLauncher.API;
using SteveLauncher.API.Service;
using SteveLauncher.Domain.Entity;

namespace SteveLauncher.Views.Setting;

public partial class SettingPopupViewModel : BaseViewModel {
    private readonly IMinecraftGameService minecraftGameService;
    private readonly IDirectoryLaunchService directoryLauncher;

    [ObservableProperty]
    private int allocatedMemory = 2048;

    [ObservableProperty]
    private string minecraftPath =
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "minecraft");

    [ObservableProperty]
    private int minecraftWidth = 1280;

    [ObservableProperty]
    private int minecraftHeight = 720;

    public SettingPopupViewModel(IMinecraftGameService minecraftGameService
        , IDirectoryLaunchService directoryLauncher) {
        this.minecraftGameService = minecraftGameService;
        this.directoryLauncher = directoryLauncher;
    }

    public event Action<MinecraftGameSetting> OnClosePopup;

    public void SetSettings(MinecraftGameSetting setting) {
        AllocatedMemory = setting.AllocatedMemory;
        MinecraftPath = setting.MinecraftPath;
        MinecraftWidth = setting.Width;
        MinecraftHeight = setting.Height;
    }

    [RelayCommand]
    async Task ClosePopup() {
        OnClosePopup.Invoke(new MinecraftGameSetting() {
            AllocatedMemory = AllocatedMemory,
            MinecraftPath = MinecraftPath,
            Width = MinecraftWidth,
            Height = MinecraftHeight
        });
    }


    [RelayCommand]
    async Task ShowDirectory() {
        directoryLauncher.Open(MinecraftPath);
    }

    [RelayCommand]
    async Task ChangeDirectory() {
        var result = await FolderPicker.Default.PickAsync(CancellationToken.None);
        if (result.IsSuccessful && result.Folder is not null) {
            MinecraftPath = result.Folder.Path;
        }
    }
}