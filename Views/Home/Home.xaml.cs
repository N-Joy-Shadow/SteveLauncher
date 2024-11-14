using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }

    private void OnOnServerInfoChange(MinecraftServerInfo serverinfo) {
        
    }


    private void Home_OnLoaded(object? sender, EventArgs e) {
        vm.LoadServerStatusAsync();
    }
    
    
}