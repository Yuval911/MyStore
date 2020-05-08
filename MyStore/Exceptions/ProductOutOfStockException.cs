using System;
using System.Runtime.Serialization;

namespace MyStore
{
    [Serializable]
    public class ProductOutOfStockException : ApplicationException
    {
        public ProductOutOfStockException()
        {
        }

        public ProductOutOfStockException(string message) : base(message)
        {
        }

        public ProductOutOfStockException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ProductOutOfStockException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}