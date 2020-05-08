using System;
using System.Runtime.Serialization;

namespace MyStore
{
    [Serializable]
    public class ProductDoesntExistException : ApplicationException
    {
        public ProductDoesntExistException()
        {
        }

        public ProductDoesntExistException(string message) : base(message)
        {
        }

        public ProductDoesntExistException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ProductDoesntExistException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}