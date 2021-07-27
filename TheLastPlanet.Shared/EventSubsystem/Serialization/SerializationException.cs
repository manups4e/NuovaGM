using System;
using TheLastPlanet.Shared.Internal.Events.Serialization;

namespace TheLastPlanet.Shared.Internal.Events.Exceptions
{
    public class SerializationException : Exception
    {
        public SerializationContext Context { get; set; }
        public Type InvolvedType { get; set; }

        public override string Message
        {
            get
            {
                if (Context != null && InvolvedType != null && _message != null)
                {
                    return
                        $"{Context.Source}: {(Context.Details != null ? $"[{Context.Details}] " : string.Empty)}({InvolvedType.FullName}) {_message}:";
                }

                return null;
            }
        }

        public override string StackTrace => null;

        private string _message;

        public SerializationException(SerializationContext context, Type type, string message)
        {
            Context = context;
            InvolvedType = type;
            _message = message;
        }

        public SerializationException(SerializationContext context, Type type, string message, Exception innerException)
            : base(null, innerException)
        {
            Context = context;
            InvolvedType = type;
            _message = message;
        }
    }
}