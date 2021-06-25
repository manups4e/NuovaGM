using System;
using System.Collections.Generic;
using MsgPack;
using MsgPack.Serialization;
using TheLastPlanet.Shared.Snowflakes.Serialization;
using TheLastPlanet.Shared.Snowflakes;
using System.IO;
using Logger;

namespace TheLastPlanet.Shared
{
    public static class MsgPackHelper
	{
		private static Log Logger = new();

		public static readonly SerializationContext Empty = new SerializationContext();

		public static MessagePackObject ToByte<T>(this T value, SnowflakeRepresentation representation = SnowflakeRepresentation.String)
        {
			SerializationContext context = new() { SerializationMethod = SerializationMethod.Map };
			SnowFlakeConverter _snowflakeConverter = new(context);
			try
			{
				context.Serializers.RegisterOverride(_snowflakeConverter);
				MessagePackSerializer<T> serializer = MessagePackSerializer.Get<T>(context);

				var result = serializer.PackSingleObject(value);

				return new MessagePackObject(result);
			}
			catch (Exception e)
			{
                Logger.Error(e.ToString());
				return default;
			}
        }

        public static T FromByte<T>(this MessagePackObject serialized)
		{
			SerializationContext context = new() { SerializationMethod = SerializationMethod.Map };
			SnowFlakeConverter _snowflakeConverter = new(context);
			try
			{
				context.Serializers.RegisterOverride(_snowflakeConverter);
				MessagePackSerializer<T> deserializer = MessagePackSerializer.Get<T>(context);
				return deserializer.UnpackSingleObject((byte[])serialized);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				return default;
			}

		}

	}
}