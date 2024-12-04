using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Alerts;
using Microsoft.Maui.Controls.Shapes;
using SteveLauncher.Domain.Entity;

namespace SteveLauncher.Views.Home;

public partial class Home : ContentPage {
    private readonly HomeViewModel vm;
    public Home(HomeViewModel viewModel) {
        InitializeComponent();
        BindingContext = viewModel;
        this.vm = (HomeViewModel)BindingContext;
        
        vm.OnServerInfoChange += OnOnServerInfoChange;
        vm.OnShowToast += OnOnShowToast;
    }

    private async void OnOnShowToast(ToastMessage obj) {
        await DisplayAlert(obj.Title,obj.Content,"Ok");
    }

    private void OnOnServerInfoChange(MinecraftServerInfo serverinfo) {
        
    }


    private async void Home_OnLoaded(object? sender, EventArgs e) {
        await vm.GetVersion();
        vm.LoadServerStatusAsync();
    }
    
    
}