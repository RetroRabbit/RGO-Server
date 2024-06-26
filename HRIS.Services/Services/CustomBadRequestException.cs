using System.Runtime.Serialization;

namespace HRIS.Services
{
    [Serializable]
    public class CustomBadRequestException : Exception
    {
        public CustomBadRequestException()
        {
        }

        public CustomBadRequestException(string? message) : base(message)
        {
        }

        public CustomBadRequestException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        public CustomBadRequestException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
