﻿using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Appointments
{
    public class InvalidEmployeeIdException : ApplicationException
    {
        public InvalidEmployeeIdException(string message) : base (message)
        {

        }
    }
}
