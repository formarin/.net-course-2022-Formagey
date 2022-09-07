using System;

namespace Services.Exceptions
{
    public class AgeLimitException : Exception
    {
        public AgeLimitException(string message) : base(message) { }
    }
}
