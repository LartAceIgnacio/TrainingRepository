using System;

namespace BlastAsia.DigiBook.Domain.Pilots
{
    public class DateActivatedRequiredException
        :ApplicationException
    {
        public DateActivatedRequiredException(string message)
            : base(message)
        {

        }
    }
}