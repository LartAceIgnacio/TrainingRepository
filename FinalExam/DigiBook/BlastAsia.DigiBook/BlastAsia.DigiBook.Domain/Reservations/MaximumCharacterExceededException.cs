using System;

namespace BlastAsia.DigiBook.Domain.Reservations
{
    public class MaximumCharacterExceededException: Exception
    {
        public MaximumCharacterExceededException(string message): base(message)
        {

        }
    }
}