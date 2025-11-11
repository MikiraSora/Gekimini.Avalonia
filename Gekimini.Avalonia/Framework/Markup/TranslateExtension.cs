using System.Resources;
using Gekimini.Avalonia.Assets.Languages;

namespace Gekimini.Avalonia.Framework.Markup;

public class TranslateExtension : TranslateExtensionBase
{
    public TranslateExtension(string member) : base(member, Resources.ResourceManager.GetString)
    {
    }
}