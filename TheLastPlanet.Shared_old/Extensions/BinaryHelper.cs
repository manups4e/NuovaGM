﻿using System.Globalization;
using System.Linq;
using TheLastPlanet.Shared.Internal.Events.Serialization;
using TheLastPlanet.Shared.Internal.Events.Serialization.Implementations;

namespace TheLastPlanet.Shared
{
    public static class BinaryHelper
    {
        private static readonly BinarySerialization _serialization = new BinarySerialization();

        public static byte[] ToBytes<T>(this T obj)
        {
            using SerializationContext context = new("BinarySerializer", "Binary serialization", _serialization);
            context.Serialize(obj);
            return context.GetData();
        }

        public static T FromBytes<T>(this byte[] data)
        {
            using SerializationContext context = new("BinarySerializer", "Binary deserialization", _serialization, data);
            return context.Deserialize<T>();
        }

        public static byte[] ToBytes(this string str)
        {
            var arr = str.ToCharArray();
            if (arr[2] != '-' && arr[5] != '-') return default;
            return str.Split('-').Select(x => byte.Parse(x, NumberStyles.HexNumber)).ToArray();
        }
    }
}