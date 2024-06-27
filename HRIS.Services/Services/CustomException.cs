using System.Runtime.Serialization;

namespace HRIS.Services.Services
{
    [Serializable]
    public class CustomException : Exception
    {
        public CustomException()
        {}
        public CustomException(string? message) : base(message)
        {}
    }
}
