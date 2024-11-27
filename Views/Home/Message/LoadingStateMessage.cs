using CommunityToolkit.Mvvm.Messaging.Messages;
using SteveLauncher.Domain.Entity;

namespace SteveLauncher.Views.Home.Message;

public class LoadingStateMessage : ValueChangedMessage<LoadingState> {
    public LoadingStateMessage(LoadingState value) : base(value) {
        
    }
}