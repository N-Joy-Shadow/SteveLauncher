using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Views;
using SteveLauncher.Utils.Popups;

namespace SteveLauncher.Views.Home.Popups;

public partial class RegisterServerPopup : Popup {
    private RegisterServerPopupViewModel vm;
    public RegisterServerPopup(RegisterServerPopupViewModel vm) {
        InitializeComponent();
        BindingContext = vm;
        this.vm = (RegisterServerPopupViewModel)BindingContext;
        Size = new Size(400, 300);
        
        this.vm.RecievedAlert += OnRecievedAlert;
    }

    private async void OnRecievedAlert(string obj) {
        
    }
}