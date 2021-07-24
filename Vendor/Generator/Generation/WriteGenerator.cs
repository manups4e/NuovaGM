using System.Linq;
using Microsoft.CodeAnalysis;
using TheLastPlanet.Generators.Models;
using TheLastPlanet.Generators.Problems;
using TheLastPlanet.Generators.Syntax;

namespace TheLastPlanet.Generators.Generation
{
    public static class WriteGenerator
    {
        public static void Make(ISymbol member, ITypeSymbol type, CodeWriter code, string name,
            Location location, ScopeTracker scope = null)
        {
            var disposable = scope = scope == null ? code.Encapsulate() : scope.Reference();

            using (disposable)
            {
                var nullable = type.NullableAnnotation == NullableAnnotation.Annotated;
                var hasUnderlying = false;

                if (nullable)
                {
                    var underlying = ((INamedTypeSymbol)type).TypeArguments.FirstOrDefault();

                    hasUnderlying = underlying != null;
                    type = underlying ?? type.WithNullableAnnotation(NullableAnnotation.None);

                    var check = hasUnderlying ? ".HasValue" : " is not null";

                    code.AppendLine($"writer.Write({name}{check});");
                    code.AppendLine($"if ({name}{check})");
                    code.Open();
                }

                name = nullable && hasUnderlying ? $"{name}.Value" : name;

                if (GenerationEngine.DefaultSerialization.TryGetValue(GenerationEngine.GetQualifiedName(type), out var serialization))
                {
                    serialization.Serialize(member, type, code, name, GenerationEngine.GetIdentifierWithArguments(type),
                        location);

                    return;
                }

                if (GenerationEngine.IsPrimitive(type))
                {
                    if (!type.IsValueType)
                    {
                        using (code.BeginScope($"if ({name} is default({GenerationEngine.GetIdentifierWithArguments(type)}))"))
                        {
                            code.AppendLine(
                                $"throw new Exception(\"Member '{name}' is a primitive and has no value (null). If this is not an issue, please declare it as nullable.\");");
                        }
                    }

                    code.AppendLine($"writer.Write({name});");
                }
                else
                {
                    if (type.TypeKind != TypeKind.Struct && type.TypeKind != TypeKind.Enum && !nullable)
                    {
                        code.AppendLine($"writer.Write({name} is not null);");
                        code.AppendLine($"if ({name} is not null)");
                        code.Open();
                    }

                    switch (type.TypeKind)
                    {
                        case TypeKind.Enum:
                            code.AppendLine($"writer.Write((int) {name});");

                            break;
                        case TypeKind.Interface:
                        case TypeKind.Struct:
                        case TypeKind.Class:
                            var enumerable = GenerationEngine.GetQualifiedName(type) == GenerationEngine.EnumerableQualifiedName
                                ? (INamedTypeSymbol)type
                                : type.AllInterfaces.FirstOrDefault(self =>
                                    GenerationEngine.GetQualifiedName(self) == GenerationEngine.EnumerableQualifiedName);

                            if (enumerable != null)
                            {
                                var elementType = enumerable.TypeArguments.First();

                                using (code.BeginScope())
                                {
                                    var countTechnique = GenerationEngine.GetAllMembers(type)
                                        .Where(self => self is IPropertySymbol)
                                        .Aggregate("Count()", (current, symbol) => symbol.Name switch
                                        {
                                            "Count" => "Count",
                                            "Length" => "Length",
                                            _ => current
                                        });

                                    var prefix = GenerationEngine.GetCamelCase(name);

                                    code.AppendLine($"var {prefix}Count = {name}.{countTechnique};");
                                    code.AppendLine($"writer.Write({prefix}Count);");

                                    using (code.BeginScope($"foreach (var {prefix}Entry in {name})"))
                                    {
                                        Make(member, elementType, code, $"{prefix}Entry", location,
                                            scope);
                                    }
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
                                            "Could not serialize property '{0}' of type {1} because Interface types are not supported",
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

                                if (GenerationEngine.HasImplementation(type, GenerationEngine.PackingMethod) || GenerationEngine.HasMarkedAsSerializable(type))
                                {
                                    code.AppendLine($"{name}.{GenerationEngine.PackingMethod}(writer);");
                                }
                                else
                                {
                                    using (code.BeginScope())
                                    {
                                        GenerationEngine.Generate($"{name}.", type, code, GenerationType.Write);
                                    }
                                }
                            }

                            break;
                        case TypeKind.Array:
                            var array = (IArrayTypeSymbol)type;

                            code.AppendLine($"writer.Write({name}.Length);");

                            if (GenerationEngine.GetQualifiedName(array.ElementType) == "System.Byte")
                            {
                                code.AppendLine($"writer.Write({name});");
                            }
                            else
                            {
                                var prefix = GenerationEngine.GetCamelCase(name);
                                var indexName = $"{prefix}Idx";

                                using (code.BeginScope(
                                    $"for (var {indexName} = 0; {indexName} < {name}.Length; {indexName}++)"))
                                {
                                    Make(member, array.ElementType, code, $"{name}[{indexName}]",
                                        location,
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