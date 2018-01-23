using System;
using BlastAsia.DigiBook.Domain.Models.Pilots;

namespace BlastAsia.DigiBook.Domain.Pilots
{
    public class PilotService
    {
        private IPilotRepository pilotRepository;
        private int stringMaximumLength = 60;
        public PilotService(IPilotRepository pilotRepository)
        {
            this.pilotRepository = pilotRepository;
        }

        public Pilot Save(Guid pilotId, Pilot pilot)
        {
            if (string.IsNullOrEmpty(pilot.FirstName))
            {
                throw new FirstNameRequiredException();
            }
            if (pilot.FirstName.Length > stringMaximumLength)
            {
                throw new FirstNameMaximumLenghtException();
            }
           Pilot result;

           var found = pilotRepository.Retrieve(pilotId);
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