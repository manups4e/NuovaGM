using System;
using CitizenFX.Core;
using Logger;
using MsgPack;
using MsgPack.Serialization;
using Newtonsoft.Json;

namespace TheLastPlanet.Shared.Snowflakes.Serialization
{
    
    public class SnowflakeConverter : JsonConverter
    {
        public SnowflakeRepresentation Representation { get; set; }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is Snowflake snowflake)
            {
                if (Representation == SnowflakeRepresentation.UInt)
                    writer.WriteValue(snowflake.ToInt64());
                else
                    writer.WriteValue(snowflake.ToString());
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return Representation == SnowflakeRepresentation.UInt
                ? new Snowflake((long) (reader.Value ?? 0))
                : new Snowflake(ulong.Parse((string) reader.Value ?? "0"));
        }

        public override bool CanConvert(Type objectType) => objectType == typeof(Snowflake);
    }

    public class SnowFlakeConverter : MessagePackSerializer<Snowflake>
    {
        public SnowFlakeConverter(SerializationContext ownerContext) : base(ownerContext)
        {
        }

        protected override Snowflake UnpackFromCore(Unpacker unpacker)
        {
            return new Snowflake(unpacker.LastReadData.AsUInt64());
		}

        protected override void PackToCore(Packer packer, Snowflake objectTree)
		{
            packer.Pack(objectTree.ToInt64());
        }
    }
}