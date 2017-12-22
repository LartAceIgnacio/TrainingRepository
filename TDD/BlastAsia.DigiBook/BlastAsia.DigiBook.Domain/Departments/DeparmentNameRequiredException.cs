using System;

namespace BlastAsia.DigiBook.Domain.Departments
{
    public class DeparmentNameRequiredException: Exception
    {
        public DeparmentNameRequiredException(string message)
            :base(message)
        {

        }
    }
}