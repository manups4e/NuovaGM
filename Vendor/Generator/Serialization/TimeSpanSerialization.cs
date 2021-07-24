using Microsoft.CodeAnalysis;
using TheLastPlanet.Generators.Syntax;

namespace TheLastPlanet.Generators.Serialization
{
    public class TimeSpanSerialization : IDefaultSerialization
    {
        public void Serialize(ISymbol member, ITypeSymbol type, CodeWriter code, string name,
            string typeIdentifier, Location location)
        {
            code.AppendLine($"writer.Write({name}.Ticks);");
        }

        public void Deserialize(ISymbol member, ITypeSymbol type, CodeWriter code, string name,
            string typeIdentifier, Location location)
        {
            code.AppendLine($"{name} = new TimeSpan(reader.ReadInt64());");
        }
    }
}