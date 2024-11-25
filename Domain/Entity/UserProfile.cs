namespace SteveLauncher.Domain.Entity;

public partial  class UserProfile: ObservableObject {
    private string username;
    public UserProfile(string username, string uuid) {
        this.UserName = username;
        this.UUID = uuid;
        this.UserIcon = $"https://cravatar.eu/helmhead/{UserName}/600.png";
    }

    public string UserName {
        get {
            return username;
        }
        set {
            username = value;
            UserIcon = $"https://cravatar.eu/helmhead/{value}/600.png";
            Debug.WriteLine($"UserIcon: {UserIcon}");
            OnPropertyChanged(nameof(UserName));
        }
    }

    public string UUID { get; set; }

    public string UserIcon { get; private set; }
  
}