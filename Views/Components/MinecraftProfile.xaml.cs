using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CmlLib.Core;
using SteveLauncher.Views.Home;

namespace SteveLauncher.Views.Components;

public partial class MinecraftProfile : ContentView {
    
    public MinecraftProfile() {
        InitializeComponent();
        this.PropertyChanged += OnPropertyChanged;
        
    }

    private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e) {
        //TODO: 나중에 보수
        if (BindingContext is HomeViewModel viewModel)
            if (viewModel.UserProfile is not null)
                UserIconImage.Source = ImageSource.FromUri(new Uri(viewModel.UserProfile.UserIcon));
            else
                UserIconImage.Source = null;
    }
}