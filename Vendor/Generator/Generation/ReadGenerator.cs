using System;
using System.Linq;
using TheLastPlanet.Events.Generator.Models;
using TheLastPlanet.Events.Generator.Problems;
using TheLastPlanet.Events.Generator.Syntax;
using Microsoft.CodeAnalysis;

namespace TheLastPlanet.Events.Generator.Generation
{
    public static class ReadGenerator
    {
        public static void Make(ISymbol member, ITypeSymbol type, CodeWriter code, string name,
            Location location, ScopeTracker scope = null)
        {
            var disposable = scope = scope == null ? code.Encapsulate() : scope.Reference();

            using (disposable)
            {
                var nullable = type.NullableAnnotation == NullableAnnotation.Annotated;

                {
                    if (nullable)
                    {
                        var underlying = GenerationEngine.GetNamedTypeSymbol(type).TypeArguments.FirstOrDefault();

                        type = underlying ?? type.WithNullableAnnotation(NullableAnnotation.None);

                        code.AppendLine("if (reader.ReadBoolean())");
                        code.Open();
                    }
                }

                if (GenerationEngine.DefaultSerialization.TryGetValue(GenerationEngine.GetQualifiedName(type),
                    out var serialization))
                {
                    serialization.Deserialize(member, type, code, name,
                        GenerationEngine.GetIdentifierWithArguments(type),
                        location);

                    return;
                }

                if (GenerationEngine.IsPrimitive(type))
                {
                    code.AppendLine(
                        $"{name} = reader.Read{(GenerationEngine.PredefinedTypes.TryGetValue(type.Name, out var result) ? result : type.Name)}();");
                }
                else
                {
                    if (type.TypeKind != TypeKind.Struct && type.TypeKind != TypeKind.Enum && !nullable)
                    {
                        code.AppendLine("if (reader.ReadBoolean())");
                        code.Open();
                    }

                    switch (type.TypeKind)
                    {
                        case TypeKind.Enum:
                            code.AppendLine(
                                $"{name} = ({GenerationEngine.GetIdentifierWithArguments(type)}) reader.ReadInt32();");

                            break;
                        case TypeKind.Interface:
                        case TypeKind.Struct:
                        case TypeKind.Class:
                            var enumerable = GenerationEngine.GetQualifiedName(type) ==
                                             GenerationEngine.EnumerableQualifiedName
                                ? GenerationEngine.GetNamedTypeSymbol(type)
                                : type.AllInterfaces.FirstOrDefault(self =>
                                    GenerationEngine.GetQualifiedName(self) ==
                                    GenerationEngine.EnumerableQualifiedName);

                            if (enumerable != null)
                            {
                                var elementType = enumerable.TypeArguments.First();

                                if (type.TypeKind == TypeKind.Interface &&
                                    GenerationEngine.GetQualifiedName(type) != GenerationEngine.EnumerableQualifiedName)
                                {
                                    var problem = new SerializationProblem
                                    {
                                        Descriptor = new DiagnosticDescriptor(ProblemId.InterfaceProperties,
                                            "Interface Properties",
                                            "Could not deserialize property '{0}' of type {1} because Interface types are not supported",
                                            "serialization",
                                            DiagnosticSeverity.Error, true),
                                        Locations = new[] { member.Locations.FirstOrDefault(), location },
                                        Format = new object[] { member.Name, type.Name }
                                    };

                                    GenerationEngine.Instance.Problems.Add(problem);

                                    code.AppendLine(
                                        $"throw new Exception(\"{string.Format(problem.Descriptor.MessageFormat.ToString(), problem.Format)}\");");

                                    return;
                                }

                                using (code.BeginScope())
                                {
                                    var prefix = GenerationEngine.GetVariableName(name);

                                    code.AppendLine($"var {prefix}Count = reader.ReadInt32();");

                                    var constructor =
                                        GenerationEngine.GetNamedTypeSymbol(type).Constructors.FirstOrDefault(
                                            self => GenerationEngine.GetQualifiedName(self.Parameters.FirstOrDefault()
                                                        ?.Type) ==
                                                    GenerationEngine.EnumerableQualifiedName);
                                    var method = GenerationEngine.HasImplementation(type, "Add",
                                        GenerationEngine.GetQualifiedName(elementType));
                                    var deconstructed = false;

                                    if (GenerationEngine.DeconstructionTypes.ContainsKey(
                                            GenerationEngine.GetQualifiedName(elementType)) &&
                                        elementType is INamedTypeSymbol named)
                                    {
                                        deconstructed = GenerationEngine.HasImplementation(type, "Add",
                                            named.TypeArguments
                                                .Select(GenerationEngine.GetNamedTypeSymbol)
                                                .Select(GenerationEngine.GetQualifiedName)
                                                .ToArray());
                                    }

                                    if (method || deconstructed)
                                    {
                                        code.AppendLine(
                                            $"{name} = new {GenerationEngine.GetIdentifierWithArguments(type)}();");
                                    }
                                    else
                                    {
                                        code.AppendLine(
                                            $"var {prefix}Temp = new {GenerationEngine.GetIdentifierWithArguments(elementType)}[{prefix}Count];");
                                    }

                                    var indexName = $"{prefix}Idx";

                                    using (code.BeginScope(
                                        $"for (var {indexName} = 0; {indexName} < {prefix}Count; {indexName}++)"))
                                    {
                                        var pointer = !method && !deconstructed;
                                        var variable = pointer
                                            ? $"{prefix}TempEntry"
                                            : $"{prefix}Transient";

                                        code.AppendLine(
                                            $"{GenerationEngine.GetIdentifierWithArguments(elementType)} {variable};");

                                        Make(member, elementType, code, variable, location, scope);

                                        if (pointer)
                                        {
                                            code.AppendLine($"{prefix}Temp[{indexName}] = {variable};");
                                        }
                                        else if (method)
                                        {
                                            code.AppendLine($"{name}.Add({prefix}Transient);");
                                        }
                                        else
                                        {
                                            var arguments = GenerationEngine
                                                .DeconstructionTypes[GenerationEngine.GetQualifiedName(elementType)]
                                                .Select(self => $"{prefix}Transient.{self}");

                                            code.AppendLine($"{name}.Add({string.Join(",", arguments)});");
                                        }
                                    }

                                    if (method || deconstructed)
                                    {
                                        return;
                                    }

                                    if (GenerationEngine.GetQualifiedName(type) ==
                                        GenerationEngine.EnumerableQualifiedName)
                                    {
                                        code.AppendLine($"{name} = {prefix}Temp;");

                                        return;
                                    }

                                    if (constructor != null)
                                    {
                                        code.AppendLine(
                                            $"{name} = new {GenerationEngine.GetIdentifierWithArguments(type)}({prefix}Temp);");

                                        return;
                                    }

                                    var problem = new SerializationProblem
                                    {
                                        Descriptor = new DiagnosticDescriptor(ProblemId.EnumerableProperties,
                                            "Enumerable Properties",
                                            "Could not deserialize property '{0}' because enumerable type {1} did not contain a suitable way of adding items",
                                            "serialization",
                                            DiagnosticSeverity.Error, true),
                                        Locations = new[] { member.Locations.FirstOrDefault(), location },
                                        Format = new object[] { member.Name, type.Name, elementType.Name }
                                    };

                                    GenerationEngine.Instance.Problems.Add(problem);

                                    code.AppendLine(
                                        $"throw new Exception(\"{string.Format(problem.Descriptor.MessageFormat.ToString(), problem.Format)}\");");
                                }
                            }
                            else
                            {
                                if (type.TypeKind == TypeKind.Interface)
                                {
                                    var problem = new SerializationProblem
                                    {
                                        Descriptor = new DiagnosticDescriptor(ProblemId.InterfaceProperties,
                                            "Interface Properties",
                                            "Could not deserialize property '{0}' of type {1} because Interface types are not supported",
                                            "serialization",
                                            DiagnosticSeverity.Error, true),
                                        Locations = new[] { member.Locations.FirstOrDefault(), location },
                                        Format = new object[] { member.Name, type.Name }
                                    };

                                    GenerationEngine.Instance.Problems.Add(problem);

                                    code.AppendLine(
                                        $"throw new Exception(\"{string.Format(problem.Descriptor.MessageFormat.ToString(), problem.Format)}\");");

                                    return;
                                }

                                if (GenerationEngine.GetNamedTypeSymbol(type).Constructors.Any(self =>
                                        self.Parameters.Length == 1 &&
                                        self.Parameters.Single().Type.MetadataName == "BinaryReader") ||
                                    GenerationEngine.HasMarkedAsSerializable(type))
                                {
                                    code.AppendLine(
                                        $"{name} = new {GenerationEngine.GetIdentifierWithArguments(type)}(reader);");
                                }
                                else
                                {
                                    var hasConstructor = false;

                                    foreach (var constructor in GenerationEngine.GetNamedTypeSymbol(type).Constructors)
                                    {
                                        hasConstructor = true;

                                        var parameters = constructor.Parameters;
                                        var members = GenerationEngine.GetMembers(type, GenerationType.Read);
                                        var index = 0;

                                        foreach (var (_, valueType) in members)
                                        {
                                            if (parameters.Length <= index)
                                            {
                                                hasConstructor = false;

                                                continue;
                                            }

                                            var parameter = parameters[index];

                                            if (parameter.Type.MetadataName != valueType.MetadataName)
                                            {
                                                hasConstructor = false;
                                            }

                                            index++;
                                        }

                                        if (hasConstructor)
                                        {
                                            break;
                                        }
                                    }

                                    if (hasConstructor)
                                    {
                                        var members = GenerationEngine.GetMembers(type, GenerationType.Read);

                                        foreach (var (deep, valueType) in GenerationEngine.GetMembers(type,
                                            GenerationType.Read))
                                        {
                                            code.AppendLine(
                                                $"{GenerationEngine.GetIdentifierWithArguments(valueType)} {GenerationEngine.GetVariableName(name + deep.Name)} = default;");
                                        }

                                        GenerationEngine.Generate($"{GenerationEngine.GetVariableName(name)}", type,
                                            code,
                                            GenerationType.Read);

                                        code.AppendLine(
                                            $"{name} = new {GenerationEngine.GetIdentifierWithArguments(type)}({string.Join(", ", members.Select(self => GenerationEngine.GetVariableName(name + self.Item1.Name)))});");
                                    }
                                    else
                                    {
                                        code.AppendLine(
                                            $"{name} = new {GenerationEngine.GetIdentifierWithArguments(type)}();");

                                        using (code.BeginScope())
                                        {
                                            GenerationEngine.Generate($"{name}.", type, code, GenerationType.Read);
                                        }
                                    }
                                }
                            }

                            break;
                        case TypeKind.Array:
                            var array = (IArrayTypeSymbol) type;

                            using (code.BeginScope())
                            {
                                var prefix = GenerationEngine.GetVariableName(name);

                                code.AppendLine($"var {prefix}Length = reader.ReadInt32();");

                                if (GenerationEngine.GetQualifiedName(array.ElementType) == "System.Byte")
                                {
                                    code.AppendLine($"{name} = reader.ReadBytes({prefix}Length);");
                                }
                                else
                                {
                                    var indexName = $"{prefix}Idx";

                                    code.AppendLine(
                                        $"{name} = new {GenerationEngine.GetIdentifierWithArguments(array.ElementType)}[{prefix}Length];");

                                    using (code.BeginScope(
                                        $"for (var {indexName} = 0; {indexName} < {prefix}Length; {indexName}++)"))
                                    {
                                        Make(member, array.ElementType, code, $"{name}[{indexName}]", location,
                                            scope);
                                    }
                                }
                            }

                            break;
                    }
                }
            }
        }
    }
}