using System;

namespace BlastAsia.DigiBook.Domain.Employees
{
    public class EmailRequiredException
        : Exception
    {
        public EmailRequiredException(string message)
            :base(message)
        {

        }
    }
}