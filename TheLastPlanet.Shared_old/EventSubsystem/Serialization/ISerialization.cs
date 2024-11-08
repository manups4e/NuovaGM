﻿using System;

namespace TheLastPlanet.Shared.Internal.Events.Serialization
{
    public interface ISerialization
    {
        void Serialize(Type type, object value, SerializationContext context);
        void Serialize<T>(T value, SerializationContext context);
        object Deserialize(Type type, SerializationContext context);
        T Deserialize<T>(SerializationContext context);
    }
}