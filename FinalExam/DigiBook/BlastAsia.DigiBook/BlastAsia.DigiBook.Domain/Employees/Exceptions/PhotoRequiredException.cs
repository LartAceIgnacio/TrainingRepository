using System;

namespace BlastAsia.DigiBook.Domain.Employees.Exceptions
{
    public class PhotoRequiredException : Exception
    {
        public PhotoRequiredException(string message)
            : base(message)
        {
        }
    }
}