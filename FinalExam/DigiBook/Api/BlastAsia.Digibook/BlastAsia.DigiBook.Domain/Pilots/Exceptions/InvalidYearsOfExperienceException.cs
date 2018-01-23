using System;

namespace BlastAsia.DigiBook.Domain.Pilots.Exceptions
{
    public class InvalidYearsOfExperienceException : ApplicationException
    {
        public InvalidYearsOfExperienceException(string message) : base(message)
        {
        }
    }
}