using System;

namespace BlastAsia.Digibook.Domain.Employees
{
    public class InvalidNameFormatException:Exception
    {
        public InvalidNameFormatException(string message):base(message)
        {

        }
    }
}