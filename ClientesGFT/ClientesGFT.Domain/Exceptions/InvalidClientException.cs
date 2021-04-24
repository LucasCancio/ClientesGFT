using System;
using System.Collections.Generic;
using System.Text;

namespace ClientesGFT.Domain.Exceptions
{
    public class InvalidClientException : ArgumentException
    {
        public IList<string> Errors { get; private set; }
        public InvalidClientException(string message) : base(message)
        {
            this.Errors = new List<string> { message };
        }

        public InvalidClientException(string[] errors) : base(string.Join("\n", errors))
        {
            this.Errors = errors;
        }

        public InvalidClientException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
