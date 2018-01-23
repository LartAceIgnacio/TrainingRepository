using System;
using BlastAsia.DigiBook.Domain.Models.Pilots;

namespace BlastAsia.DigiBook.Domain.Pilots
{
    public class PilotService : IPilotService
    {
        private IPilotRepository pilotRepository;
        private DateTime today = DateTime.Today;
        private int ageRequirement = 21;
        private int expRequired = 10;
        private int pilotCodeLength = 14;
        public PilotService(IPilotRepository pilotRepository)
        {
            this.pilotRepository = pilotRepository;
        }

        public Pilot Save(Guid pilotId, Pilot pilot)
        {
            if (string.IsNullOrWhiteSpace(pilot.FirstName))
            {
                throw new FirstNameRequiredException("First name is required");
            }
            else if(pilot.FirstName.Length > 60)
            {
                throw new FirstNameRequiredException("First name must not be greater than sixty");
            }
            if (string.IsNullOrWhiteSpace(pilot.LastName))
            {
                throw new LastNameRequiredException("Last name is required");
            }
            else if (pilot.LastName.Length > 60)
            {
                throw new LastNameRequiredException("Last name must not be greater than sixty");
            }
            if (pilot.DateOfBirth == null)
            {
                throw new DateOfBirthRequiredException("Date of birth is required");
            }
            else
            {
                var age = today.Year - pilot.DateOfBirth.Value.Year;
                if (age <= ageRequirement)
                {
                    throw new DateOfBirthRequiredException("Age must greater than 21");
                }

            }
            if (pilot.YearsOfExperience == null)
            {
                throw new YearsOfExperienceRequiredException("Year of experience is required");
            }
            else
            {
                if (pilot.YearsOfExperience < expRequired)
                {
                    throw new YearsOfExperienceRequiredException("Year of experience must be greater than 10 years");
                }
            }
            if (pilot.DateActivated == null)
            {
                throw new DateActivatedRequiredException("Date activated is required");
            }
            if (pilot.PilotCode.Length != pilotCodeLength)
            {
                throw new PilotCodeMustBe12CharactersRequiredException("Pilot code lenght must be equal to 12");
            }

            var found = pilotRepository.Retrieve(pilot.PilotId);

            Pilot result = null;

            if(found == null)
            {
                result = pilotRepository.Create(pilot);
            }
            else
            {
                result = pilotRepository.Update(pilot.PilotId, pilot);
            }

            return result;


        }
    }
}