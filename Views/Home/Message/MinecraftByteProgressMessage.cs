using CommunityToolkit.Mvvm.Messaging.Messages;

namespace SteveLauncher.Views.Home.Message;

public class MinecraftByteProgressMessage: ValueChangedMessage<double> {
    public MinecraftByteProgressMessage(double value) : base(value) {
    }
}