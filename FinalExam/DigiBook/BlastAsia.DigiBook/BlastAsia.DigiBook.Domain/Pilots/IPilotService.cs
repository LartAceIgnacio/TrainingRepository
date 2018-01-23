using BlastAsia.DigiBook.Domain.Models.Pilots;
using System;

namespace BlastAsia.DigiBook.Domain.Pilots
{
    public interface IPilotService
    {
        Pilot Save(Guid id, Pilot pilot);
    }
}