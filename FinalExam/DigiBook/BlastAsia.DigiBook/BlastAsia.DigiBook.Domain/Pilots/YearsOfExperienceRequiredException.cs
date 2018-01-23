using System;

namespace BlastAsia.DigiBook.Domain.Pilots
{
    public class YearsOfExperienceRequiredException : ApplicationException
    {
        public YearsOfExperienceRequiredException(string message)
        : base(message)
        {

        }
    }
}