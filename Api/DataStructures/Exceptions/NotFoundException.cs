using System;

namespace CurrencyRate.DataStructures.Exceptions
{
    internal class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message)
        {
            
        }

        public NotFoundException() : base()
        {
        }

        public NotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NotFoundException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
        {
        }
    }
}