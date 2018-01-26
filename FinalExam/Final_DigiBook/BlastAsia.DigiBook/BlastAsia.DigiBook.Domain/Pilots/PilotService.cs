using System;
using System.Linq;
using System.Text.RegularExpressions;
using BlastAsia.DigiBook.Domain.Models.Pilots;
using BlastAsia.DigiBook.Domain.Pilots;

namespace BlastAsia.DigiBook.Domain.Pilots
{
    public class PilotService : IPilotService
    {
        private IPilotRepository pilotRepository;
        const int nameMaximumLength = 60;
        const int minimumAge = 21;
        const int yearsOfExperienceRequirement = 10;
        DateTime now = DateTime.Today;
        private string pilotCodePattern =  @"^[A-Z]{6,8}[0-9]{6}$";
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
            if (pilot.FirstName.Length > nameMaximumLength)
            {
                throw new FirstNameMaximumLenghtException();
            }
            if(!string.IsNullOrEmpty(pilot.MiddleName))
            {
                if (pilot.MiddleName.Length > nameMaximumLength)
                {
                    throw new MiddleNameLengthException();
                }
            }
            if (string.IsNullOrEmpty(pilot.LastName))
            {
                throw new LastNameRequiredException();
            }
            if (pilot.LastName.Length > nameMaximumLength)
            {
                throw new LastNameMaximumLenghtException();
            }
            if (this.ComputeAge(pilot.BirthDate) < minimumAge)
            {
                throw new MinimumAgeRequirement();
            }
            if (pilot.YearsOfExperience < yearsOfExperienceRequirement)
            {
                throw new YearsOfExperienceMinimumRequiredException();
            }
  
            if (pilot.PilotId == Guid.Empty)
            {
                var generator = new PilotCodeGenerator();
                pilot.PilotCode = generator.PilotCodeGenerate(pilot);

                if (!Regex.IsMatch(pilot.PilotCode, pilotCodePattern))
                {
                    throw new InvalidPilotCodeException();
                }

                var foundPilotCode = pilotRepository.Retrieve(pilot.PilotCode);

                if (foundPilotCode != null)
                {
                    throw new NonUniquePilotCodeException();
                }

                pilot.DateCreated = DateTime.Now;
            }
            else
            {
                pilot.DateModified = DateTime.Now;
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
        
        public int ComputeAge(DateTime birthdate)
        {
            int result = now.Year - birthdate.Year ;
            return result;
        }

      
    }
}