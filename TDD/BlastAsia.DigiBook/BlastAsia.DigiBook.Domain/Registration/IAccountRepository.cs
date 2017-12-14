using BlastAsia.DigiBook.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Registration
{
    public interface IAccountRepository
    {
        void Create(Account account);
    }
}
