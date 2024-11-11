using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteveLauncher.Views.Home;

public partial class Home : ContentPage {
    public Home(MainViewModel viewModel) {
        InitializeComponent();
        BindingContext = viewModel;
    }
}