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
    public RegisterServerPopup(RegisterServerPopupViewModel viewModel) {
        InitializeComponent();
        BindingContext = viewModel;
        this.vm = (RegisterServerPopupViewModel)BindingContext;
        Size = new Size(400, 300);
        
    }
    
}