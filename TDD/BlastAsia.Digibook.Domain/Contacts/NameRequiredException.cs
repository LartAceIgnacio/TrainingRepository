using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.Digibook.Domain.Contacts
{
    public class NameRequiredException:Exception
    {
        public NameRequiredException(string message) : base(message)
        {

        }
    }
}
