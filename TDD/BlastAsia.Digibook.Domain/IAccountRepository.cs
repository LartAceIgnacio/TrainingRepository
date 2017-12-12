using BlastAsia.Digibook.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.Digibook.Domain
{
    public interface IAccountRepository
    {
        void Create(Account account);
    }
}
