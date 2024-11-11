using Maui.Plugins.PageResolver.Attributes;
using McLib.Model.Network.Mc;

namespace SteveLauncher.Views.Home;

[Singleton]
public partial class MainViewModel : BaseViewModel {

    public ObservableCollection<McServerStatus> ServerStatusList = new();
    
    public MainViewModel() {
        
    }
    
}
