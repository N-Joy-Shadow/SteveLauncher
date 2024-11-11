using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SteveLauncher.Views.Setting;

namespace SteveLauncher.Views;

public partial class SettingPage : ContentPage {
    public SettingPage(SettingViewModel viewModel) {
        InitializeComponent();
        BindingContext = viewModel;
    }
}