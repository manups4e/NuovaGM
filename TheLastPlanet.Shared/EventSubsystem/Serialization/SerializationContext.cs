using System;
using System.IO;

namespace TheLastPlanet.Shared.Internal.Events.Serialization
{
    public class SerializationContext : IDisposable
    {
        public string Source { get; set; }
        public string Details { get; set; }
        public BinaryWriter Writer
        {
            get => _writer;
            set
            {
                _writer?.Dispose();
                _writer = value;
            }
        }

        public BinaryReader Reader
        {
            get => _reader;
            set
            {
                _reader?.Dispose();
                _reader = value;
            }
        }

        public byte[] Original { get; set; }

        private ISerialization _serialization;
        private MemoryStream _memory;
        private BinaryReader _reader;
        private BinaryWriter _writer;

        public byte[] GetData()
        {
            return _memory.ToArray();
        }

        public SerializationContext(string source, string details, ISerialization serialization)
        {
            Source = source;
            Details = details;
            _serialization = serialization;
            _memory = new MemoryStream();
            _writer = new BinaryWriter(_memory);
        }

        public SerializationContext(string source, string details, ISerialization serialization, byte[] data)
        {
            Source = source;
            Details = details;
            _serialization = serialization;
            _memory = new MemoryStream(data);
            _reader = new BinaryReader(_memory);

            Original = data;
        }

        public void Dispose()
        {
            Writer?.Dispose();
            Reader?.Dispose();
        }

        public void Serialize(Type type, object value) => _serialization.Serialize(type, value, this);
        public void Serialize<T>(T value) => _serialization.Serialize(value, this);
        public object Deserialize(Type type) => _serialization.Deserialize(type, this);
        public T Deserialize<T>() => _serialization.Deserialize<T>(this);
    }
}