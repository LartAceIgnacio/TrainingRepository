using System;

namespace BlastAsia.DigiBook.Domain.Employees
{
    public class FirstNameRequiredException
        :Exception
    {
        public FirstNameRequiredException(string message)
            :base(message)
        {

        }
    }
}