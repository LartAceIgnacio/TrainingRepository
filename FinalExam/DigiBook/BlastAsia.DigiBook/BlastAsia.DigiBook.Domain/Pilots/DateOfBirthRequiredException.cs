using System;

namespace BlastAsia.DigiBook.Domain.Pilots
{
    public class DateOfBirthRequiredException : ApplicationException
    {
        public DateOfBirthRequiredException(string message)
         : base(message)
        {

        }
    }
}