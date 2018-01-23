using System;
using BlastAsia.DigiBook.Domain.Models.Pilots;

namespace BlastAsia.DigiBook.Domain.Pilots
{
    public interface IPilotService
    {
        Pilot Save(Guid id, Pilot pilot);
    }
}