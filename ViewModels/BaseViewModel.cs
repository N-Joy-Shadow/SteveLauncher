namespace SteveLauncher.ViewModels;

public partial class BaseViewModel : ObservableObject
{
    public BaseViewModel() : base() {
        this.BindingMessageCenter();
    }
    
    protected virtual void BindingMessageCenter() {
        
    }
}
