using System;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CoffeeShop.Generator
{
    [Generator]
    public class ToStringGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var classes = context.SyntaxProvider.CreateSyntaxProvider(
            // look for the class in the file syntax tree
            predicate: static (node, _) => IsSyntaxTarget(node)
,
            // return the class node
            transform: static (ctx, _) => GetSemanticTarget(ctx)
            );

            context.RegisterSourceOutput(classes,
                static (ctx, source) => Execute(ctx, source));

            context.RegisterPostInitializationOutput(
                static (ctx) => PostInitializationOutput(ctx));
        }

        private static ClassDeclarationSyntax GetSemanticTarget(GeneratorSyntaxContext ctx)
        {
            var classDeclarationSyntax  = ctx.Node as ClassDeclarationSyntax;
            foreach(var attributeList in classDeclarationSyntax.AttributeLists)
            {
                foreach(var attributeSyntax in attributeList.Attributes) 
                {
                    var attributeName = attributeSyntax.Name.ToString();
                    if (attributeName == "GenerateToString" || attributeName == "GenerateToStringAttribute")
                    {
                        return classDeclarationSyntax;
                    }
                }
            }
            return null;
        }

        private static bool IsSyntaxTarget(SyntaxNode node)
        {
            return node is ClassDeclarationSyntax syntax && syntax.AttributeLists.Count > 0;
        }

        private static void PostInitializationOutput(IncrementalGeneratorPostInitializationContext ctx)
        {
            ctx.AddSource("CoffeeShop.Generator.GenerateToStringAttribute.g.cs",
@"namespace CoffeeShop.Generator
{
    internal class GenerateToStringAttribute : System.Attribute { }
}");
        }

        private static void Execute(SourceProductionContext ctx, ClassDeclarationSyntax syntax)
        {
            if (syntax.Parent is BaseNamespaceDeclarationSyntax nsDeclarationSyntax)
            {
                var namespaceName = nsDeclarationSyntax.Name.ToString();
                var className = syntax.Identifier.Text;
                var fileName = $"{ namespaceName}.{className}.g.cs";                

                var sb = new StringBuilder();
                sb.Append($@"namespace {namespaceName}
{{ 
    partial class {className} 
    {{
        public override string ToString()
        {{
            return $""");
                foreach(var member in syntax.Members)
                {
                    if (member is PropertyDeclarationSyntax property && property.Modifiers.Any(SyntaxKind.PublicKeyword))
                    {
                        var name = property.Identifier.Text;
                        sb.Append($"{name}:{{{name}}} ");
                    }
                }
                sb.Append($@""";  
        }}
    }}
}}
");
                ctx.AddSource(fileName, sb.ToString());
            }
        }
    }
}
