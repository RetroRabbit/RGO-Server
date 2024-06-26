using System.Runtime.Serialization;

namespace HRIS.Services.Services
{
    [Serializable]
    public class CustomNotFoundException : Exception
    {
        public CustomNotFoundException()
        {
        }

        public CustomNotFoundException(string? message) : base(message)
        {
        }

        public CustomNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        public CustomNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
