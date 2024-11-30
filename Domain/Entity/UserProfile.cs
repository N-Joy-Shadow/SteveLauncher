namespace SteveLauncher.Domain.Entity;

public partial  class UserProfile: ObservableObject {
    private string username;
    public UserProfile(string username, string uuid) {
        this.UserName = username;
        this.UUID = uuid;
    }

    public string UserName { get; set; }

    public string UUID { get; set; }

    public string UserIcon => $"https://cravatar.eu/helmhead/{UserName}/600.png";

}