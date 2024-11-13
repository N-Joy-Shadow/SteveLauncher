using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using SteveLauncher.Domain.Entity;

namespace SteveLauncher.Views.Home.Cell;

public partial class ServerListCell : ViewCell {
    public static readonly BindableProperty DeleteCommandProperty =
        BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(ServerListCell));

    public static readonly BindableProperty TappedCommandProperty =
        BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(ServerListCell));

    public static readonly BindableProperty CommandParameterProperty  =
        BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(ServerListCell));

    
    public ICommand DeleteCommand
    {
        get => (ICommand)GetValue(DeleteCommandProperty);
        set => SetValue(DeleteCommandProperty, value);
    }

    
    public ICommand TappedCommand
    {
        get => (ICommand)GetValue(TappedCommandProperty);
        set => SetValue(TappedCommandProperty, value);
    }
    
    public object CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }


    
    public ServerListCell() {
        InitializeComponent();
        Icon.Source = ImageSource.FromStream(() => new MemoryStream(Convert.FromBase64String(((MinecraftServerInfo)BindingContext).Icon.Split(",")[1])));
        
        ContextMenu.Clicked += ContextMenuOnClicked;
        
        var tapGestureRecognizer = new TapGestureRecognizer();
        tapGestureRecognizer.Tapped += OnTapped;
        CellItem.GestureRecognizers.Add(tapGestureRecognizer);    }

    private void OnTapped(object? sender, TappedEventArgs e) {
        TappedCommand.Execute(CommandParameter);
    }

    private void ContextMenuOnClicked(object? sender, EventArgs e) {
        DeleteCommand.Execute(CommandParameter);
    }
}