using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Views;

namespace SteveLauncher.Views.Login;

public partial class Login : Popup {
    private LoginViewModel viewModel;
    public Login(LoginViewModel viewModel) {
        InitializeComponent();
        BindingContext = viewModel;
        this.viewModel = (LoginViewModel)BindingContext;
    }

    private void WebView_OnNavigated(object? sender, WebNavigatedEventArgs e) {
        throw new NotImplementedException();
    }
}