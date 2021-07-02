using Microsoft.CodeAnalysis;
using TheLastPlanet.Generators.Syntax;

namespace TheLastPlanet.Generators.Serialization
{
    public interface IDefaultSerialization
    {
        void Serialize(SerializationEngine engine, IPropertySymbol property, ITypeSymbol type, CodeWriter code, string name, string typeIdentifier, Location location);
        void Deserialize(SerializationEngine engine, IPropertySymbol property, ITypeSymbol type, CodeWriter code, string name, string typeIdentifier, Location
             location);
    }
}