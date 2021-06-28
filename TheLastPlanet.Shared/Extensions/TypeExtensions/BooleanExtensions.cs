

namespace TheLastPlanet.Shared.Extensions
{
    
    public static class BooleanExtensions
    {
        public static void Toggle(this ref bool value)
        {
            value = !value;
        }
    }
}