using Microsoft.CodeAnalysis;
using TheLastPlanet.Generators.Generation;
using TheLastPlanet.Generators.Syntax;

namespace TheLastPlanet.Generators.Serialization
{
    public class KeyValuePairSerialization : IDefaultSerialization
    {
        public void Serialize(ISymbol member, ITypeSymbol type, CodeWriter code,
            string name, string typeIdentifier, Location location)
        {
            var named = (INamedTypeSymbol) type;
            var types = named.TypeArguments;

            WriteGenerator.Make(member, types[0], code, $"{name}.Key", location);
            WriteGenerator.Make(member, types[1], code, $"{name}.Value", location);
        }

        public void Deserialize(ISymbol member, ITypeSymbol type, CodeWriter code,
            string name, string typeIdentifier, Location location)
        {
            var named = (INamedTypeSymbol) type;
            var types = named.TypeArguments;
            var prefix = GenerationEngine.GetCamelCase(name);

            code.AppendLine($"{GenerationEngine.GetQualifiedName(types[0])} {prefix}Key = default;");
            code.AppendLine($"{GenerationEngine.GetQualifiedName(types[1])} {prefix}Value = default;");

            ReadGenerator.Make(member, types[0], code, $"{prefix}Key", location);
            ReadGenerator.Make(member, types[1], code, $"{prefix}Value", location);

            code.AppendLine($"{name} = new {typeIdentifier}({prefix}Key, {prefix}Value);");
        }
    }
}