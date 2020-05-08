using System;
using System.Runtime.Serialization;

namespace MyStore
{
    [Serializable]
    public class ProductQuantityCannotBeNegativeNumberException : ApplicationException
    {
        public ProductQuantityCannotBeNegativeNumberException()
        {
        }

        public ProductQuantityCannotBeNegativeNumberException(string message) : base(message)
        {
        }

        public ProductQuantityCannotBeNegativeNumberException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ProductQuantityCannotBeNegativeNumberException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}