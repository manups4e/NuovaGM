using System.Linq;
using TheLastPlanet.Events.Generator.Generation;
using TheLastPlanet.Events.Generator.Syntax;
using Microsoft.CodeAnalysis;

namespace TheLastPlanet.Events.Generator.Serialization
{
    public abstract class BaseTupleSerialization : IDefaultSerialization
    {
        public abstract int Items { get; }

        public void Serialize(ISymbol member, ITypeSymbol type, CodeWriter code,
            string name,
            string typeIdentifier, Location location)
        {
            var named = GenerationEngine.GetNamedTypeSymbol(type);
            var types = named.TypeArguments;

            for (var idx = 0; idx < Items; idx++)
            {
                WriteGenerator.Make(member, types[idx], code, $"{name}.Item{idx + 1}", location);
            }
        }

        public void Deserialize(ISymbol member, ITypeSymbol type, CodeWriter code,
            string name,
            string typeIdentifier, Location location)
        {
            var named = GenerationEngine.GetNamedTypeSymbol(type);
            var types = named.TypeArguments;
            var prefix = GenerationEngine.GetVariableName(name);

            for (var idx = 0; idx < Items; idx++)
            {
                var item = idx + 1;
                var identifier = $"{prefix}Item{item}";

                code.AppendLine($"{GenerationEngine.GetQualifiedName(types[idx])} {identifier} = default;");
                ReadGenerator.Make(member, types[idx], code, identifier, location);
            }

            code.AppendLine(
                $"{name} = new {typeIdentifier}({string.Join(", ", Enumerable.Range(1, Items).Select(self => $"{prefix}Item{self}"))});");
        }
    }

    public class TupleSingleSerialization : BaseTupleSerialization
    {
        public override int Items => 1;
    }

    public class TupleDoubleSerialization : BaseTupleSerialization
    {
        public override int Items => 2;
    }

    public class TupleTripleSerialization : BaseTupleSerialization
    {
        public override int Items => 3;
    }

    public class TupleQuadrupleSerialization : BaseTupleSerialization
    {
        public override int Items => 4;
    }

    public class TupleQuintupleSerialization : BaseTupleSerialization
    {
        public override int Items => 5;
    }

    public class TupleSextupleSerialization : BaseTupleSerialization
    {
        public override int Items => 6;
    }

    public class TupleSeptupleSerialization : BaseTupleSerialization
    {
        public override int Items => 7;
    }
}