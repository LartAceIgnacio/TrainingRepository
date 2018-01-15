using System;

namespace BlastAsia.DigiBook.Domain.Employees
{
    public class NameRequiredException
        :Exception
    {
        public NameRequiredException(string message)
            :base(message)
        {

        }
    }
}