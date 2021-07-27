using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace TheLastPlanet.Events.Generator.Problems
{
    public struct SerializationProblem
    {
        public DiagnosticDescriptor Descriptor;
        public IEnumerable<Location> Locations;
        public object[] Format;
    }
}