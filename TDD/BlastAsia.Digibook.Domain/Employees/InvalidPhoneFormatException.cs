using System;

namespace BlastAsia.Digibook.Domain.Employees
{
    public class InvalidPhoneFormatException:Exception
    {
        public InvalidPhoneFormatException(string message) : base(message)
        {
        }
    }
}