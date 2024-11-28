using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using SteveLauncher.Domain.Entity;
using SteveLauncher.Extension;
namespace SteveLauncher.Views.Home.Cell;

public partial class ServerListCell : ContentView {
    public static readonly BindableProperty TappedCommandProperty =
        BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(ServerListCell));
    
    public static readonly BindableProperty ContextMenuCommandProperty =
        BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(ServerListCell));

    
    public ICommand TappedCommand
    {
        get => (ICommand)GetValue(TappedCommandProperty);
        set => SetValue(TappedCommandProperty, value);
    }
    public ICommand ContextMenuCommand
    {
        get => (ICommand)GetValue(ContextMenuCommandProperty);
        set => SetValue(ContextMenuCommandProperty, value);
    }

    private MinecraftServerInfo serverInfo; 
    
    public ServerListCell() {
        InitializeComponent();
        
        //별로 마음에 안드는 코드   
        BindingContextChanged += OnBindingContextChanged;

    }

    private void OnBindingContextChanged(object? sender, EventArgs e) {
        if(BindingContext is MinecraftServerInfo serverInfo) {
            this.serverInfo = serverInfo;
            //Icon.Source = ImageSourceExtension.FromBase64(serverInfo.Icon);

        }
    }


    private void OnTapped(object? sender, TappedEventArgs e) {
        if(IsEnabled)
            TappedCommand.Execute(serverInfo);
    }

    private void OnContextMenuClicked(object? sender, EventArgs e) {
        if(IsEnabled)
            ContextMenuCommand.Execute(serverInfo);
    }
}