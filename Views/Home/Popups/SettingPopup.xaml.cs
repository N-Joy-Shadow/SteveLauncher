using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Views;
using SteveLauncher.Utils.Popups;

namespace SteveLauncher.Views.Home.Popups;

public partial class SettingPopup : Popup {
    private SettingPopupViewModel vm;
    public SettingPopup(
        PopupSizeConstants size,
        SettingPopupViewModel viewModel) {
        InitializeComponent();
        BindingContext = viewModel;
        this.vm = (SettingPopupViewModel)BindingContext;
        Size = size.Medium;
        vm.OnClosePopup += OnClosePopup;
    }

    private async void OnClosePopup(object? obj) {
        await CloseAsync(obj);
    }
}