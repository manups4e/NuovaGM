using System.Linq;
using Microsoft.CodeAnalysis;

namespace TheLastPlanet.Events.Generator.Extensions
{
    public static class AttributeDataExtensions
    {
        public static T
            GetAttributeValue<T>(this AttributeData attributeData, string name, T defaultValue)
        {
            var constructor = attributeData.AttributeConstructor;
            var parameterSymbol = constructor?.Parameters.FirstOrDefault(self => self?.Name == name);

            TypedConstant value = default;

            if (parameterSymbol != null)
            {
                var parameterIdx = constructor.Parameters.IndexOf(parameterSymbol);

                value = attributeData.ConstructorArguments[parameterIdx];
            }
            else
            {
                if (!attributeData.NamedArguments.IsDefaultOrEmpty &&
                    attributeData.NamedArguments.FirstOrDefault(self => self.Key == name) is var argument)
                {
                    value = argument.Value;
                }
            }

            return (T) (!value.IsNull
                ? value.Kind != TypedConstantKind.Array
                    ? value.Value ?? defaultValue
                    : value.Values != null
                        ? value.Values.Select(typedConst => typedConst.Value).ToList()
                        : defaultValue
                : defaultValue);
        }
    }
}