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
using SteveLauncher.Views.Home.Popups;
using SteveLauncher.Views.Login;
using SteveLauncher.Views.Setting;

namespace SteveLauncher.Views.Home;

public partial class HomeViewModel : BaseViewModel {
    private readonly IMinecraftServerService serverService;
    private readonly IStorageRepository _storageRepository;
    private readonly IPopupService popupService;
    private readonly IMinecraftGameService gameService;
    [ObservableProperty]
    private RangeObservableCollection<MinecraftServerInfo> serverStatusList = new();

    //바인딩 안해도 됨
    [ObservableProperty]
    private bool isLoading = true;

    [ObservableProperty]
    private MinecraftServerInfo? selectedServerInfo = null;

    //나중에 Enum으로 바꾸기
    [ObservableProperty]
    private AuthStateEnum currentAuthState = AuthStateEnum.UnAuth;

    [ObservableProperty]
    private UserProfile? userProfile = null;

    public event Action<MinecraftServerInfo> OnServerInfoChange;

    public event Action<ToastMessage> OnShowToast;
    public HomeViewModel(
        IMinecraftServerService minecraftServerService,
        IStorageRepository storageRepository,
        IPopupService popupService,
        IMinecraftGameService gameService
    ) {
        this.serverService = minecraftServerService;
        this._storageRepository = storageRepository; //나중에 서비스로 빼야함 
        this.popupService = popupService;
        this.gameService = gameService;
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
        
        WeakReferenceMessenger.Default.Register<MinecraftByteProgressMessage>(this, (r, m) => {
            ByteInstallProgress = m.Value;
        });
        
        WeakReferenceMessenger.Default.Register<MinecraftInstallProgressMessage>(this, (r, m) => {
            InstallProgress = $"{m.Value.ProgressedTasks} / {m.Value.TotalTasks} - {m.Value.Name}";
        });
    }

    //TOOD: 나중에 커맨드로 빼기
    public async void LoadServerStatusAsync() {
        try {
            //로직을 나중에 최적화 해야함
            var serverStatusList = await Task.Run(() => serverService.GetServerStatusList());
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
        SelectedServerInfo = serverInfo;
        OnServerInfoChange.Invoke(serverInfo);
    }

    [RelayCommand]
    async Task DeleteServer(MinecraftServerInfo serverInfo) {
        var result = serverService.DeleteServer(serverInfo);
        if (result)
            LoadServerStatusAsync();
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
            => settingViewModel.SetSettings(settings),CancellationToken.None);
        
        if(resultSetting is MinecraftGameSetting setting) {
            gameService.SetSettings(setting);
        }
    }

    [RelayCommand]
    async void ShowLoginPopup() {
        var res = await popupService.ShowPopupAsync<LoginViewModel>(CancellationToken.None);
        if (res is string error) {
            Debug.WriteLine(res);
        }
        //성공 할 경우
        else if (res is McUserProfile profile) {
            _storageRepository.Insert(StorageEnum.USER_PROFILE, profile);
            CurrentAuthState = AuthStateEnum.Auth;
            //나중에 리팩토링 필요 
            UserProfile = new UserProfile(profile.UserName, profile.UUID);
        }
    }

    [RelayCommand]
    async void Logout() {
        UserProfile = null;
        CurrentAuthState = AuthStateEnum.UnAuth;
    }
    
    [RelayCommand]
    async void StartGame() {
        if (SelectedServerInfo is null) {
            OnShowToast.Invoke(new () {
                Content = "서버를 선택해주세요",
                Title = "서버 선택 필요",
            });
            return;
        }

        try {
            await gameService.StartGame(SelectedServerInfo.HostName);
        }
        catch (Exception e) {
            if (e is MinecraftLauncherNotAuthorizedException) {
                OnShowToast.Invoke(new () {
                    Content = "게임시작전 로그인을 해주세요.",
                    Title = "로그인 필요",
                });
            }
        }
    }
}