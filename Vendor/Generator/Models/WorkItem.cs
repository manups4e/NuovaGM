using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TheLastPlanet.Generators.Models
{
    public struct WorkItem
    {
        public INamedTypeSymbol TypeSymbol;
        public SemanticModel SemanticModel;
        public ClassDeclarationSyntax ClassDeclaration;
        public NamespaceDeclarationSyntax NamespaceDeclaration;
        public CompilationUnitSyntax Unit;
    }
}