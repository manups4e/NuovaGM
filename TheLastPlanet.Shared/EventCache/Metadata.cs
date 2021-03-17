using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace TheLastPlanet.Shared
{
    public class Metadata
    {
        public Dictionary<string, MetadataPack> Datapack { get; set; } = new Dictionary<string, MetadataPack>();

        public T Find<T>(int index)
        {
            return Find<T>($"payload_{index}");
        }

        public T Find<T>(string key)
        {
            return Datapack.TryGetValue(key, out var response) ? Parse<T>(response) : default(T);
        }

        public void Write(string key, object payload)
        {
            Datapack[key] = new MetadataPack(payload, payload.GetType());
        }

        public void Write(int index, object payload)
        {
            Write($"payload_{index}", payload);
        }

        public void Flush(string key)
        {
            Datapack.Remove(key);
        }

        public void Flush(int index)
        {
            Datapack.Remove($"payload_{index}");
        }

        public int Length()
        {
            return Datapack.Count;
        }

        public T Parse<T>(MetadataPack data)
        {
            return (T)data.Container;
        }

        public Dictionary<string, object> ToDictionary()
        {
            var dictionary = new Dictionary<string, object>();

            foreach (var entry in Datapack)
            {
                dictionary[entry.Key] = Parse<object>(entry.Value);
            }

            return dictionary;
        }

        public IEnumerable<object> AsEnumerable()
        {
            return Datapack.Select(self => Convert.ChangeType(self.Value.Container, self.Value.Type)).ToArray();
        }
    }

    public class MetadataPack
    {
        [JsonIgnore] public Type Type { get; set; }
        [JsonIgnore] public object Container { get; set; }

        [JsonProperty("__type")]
        public string _Type
        {
            get => Type.FullName;
            set => Type = Type.GetType(value);
        }

        [JsonProperty("__container")]
        public string _Container
        {
            get => JsonConvert.SerializeObject(Container);
            set => Unpack(value);
        }

        public MetadataPack(dynamic pack, Type type)
        {
            Type = type;
            Container = pack;
        }

        public void Unpack(string container)
        {
            Container = JsonConvert.DeserializeObject(container, Type);
        }
    }
}