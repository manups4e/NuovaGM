using TheLastPlanet.Events.Generator.Syntax;
using Microsoft.CodeAnalysis;

namespace TheLastPlanet.Events.Generator.Serialization
{
    public class DateTimeSerialization : IDefaultSerialization
    {
        public void Serialize(ISymbol member, ITypeSymbol type, CodeWriter code, string name,
            string typeIdentifier, Location location)
        {
            code.AppendLine($"writer.Write({name}.Ticks);");
        }

        public void Deserialize(ISymbol member, ITypeSymbol type, CodeWriter code, string name,
            string typeIdentifier, Location location)
        {
            code.AppendLine($"{name} = new DateTime(reader.ReadInt64());");
        }
    }
}