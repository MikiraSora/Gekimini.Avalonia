using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Gekimini.Avalonia.Models.Events;

public class ApplicationAskQuitEvent : AsyncCollectionRequestMessage<bool>
{
}