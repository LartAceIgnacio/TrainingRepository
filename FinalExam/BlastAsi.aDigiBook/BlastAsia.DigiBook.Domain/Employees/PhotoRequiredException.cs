using System;

namespace BlastAsia.DigiBook.Domain.Employees
{
    public class PhotoRequiredException
        : Exception
    {
        public PhotoRequiredException(string message)
            :base(message)
        {

        }
    }
}