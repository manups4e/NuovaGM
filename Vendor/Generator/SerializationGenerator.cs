using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace TheLastPlanet.Events.Generator
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
            List<string> _sources = new();
            var engine = (GenerationEngine) context.SyntaxContextReceiver;

            if (engine == null) return;

            foreach (var item in engine.WorkItems.ToList())
            {
                var code = engine.Compile(item);
                var identifier = $"{item.TypeSymbol.Name}";
                var count = _sources.Count(self => $"{self}" == identifier);
                var unique = $"{identifier}.Serialization.cs";

                foreach (var problem in engine.Problems)
                {
                    Location location = null;

                    foreach (var entry in problem.Locations.ToList())
                    {
                        location = entry;

                        if (!location.IsInMetadata) { break; }
                    }

                    context.ReportDiagnostic(Diagnostic.Create(problem.Descriptor, location, problem.Format));
                }

                engine.Problems.Clear();

                try
                {
                    context.AddSource(unique, SourceText.From(code.ToString(), Encoding.UTF8));
                    _sources.Add(identifier);
                }
                catch (ArgumentException)
                {
                    throw new Exception(
                        $"Duplicate entry '{item.TypeSymbol.ContainingNamespace}.{item.TypeSymbol.MetadataName}' ({unique})");
                }
            }
            
            _sources.Clear();
            GenerationEngine.Instance.Init();

            // context.AddSource("Logs.cs",
            //     SourceText.From(
            //         $@"/*{Environment.NewLine + string.Join(Environment.NewLine, engine.Logs) + Environment.NewLine}*/",
            //         Encoding.UTF8));
        }
    }
}