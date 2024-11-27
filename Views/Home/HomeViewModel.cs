﻿using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.Messaging;
using Maui.Plugins.PageResolver.Attributes;
using McLib.Auth.Model.Minecraft;
using McLib.Model.Network.Dns;
using Microsoft.EntityFrameworkCore.Query;
using SteveLauncher.API.Enum;
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
    private readonly ISecureStorageRepository secureStorageRepository;
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

    public HomeViewModel(
        IMinecraftServerService minecraftServerService,
        ISecureStorageRepository secureStorageRepository,
        IPopupService popupService,
        IMinecraftGameService gameService
    ) {
        this.serverService = minecraftServerService;
        this.secureStorageRepository = secureStorageRepository; //나중에 서비스로 빼야함 
        this.popupService = popupService;
        this.gameService = gameService;
    }

    
    
    protected override void BindingMessageCenter() {
        WeakReferenceMessenger.Default.Register<ServerAddedMessage>(this, (r, m) => {
            if (m.Value)
                LoadServerStatusAsync();
        });
        
        WeakReferenceMessenger.Default.Register<LoadingStateMessage>(this, (r, m) => {
            if (m.Value.IsLoading) {
                
            }
        });
    }

    //TOOD: 나중에 커맨드로 빼기
    public async void LoadServerStatusAsync() {
        try {
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
        var settings = await gameService.GetSetting();
        
        var resultSetting = await popupService.ShowPopupAsync<SettingPopupViewModel>((settingViewModel) => {
            if (settings is not null) {
                settingViewModel.MinecraftHeight = settings.Height;
                settingViewModel.MinecraftWidth = settings.Width;
                settingViewModel.AllocatedMemory = settings.AllocatedMemory;
                settingViewModel.MinecraftPath = settings.MinecraftPath;
            }
        },CancellationToken.None);
        
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
            secureStorageRepository.InsertAsync(SecureStorageEnum.USER_PROFILE, profile);
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
        if (SelectedServerInfo is not null) {
            await gameService.StartGame(SelectedServerInfo.HostName);
        }
    }
}