using System.Globalization;
using System.Linq;
using TheLastPlanet.Shared.Internal.Events.Serialization;
using TheLastPlanet.Shared.Internal.Events.Serialization.Implementations;

namespace TheLastPlanet.Shared
{
    public static class BinaryHelper
    {
        private static ISerialization _serialization { get; set; }

        public static byte[] ToBytes<T>(this T obj)
        {
            _serialization = new BinarySerialization();
            using SerializationContext context = new(typeof(T).ToString(), null, _serialization);
            context.Serialize(obj);
            return context.GetData();
        }
        public static byte[] ToBytes(this string str)
        {
            _serialization = new BinarySerialization();
            var arr = str.ToCharArray();
            if (arr[2] != '-' && arr[5] != '-') return default;
            return str.Split('-').Select(x => byte.Parse(x, NumberStyles.HexNumber)).ToArray();
        }

        public static T FromBytes<T>(this byte[] data)
        {
            _serialization = new BinarySerialization();
            using SerializationContext context = new(data.ToString(), null, _serialization, data);
            return context.Deserialize<T>();
        }
    }
}