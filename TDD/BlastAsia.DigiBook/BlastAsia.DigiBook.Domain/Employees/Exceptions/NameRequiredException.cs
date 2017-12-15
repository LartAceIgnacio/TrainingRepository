﻿using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Employees.Exceptions
{
    public class NameRequiredException : Exception
    {
        public NameRequiredException(String message) : base(message)
        {

        }
    }
}
