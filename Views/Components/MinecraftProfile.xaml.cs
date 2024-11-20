using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CmlLib.Core;

namespace SteveLauncher.Views.Components;

public partial class MinecraftProfile : ContentView {
    
    public static readonly BindableProperty SkinUrlProperty = BindableProperty.Create(
        nameof(SkinUrl), typeof(string), typeof(MinecraftProfile), default(string));

    public string SkinUrl {
        get => (string)GetValue(SkinUrlProperty);
        set => SetValue(SkinUrlProperty, value);
    }

    public static readonly BindableProperty PlayerNameProperty = BindableProperty.Create(
        nameof(PlayerName), typeof(string), typeof(MinecraftProfile), default(string));

    public string PlayerName {
        get => (string)GetValue(PlayerNameProperty);
        set => SetValue(PlayerNameProperty, value);
    }
    public MinecraftProfile() {
        InitializeComponent();
        
    }
}