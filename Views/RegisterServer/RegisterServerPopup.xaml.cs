using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using McLib.Model.Network;
using SteveLauncher.Domain.Entity;
using SteveLauncher.Utils.Popups;

namespace SteveLauncher.Views.RegisterServer;

public partial class RegisterServerPopup : Popup {
    private RegisterServerPopupViewModel vm;
    public RegisterServerPopup(RegisterServerPopupViewModel viewModel,
        PopupSizeConstants size) {
        InitializeComponent();
        BindingContext = viewModel;
        this.vm = (RegisterServerPopupViewModel)BindingContext;
        Size = size.Medium;
        vm.OnClosePopup += OnClosedPopup;
    }

    private async void OnClosedPopup(MinecraftServerInfo? info) {
        Close(info);
    }
}