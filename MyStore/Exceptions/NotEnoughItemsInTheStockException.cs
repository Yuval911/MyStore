using System;
using System.Runtime.Serialization;

namespace MyStore
{
    [Serializable]
    public class NotEnoughItemsInTheStockException : ApplicationException
    {
        public NotEnoughItemsInTheStockException()
        {
        }

        public NotEnoughItemsInTheStockException(string message) : base(message)
        {
        }

        public NotEnoughItemsInTheStockException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NotEnoughItemsInTheStockException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}