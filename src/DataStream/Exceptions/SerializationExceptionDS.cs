using System.Runtime.Serialization;

namespace DataStream.Exceptions
{
    internal class SerializationExceptionDS : SerializationException
    {
        public SerializationExceptionDS() { }
        public SerializationExceptionDS(string message) : base(message) { }
        public SerializationExceptionDS(string message, Exception inner) : base(message, inner) { }
    }
}
