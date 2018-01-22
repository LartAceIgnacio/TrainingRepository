using System;
using System.Runtime.Serialization;

namespace BlastAsia.DigiBook.Domain.Luigis
{
    public class FirstNameRequired : Exception
    {
        public FirstNameRequired(string message)
            : base(message)
        {

        }
    }
}