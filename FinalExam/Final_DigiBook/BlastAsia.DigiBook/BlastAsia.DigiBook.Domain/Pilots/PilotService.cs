using System;
using BlastAsia.DigiBook.Domain.Models.Pilots;

namespace BlastAsia.DigiBook.Domain.Pilots
{
    public class PilotService
    {
        private IPilotRepository pilotRepository;

        public PilotService(IPilotRepository pilotRepository)
        {
            this.pilotRepository = pilotRepository;
        }

        public Pilot Save(Guid pilotId, Pilot pilot)
        {
           Pilot result;

           Pilot found = pilotRepository.Retrieve(pilotId);
            if (found == null)
            {
                result = pilotRepository.Create(pilot);
            }
            else
            {
                result = pilotRepository.Update(pilotId, pilot);
            }
                
            return result;
        }
    }
}