using System;

namespace BlastAsia.DigiBook.Domain.Employees
{
    public class LastNameRequiredException
        :Exception
    {
        public LastNameRequiredException(string message)
            :base(message)
        {

        }
    }
}