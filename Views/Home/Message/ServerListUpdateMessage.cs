using CommunityToolkit.Mvvm.Messaging.Messages;

namespace SteveLauncher.Views.Home.Message;

public class ServerListUpdateMessage: ValueChangedMessage<bool> {
    public ServerListUpdateMessage(bool value) : base(value) {
    }
}