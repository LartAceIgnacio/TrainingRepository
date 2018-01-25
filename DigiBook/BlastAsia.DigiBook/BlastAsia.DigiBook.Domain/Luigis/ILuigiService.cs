using BlastAsia.DigiBook.Domain.Models.Luigis;
using System;

namespace BlastAsia.DigiBook.Domain.Luigis
{
    public interface ILuigiService
    {
        Luigi Save(Guid id, Luigi luigi);
    }
}