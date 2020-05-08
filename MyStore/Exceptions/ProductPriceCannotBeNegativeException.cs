using System;
using System.Runtime.Serialization;

namespace MyStore
{
    [Serializable]
    public class ProductPriceCannotBeNegativeException : ApplicationException
    {
        public ProductPriceCannotBeNegativeException()
        {
        }

        public ProductPriceCannotBeNegativeException(string message) : base(message)
        {
        }

        public ProductPriceCannotBeNegativeException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ProductPriceCannotBeNegativeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}