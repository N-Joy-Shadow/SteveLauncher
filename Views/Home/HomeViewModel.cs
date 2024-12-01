using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.Messaging;
using Maui.Plugins.PageResolver.Attributes;
using McLib.Auth.Model.Minecraft;
using McLib.Model.Network.Dns;
using Microsoft.EntityFrameworkCore.Query;
using SteveLauncher.API;
using SteveLauncher.API.Enum;
using SteveLauncher.API.Exception;
using SteveLauncher.API.Repository;
using SteveLauncher.API.Service;
using SteveLauncher.Data.Database;
using SteveLauncher.Domain.Entity;
using SteveLauncher.Domain.Service;
using SteveLauncher.Utils;
using SteveLauncher.Views.Home.Message;
using SteveLauncher.Views.Login;
using SteveLauncher.Views.RegisterServer;
using SteveLauncher.Views.Setting;

namespace SteveLauncher.Views.Home;

public partial class HomeViewModel : BaseViewModel {
    #region DIContainer

    private readonly IMinecraftServerService serverService;
    private readonly IStorageRepository storageRepository;
    private readonly IPopupService popupService;
    private readonly IMinecraftGameService gameService;
    private readonly IDirectoryLaunchService directoryLaunchService;

    #endregion

    #region ObservableProperties

    [ObservableProperty]
    private RangeObservableCollection<MinecraftServerInfo> serverStatusList = new();

    [ObservableProperty]
    private bool isLoading = true;

    [ObservableProperty]
    private MinecraftServerInfo? selectedServerInfo = null;

    [ObservableProperty]
    private string selectedVersion = string.Empty;

    [ObservableProperty]
    private AuthStateEnum currentAuthState = AuthStateEnum.UnAuth;

    [ObservableProperty]
    private GameProgressStateEnum gameProgressState = GameProgressStateEnum.None;
    [ObservableProperty]
    private UserProfile? userProfile = null;

    #endregion

    #region Events

    public event Action<MinecraftServerInfo> OnServerInfoChange;
    public event Action<ToastMessage> OnShowToast;

    #endregion

    public HomeViewModel(
        IMinecraftServerService minecraftServerService,
        IStorageRepository storageRepository,
        IPopupService popupService,
        IMinecraftGameService gameService,
        IDirectoryLaunchService directoryLaunchService
    ) {
        this.serverService = minecraftServerService;
        this.storageRepository = storageRepository; //나중에 서비스로 빼야함 
        this.popupService = popupService;
        this.gameService = gameService;
        this.directoryLaunchService = directoryLaunchService;
    }


    [ObservableProperty]
    private double byteInstallProgress = 0;

    [ObservableProperty]
    private string installProgress = string.Empty;

    protected override void BindingMessageCenter() {
        WeakReferenceMessenger.Default.Register<ServerListUpdateMessage>(this, (r, m) => {
            if (m.Value)
                LoadServerStatusAsync();
        });

        WeakReferenceMessenger.Default.Register<LoadingStateMessage>(this, (r, m) => {
            //흠.. 이건 나중에 해야할 듯
            if (m.Value.IsLoading) {
            }
        });

        WeakReferenceMessenger.Default.Register<MinecraftByteProgressMessage>(this,
            (r, m) => {
                ByteInstallProgress = m.Value;
                if(m.Value == 1)
                    GameProgressState = GameProgressStateEnum.Done;
            });

        WeakReferenceMessenger.Default.Register<MinecraftInstallProgressMessage>(this,
            (r, m) => { InstallProgress = $"{m.Value.ProgressedTasks} / {m.Value.TotalTasks} - {m.Value.Name}"; });
    }

    //TOOD: 나중에 커맨드로 빼기
    public async void LoadServerStatusAsync() {
        try {
            //로직을 나중에 최적화 해야함
            var serverStatusList = await Task.Run(() => serverService.GetServerStatusListAsync());
            this.ServerStatusList.Clear();
            this.ServerStatusList.AddRange(serverStatusList);
        }
        catch (Exception e) {
            Debug.WriteLine(e);
        }
        finally {
            IsLoading = false;
        }
    }

