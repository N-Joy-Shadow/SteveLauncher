using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using SteveLauncher.Utils.Popups;

#if WINDOWS
using Microsoft.UI.Xaml.Controls;
#endif

namespace SteveLauncher.Views.Login;

public partial class Login : Popup {
    private LoginViewModel viewModel;

    public Login(LoginViewModel viewModel, PopupSizeConstants size) {
        InitializeComponent();
        BindingContext = viewModel;
        this.viewModel = (LoginViewModel)BindingContext;
        this.Size = size.Medium;
        Opened += OnOpened;

    }



    private async void WebView_OnNavigated(object? sender, WebNavigatedEventArgs e) {
        if (e.Url.Contains("code=")) {
            if (await viewModel.LoginAsync(e.Url)) {
                await CloseAsync(viewModel.userProfile, CancellationToken.None);
            }
            else {
                //타입 너무 대충 지은듯
                await CloseAsync("오류가 발생하였습니다.", CancellationToken.None);
            }
        }

        //Close("오류가 발생하였습니다.");
    }

    //흠...
    private void OnOpened(object? sender, PopupOpenedEventArgs e) {
        if (webView.IsLoaded)
            webView.Source = viewModel.GetAuthUrl();
    }

    //좀 애매 한데
    private void WebView_OnLoaded(object? sender, EventArgs e) {
        webView.Source = viewModel.GetAuthUrl();
    }
}