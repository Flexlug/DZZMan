using System.Runtime.Serialization;

namespace DZZMan.API.Exceptions
{
    [Serializable]
    internal class NoSatellitesException : Exception
    {
        public NoSatellitesException() : base("Couldn't find such satellite") { }

        public NoSatellitesException(string? message) : base(message) { }

        public NoSatellitesException(string? message, Exception? innerException) : base(message, innerException) { }

        protected NoSatellitesException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}