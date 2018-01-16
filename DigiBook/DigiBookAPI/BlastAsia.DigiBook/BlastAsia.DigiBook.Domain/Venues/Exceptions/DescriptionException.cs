using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Venues.Exceptions
{
    public class DescriptionException
        :Exception
    {
        public DescriptionException(String message)
            : base(message)
        {

        }
    }
}
