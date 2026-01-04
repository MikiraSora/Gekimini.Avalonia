using System.Globalization;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Gekimini.Avalonia.Models.Events;

public class CurrentCultureInfoChangedEvent(CultureInfo value) : ValueChangedMessage<CultureInfo>(value);