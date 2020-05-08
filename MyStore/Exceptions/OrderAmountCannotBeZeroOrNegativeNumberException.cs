using System;
using System.Runtime.Serialization;

namespace MyStore
{
    [Serializable]
    public class OrderAmountCannotBeZeroOrNegativeNumberException : ApplicationException
    {
        public OrderAmountCannotBeZeroOrNegativeNumberException()
        {
        }

        public OrderAmountCannotBeZeroOrNegativeNumberException(string message) : base(message)
        {
        }

        public OrderAmountCannotBeZeroOrNegativeNumberException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected OrderAmountCannotBeZeroOrNegativeNumberException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}