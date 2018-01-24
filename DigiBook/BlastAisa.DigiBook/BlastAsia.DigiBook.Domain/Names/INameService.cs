using BlastAsia.DigiBook.Domain.Models.Names;
using System;

namespace BlastAsia.DigiBook.Domain.Names
{
    public interface INameService
    {
        Name Save(Guid id, Name name);
    }
}