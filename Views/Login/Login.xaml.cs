using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using SteveLauncher.Utils.Popups;

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
        var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));

        if (await viewModel.LoginAsync(e.Url)) {
             await CloseAsync(viewModel.userProfile,cts.Token);
            
        }
        else { //타입 너무 대충 지은듯
            await CloseAsync("오류가 발생하였습니다.",cts.Token);
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