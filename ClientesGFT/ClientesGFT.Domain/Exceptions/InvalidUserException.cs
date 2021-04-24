using System;
using System.Collections.Generic;
using System.Text;

namespace ClientesGFT.Domain.Exceptions
{
    public class InvalidUserException : ArgumentException
    {
        public InvalidUserException(string message) : base(message)
        {
        }

        public InvalidUserException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
