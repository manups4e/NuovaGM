using Microsoft.CodeAnalysis;
using TheLastPlanet.Generators.Syntax;

namespace TheLastPlanet.Generators.Serialization
{
    public interface IDefaultSerialization
    {
        void Serialize(ISymbol member, ITypeSymbol type, CodeWriter code, string name, string typeIdentifier, Location location);
        void Deserialize(ISymbol member, ITypeSymbol type, CodeWriter code, string name, string typeIdentifier, Location location);
    }
}