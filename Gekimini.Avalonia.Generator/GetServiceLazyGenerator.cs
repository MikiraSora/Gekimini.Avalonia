using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Gekimini.Avalonia.Generator;

[Generator]
public class GetServiceLazyGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var props = context.SyntaxProvider
            .CreateSyntaxProvider(
                static (n, _) => n is PropertyDeclarationSyntax {AttributeLists.Count: > 0},
                static (ctx, _) =>
                {
                    var prop = (PropertyDeclarationSyntax) ctx.Node;
                    var model = ctx.SemanticModel;
                    var symbol = model.GetDeclaredSymbol(prop) as IPropertySymbol;
                    if (symbol == null)
                        return null;

                    // 找到 GetServiceLazyAttribute
                    foreach (var attr in symbol.GetAttributes())
                        if (attr.AttributeClass?.Name == "GetServiceLazyAttribute")
                            return new PropertyInjectInfo(symbol.ContainingType, symbol);

                    return null;
                })
            .Where(x => x != null);

        context.RegisterSourceOutput(props, (spc, info) =>
        {
            var classSymbol = info.ClassSymbol;
            var propSymbol = info.PropertySymbol;

            var className = classSymbol.Name;
            var ns = classSymbol.ContainingNamespace.ToDisplayString();
            var propName = propSymbol.Name;
            var propType = propSymbol.Type.ToDisplayString();
            var propAccess = propSymbol.DeclaredAccessibility switch
            {
                Accessibility.Public => "public",
                Accessibility.Internal => "internal",
                Accessibility.Protected => "protected",
                _ => "private"
            };
            var classAccess = classSymbol.DeclaredAccessibility switch
            {
                Accessibility.Public => "public",
                Accessibility.Internal => "internal",
                Accessibility.Protected => "protected",
                _ => "private"
            };

            spc.AddSource($"{className}_{propName}_GetServiceLazy.g.cs", $@"
            using Microsoft.Extensions.DependencyInjection;

            namespace {ns}
            {{
                {classAccess} partial class {className}
                {{
                    {propAccess} partial {propType} {propName} 
                        => field ??= (App.Current as App)?.ServiceProvider.GetService<{propType}>();
                }}
            }}
            ");
        });
    }
}

public class PropertyInjectInfo(INamedTypeSymbol ClassSymbol, IPropertySymbol PropertySymbol)
{
    public INamedTypeSymbol ClassSymbol { get; } = ClassSymbol;
    public IPropertySymbol PropertySymbol { get; } = PropertySymbol;
}