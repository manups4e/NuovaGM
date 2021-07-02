using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace TheLastPlanet.Generators
{
    [Generator]
    public class SerializationGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new SerializationEngine());
        }

        public void Execute(GeneratorExecutionContext context)
        {
            var engine = (SerializationEngine) context.SyntaxContextReceiver;

            if (engine == null) return;
            var sources = new List<string>();

            foreach (var item in engine.WorkItems)
            {
                var code = engine.Compile(item);
                var unique = $"{item.TypeSymbol.Name}.Serialization.cs";
                if (item.TypeSymbol.ContainingType != null)
                {
                    unique = item.TypeSymbol.ContainingType.Name + "." + unique;
                }

                if (sources.Contains(unique))
                {
                    throw new Exception(
                        $"Could not generate methods for type {item.TypeSymbol.ContainingNamespace}.{item.TypeSymbol.MetadataName} due the source-gen having already processed a type with that name.");
                }

                foreach (var problem in engine.Problems)
                {
                    Location location = null;

                    foreach (var entry in problem.Locations)
                    {
                        location = entry;

                        if (!location.IsInMetadata)
                        {
                            break;
                        }
                    }

                    context.ReportDiagnostic(Diagnostic.Create(problem.Descriptor, location, problem.Format));
                }

                engine.Problems.Clear();
                sources.Add(unique);
                context.AddSource(unique,
                    SourceText.From(code.ToString(), Encoding.UTF8));
            }

            // context.AddSource("Logs.cs",
            //     SourceText.From(
            //         $@"/*{Environment.NewLine + string.Join(Environment.NewLine, engine.Logs) + Environment.NewLine}*/",
            //         Encoding.UTF8));
        }
    }
}