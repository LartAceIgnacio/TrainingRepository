using System;
using System.Runtime.Serialization;

namespace BlastAsia.DigiBook.Domain.Pilots
{
    [Serializable]
    public class InvalidFormatException : ApplicationException
    {

        public InvalidFormatException(string message) : base(message)
        {
        }

    }
}