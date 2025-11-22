using System.Threading.Tasks;
using Gekimini.Avalonia.Framework.Events;

namespace Gekimini.Avalonia.Models.Events;

public class ApplicationAskQuitEvent : IResponsibleMessage<Task<bool>>
{
}