﻿using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Models.Registration
{
    public interface IAccountRepository
    {
        void Create(Account account);
    }
}
