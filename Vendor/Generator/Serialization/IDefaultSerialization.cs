using TheLastPlanet.Events.Generator.Syntax;
using Microsoft.CodeAnalysis;

namespace TheLastPlanet.Events.Generator.Serialization
{
    public interface IDefaultSerialization
    {
        void Serialize(ISymbol member, ITypeSymbol type, CodeWriter code, string name, string typeIdentifier, Location location);
        void Deserialize(ISymbol member, ITypeSymbol type, CodeWriter code, string name, string typeIdentifier, Location
             location);
    }
}