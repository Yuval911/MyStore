using System;
using System.Runtime.Serialization;

namespace MyStore
{
    [Serializable]
    public class UserNameAlreadyExistException : ApplicationException
    {
        public UserNameAlreadyExistException()
        {
        }

        public UserNameAlreadyExistException(string message) : base(message)
        {
        }

        public UserNameAlreadyExistException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UserNameAlreadyExistException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}