    [RelayCommand]
    async Task GetServerInfo(MinecraftServerInfo serverInfo) {
        serverInfo.AvilableVersions = await gameService.GetAvailableVersions(serverInfo.Version);
        SelectedServerInfo = serverInfo;
        SelectedVersion = serverInfo.AvilableVersions.First();
        OnServerInfoChange.Invoke(serverInfo);
    }

    [RelayCommand]
    async Task DeleteServer(MinecraftServerInfo serverInfo) {
        var result = serverService.DeleteServer(serverInfo);
        if (result)
            LoadServerStatusAsync();

    }

    [RelayCommand]
    async Task OpenServerFolder(MinecraftServerInfo serverInfo) {
        var mpath = gameService.GetSetting().MinecraftPath;

        //임마도 따로 로직 빼기
        var path = Path.Combine(mpath, serverInfo.HostName.ToString().Replace(".", "_"));
        try {
            directoryLaunchService.Open(path);
        }
        catch (Exception e) {
            if (e is DirectoryNotFoundException)
                OnShowToast.Invoke(new() {
                    Content = "서버 폴더가 존재하지 않습니다.\n게임을 한번 실행 시켜 주세요.",
                    Title = "서버 폴더 없음",
                });
            else
                Debug.WriteLine($"{e.Message}");
        }
    }

    [RelayCommand]
    async Task ShowRegisterPopup() {
        var obj = await popupService.ShowPopupAsync<RegisterServerPopupViewModel>(CancellationToken.None);
        if (obj is bool isRegistered)
            LoadServerStatusAsync();
    }

    [RelayCommand]
    async Task ShowSettingPopup() {
        var settings = gameService.GetSetting();

        var resultSetting = await popupService.ShowPopupAsync<SettingPopupViewModel>(settingViewModel
            => settingViewModel.SetSettings(settings), CancellationToken.None);

        if (resultSetting is MinecraftGameSetting setting) {
            gameService.SetSettings(setting);
        }
    }

    //로직 분리 핋요
    //repository가 viewmodel에 접근해서 사용중
    [RelayCommand]
    async void ShowLoginPopup() {
        var res = await popupService.ShowPopupAsync<LoginViewModel>(CancellationToken.None);
        if (res is string error) {
            Debug.WriteLine(res);
        }
        //성공 할 경우
        else if (res is McUserProfile profile) {
            storageRepository.Insert(StorageEnum.USER_PROFILE, profile);
            CurrentAuthState = AuthStateEnum.Auth;
            //나중에 리팩토링 필요 
            UserProfile = new UserProfile(profile.UserName, profile.UUID);
        }
    }

    [RelayCommand]
    async void Logout() {
        UserProfile = null;
        storageRepository.Remove(StorageEnum.USER_PROFILE);
        CurrentAuthState = AuthStateEnum.UnAuth;
    }

    [RelayCommand]
    async void StartGame() {
        if (SelectedServerInfo is null) {
            OnShowToast.Invoke(new() {
                Content = "서버를 선택해주세요",
                Title = "서버 선택 필요",
            });
            return;
        }

        try {
            GameProgressState = GameProgressStateEnum.Downloading;
            await gameService.StartGame(SelectedServerInfo.HostName);
        }
        catch (Exception e) {
            if (e is MinecraftLauncherNotAuthorizedException) {
                OnShowToast.Invoke(new() {
                    Content = "게임시작전 로그인을 해주세요.",
                    Title = "로그인 필요",
                });
            }

            if (e is NullReferenceException) {
                OnShowToast.Invoke(new() {
                    Content = "버전을 선택 해주세요.",
                    Title = "버전 선택 필요",
                });
            }
        }
        MainThread.BeginInvokeOnMainThread(() => {
            Thread.Sleep(5 * 1000);
            GameProgressState = GameProgressStateEnum.None;
        });
    }

    partial void OnSelectedVersionChanged(string value) {
        gameService.SetGameVersion(value);
    }
}