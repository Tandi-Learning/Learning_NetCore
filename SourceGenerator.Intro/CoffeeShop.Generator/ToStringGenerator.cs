using System;
using Microsoft.CodeAnalysis;
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
                static (node, _) => {
                    return node is ClassDeclarationSyntax;
                },
                // return the class node
                static (ctx, _) => {
                    return ctx.Node as ClassDeclarationSyntax; 
                }
             );

             context.RegisterSourceOutput(classes, 
                static (ctx, source) => Execute(ctx, source)
             );
        }

    private static void Execute(SourceProductionContext ctx, ClassDeclarationSyntax source)
    {
      var className = source.Identifier.Text;
      var fileName = $"{className}.g.cs";

      ctx.AddSource(fileName, "// Automatically Generated **");
    }
  }
}
