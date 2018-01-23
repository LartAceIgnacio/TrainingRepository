using System;
using BlastAsia.DigiBook.Domain.Models.Pilots;

namespace BlastAsia.DigiBook.Domain.Pilots
{
    public class PilotService : IPilotService
    {
        private IPilotRepository repo;

        public PilotService(IPilotRepository repo)
        {
            this.repo = repo;
        }

        public Pilot Save(Guid id, Pilot pilot)
        {
            return repo.Create(pilot);
        }
    }
}