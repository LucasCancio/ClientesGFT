using System;

namespace ClientesGFT.Domain.Exceptions
{
    public class InvalidLoginException : ArgumentException
    {
        public InvalidLoginException()
        {
        }
        public InvalidLoginException(string message) : base(message)
        {
        }

        public InvalidLoginException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
