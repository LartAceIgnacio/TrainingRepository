using BlastAsia.DigiBook.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain
{
    public interface IAccountRepository
    {
        bool Create(Account account);
        int ReturnsId(int id);
    }
}
