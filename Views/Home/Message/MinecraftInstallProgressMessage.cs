using CmlLib.Core.Installers;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace SteveLauncher.Views.Home.Message;

public class MinecraftInstallProgressMessage: ValueChangedMessage<InstallerProgressChangedEventArgs> {
    public MinecraftInstallProgressMessage(InstallerProgressChangedEventArgs value) : base(value) {
    }

}