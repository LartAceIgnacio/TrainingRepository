using System;
using BlastAsia.DigiBook.Domain.Models.Pilots;

namespace BlastAsia.DigiBook.Domain.Pilots
{
    public interface IPilotRepository
    {
        Pilot Create(Pilot pilot);
        Pilot Retrieve(string code);
        Pilot Retrieve(Guid id);
        Pilot Update(Pilot pilot);
    }
}