using System;
using System.Collections.Generic;
using System.Text;
using BlastAsia.DigiBook.Domain.Models;

namespace BlastAsia.DigiBook.Domain.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        public bool Create(Account account)
        {
            return true;
        }

        public int ReturnsId(int id)
        {
            return id;
        }
    }
}
