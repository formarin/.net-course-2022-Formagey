using System;

namespace Services.Exceptions
{
    public class NoPassportDataException : Exception
    {
        public NoPassportDataException(string message) : base(message) { }
    }
}