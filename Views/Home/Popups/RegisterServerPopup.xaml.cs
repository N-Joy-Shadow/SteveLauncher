using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using McLib.Model.Network.Dns;
using SteveLauncher.Utils.Popups;

namespace SteveLauncher.Views.Home.Popups;

public partial class RegisterServerPopup : Popup {
    private RegisterServerPopupViewModel vm;
    public RegisterServerPopup(RegisterServerPopupViewModel viewModel,
        PopupSizeConstants  sizeConstants) {
        InitializeComponent();
        BindingContext = viewModel;
        this.vm = (RegisterServerPopupViewModel)BindingContext;
        Size = sizeConstants.Medium;
        vm.OnClosePopup += OnClosedPopup;
    }

    private async void OnClosedPopup(bool isRegistered) {
        Close();
    }
}