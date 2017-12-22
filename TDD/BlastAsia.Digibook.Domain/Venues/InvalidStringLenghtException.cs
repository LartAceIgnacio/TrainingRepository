using System;

namespace BlastAsia.Digibook.Domain
{
    public class InvalidStringLenghtException:Exception
    {
        public InvalidStringLenghtException(string message):base(message)
        {

        }
    }
}