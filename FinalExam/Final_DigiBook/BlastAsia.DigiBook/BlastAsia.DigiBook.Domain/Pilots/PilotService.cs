using System;
using BlastAsia.DigiBook.Domain.Models.Pilots;

namespace BlastAsia.DigiBook.Domain.Pilots
{
    public class PilotService
    {
        private IPilotRepository @object;

        public PilotService(IPilotRepository @object)
        {
            this.@object = @object;
        }

        public void Save(Guid pilotId, Pilot pilot)
        {
            throw new NotImplementedException();
        }
    }
}