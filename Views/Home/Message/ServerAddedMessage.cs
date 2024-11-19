using CommunityToolkit.Mvvm.Messaging.Messages;

namespace SteveLauncher.Views.Home.Message;

public class ServerAddedMessage: ValueChangedMessage<bool> {
    public ServerAddedMessage(bool value) : base(value) {
    }
}