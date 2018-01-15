using System;

namespace BlastAsia.DigiBook.Domain.Employees.Exceptions
{
    public class NameRequiredException : Exception
    {
        public NameRequiredException(string message)
            : base(message)
        {

        }
    }
}