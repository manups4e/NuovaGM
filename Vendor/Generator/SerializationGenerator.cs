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
			context.RegisterForSyntaxNotifications(() => GenerationEngine.Instance);
        }

        public void Execute(GeneratorExecutionContext context)
        {
            var engine = (GenerationEngine) context.SyntaxContextReceiver;

            if (engine == null) return;

            foreach (var item in engine.WorkItems)
            {
                var code = engine.Compile(item);
                var unique = $"{item.TypeSymbol.Name}.Serialization.cs";
                if (item.TypeSymbol.ContainingType != null)
                {
                    unique = item.TypeSymbol.ContainingType.Name + "." + unique;
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
                try
                {
                    context.AddSource(unique,
                        SourceText.From(code.ToString(), Encoding.UTF8));
                }
                catch (ArgumentException)
                {
                    throw new Exception(
                        $"Duplicate entry: {item.TypeSymbol.ContainingNamespace}.{item.TypeSymbol.MetadataName}");
                }
            }

            // context.AddSource("Logs.cs",
            //     SourceText.From(
            //         $@"/*{Environment.NewLine + string.Join(Environment.NewLine, engine.Logs) + Environment.NewLine}*/",
            //         Encoding.UTF8));
        }
    }
}