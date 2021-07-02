using Microsoft.CodeAnalysis;
using TheLastPlanet.Generators.Syntax;

namespace TheLastPlanet.Generators.Serialization
{
    public class KeyValuePairSerialization : IDefaultSerialization
    {
        public void Serialize(SerializationEngine engine, IPropertySymbol property, ITypeSymbol type, CodeWriter code,
            string name, string typeIdentifier, Location location)
        {
            var named = (INamedTypeSymbol) type;
            var types = named.TypeArguments;

            engine.AppendWriteLogic(property, types[0], code, $"{name}.Key", location);
            engine.AppendWriteLogic(property, types[1], code, $"{name}.Value", location);
        }

        public void Deserialize(SerializationEngine engine, IPropertySymbol property, ITypeSymbol type, CodeWriter code,
            string name, string typeIdentifier, Location location)
        {
            var named = (INamedTypeSymbol) type;
            var types = named.TypeArguments;
            var prefix = SerializationEngine.GetVariablePrefix(name);

            code.AppendLine($"{SerializationEngine.GetQualifiedName(types[0])} {prefix}Key = default;");
            code.AppendLine($"{SerializationEngine.GetQualifiedName(types[1])} {prefix}Value = default;");

            engine.AppendReadLogic(property, types[0], code, $"{prefix}Key", location);
            engine.AppendReadLogic(property, types[1], code, $"{prefix}Value", location);

            code.AppendLine($"{name} = new {typeIdentifier}({prefix}Key, {prefix}Value);");
        }
    }
}