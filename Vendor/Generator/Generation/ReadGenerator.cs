using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using TheLastPlanet.Generators.Models;
using TheLastPlanet.Generators.Problems;
using TheLastPlanet.Generators.Syntax;

namespace TheLastPlanet.Generators.Generation
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

                if (nullable)
                {
                    var underlying = ((INamedTypeSymbol)type).TypeArguments.FirstOrDefault();

                    type = underlying ?? type.WithNullableAnnotation(NullableAnnotation.None);

                    code.AppendLine("if (reader.ReadBoolean())");
                    code.Open();
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
                                ? (INamedTypeSymbol)type
                                : type.AllInterfaces.FirstOrDefault(self =>
                                    GenerationEngine.GetQualifiedName(self) ==
                                    GenerationEngine.EnumerableQualifiedName);

                            if (enumerable != null)
                            {
                                var elementType = (INamedTypeSymbol)enumerable.TypeArguments.First();

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
                                    var prefix = GenerationEngine.GetCamelCase(name);

                                    code.AppendLine($"var {prefix}Count = reader.ReadInt32();");

                                    var constructor =
                                        ((INamedTypeSymbol)type).Constructors.FirstOrDefault(
                                            self => GenerationEngine.GetQualifiedName(self.Parameters.FirstOrDefault()
                                                        ?.Type) ==
                                                    GenerationEngine.EnumerableQualifiedName);

                                    var method = GenerationEngine.HasImplementation(type, "Add",
                                        GenerationEngine.GetQualifiedName(elementType));
                                    var deconstructed = false;

                                    if (GenerationEngine.DeconstructionTypes.ContainsKey(
                                        GenerationEngine.GetQualifiedName(elementType)))
                                    {
                                        deconstructed = GenerationEngine.HasImplementation(type, "Add",
                                            elementType.TypeArguments.Cast<INamedTypeSymbol>()
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
                                        var shouldBeTransient = method || deconstructed;
                                        var variable = shouldBeTransient
                                            ? $"{prefix}Transient"
                                            : $"{prefix}Temp[{indexName}]";

                                        if (shouldBeTransient)
                                            code.AppendLine(
                                                $"{GenerationEngine.GetIdentifierWithArguments(elementType)} {variable};");

                                        Make(member, elementType, code, variable, location, scope);

                                        if (method)
                                        {
                                            code.AppendLine($"{name}.Add({prefix}Transient);");
                                        }
                                        else if (deconstructed)
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

                                    if (constructor != null)
                                    {
                                        code.AppendLine(
                                            $"{name} = new {GenerationEngine.GetIdentifierWithArguments(enumerable)}({prefix}Temp);");

                                        return;
                                    }

                                    if (GenerationEngine.GetQualifiedName(type) !=
                                        GenerationEngine.EnumerableQualifiedName)
                                    {
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

                                        return;
                                    }

                                    code.AppendLine($"{name} = {prefix}Temp;");
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

                                if (GenerationEngine.HasImplementation(type, GenerationEngine.UnpackingMethod) ||
                                    GenerationEngine.HasMarkedAsSerializable(type))
                                {
                                    code.AppendLine(
                                        $"{name} = new {GenerationEngine.GetIdentifierWithArguments(type)}(reader);");
                                }
                                else
                                {
                                    var named = (INamedTypeSymbol)type;
                                    var hasConstructor = false;

                                    foreach (var constructor in named.Constructors)
                                    {
                                        hasConstructor = true;

                                        var parameters = constructor.Parameters;
                                        var members = GenerationEngine.GetMembers(type);
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
                                        var members = GenerationEngine.GetMembers(type);

                                        foreach (var (deep, valueType) in GenerationEngine.GetMembers(type))
                                        {
                                            code.AppendLine(
                                                $"{GenerationEngine.GetIdentifierWithArguments(valueType)} {GenerationEngine.GetCamelCase(name + deep.Name)} = default;");
                                        }

                                        GenerationEngine.Generate($"{GenerationEngine.GetCamelCase(name)}", type, code, GenerationType.Read);

                                        code.AppendLine(
                                            $"{name} = new {GenerationEngine.GetIdentifierWithArguments(type)}({string.Join(", ", members.Select(self => GenerationEngine.GetCamelCase(name + self.Item1.Name)))});");
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
                            var array = (IArrayTypeSymbol)type;

                            using (code.BeginScope())
                            {
                                var prefix = GenerationEngine.GetCamelCase(name);

                                code.AppendLine($"var {prefix}Length = reader.ReadInt32();");
                                code.AppendLine(
                                    $"{name} = new {GenerationEngine.GetIdentifierWithArguments(array.ElementType)}[{prefix}Length];");

                                var indexName = $"{prefix}Idx";

                                using (code.BeginScope(
                                    $"for (var {indexName} = 0; {indexName} < {prefix}Length; {indexName}++)"))
                                {
                                    Make(member, array.ElementType, code, $"{name}[{indexName}]", location,
                                        scope);
                                }
                            }

                            break;
                    }
                }
            }
        }
    }
}