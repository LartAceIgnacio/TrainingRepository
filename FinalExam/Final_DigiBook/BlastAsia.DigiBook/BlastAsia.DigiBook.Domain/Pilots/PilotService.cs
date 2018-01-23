using System;
using System.Linq;
using System.Text.RegularExpressions;
using BlastAsia.DigiBook.Domain.Models.Pilots;

namespace BlastAsia.DigiBook.Domain.Pilots
{
    public class PilotService
    {
        private IPilotRepository pilotRepository;
        const int nameMaximumLength = 60;
        const int minimumAge = 21;
        const int yearsOfExperienceRequirement = 10;
        DateTime now = DateTime.Today;
        private string pilotCodePattern =  @"[A-Za-z]{8}[0-9]{6}$";
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
            if (pilot.MiddleName.Length > nameMaximumLength)
            {
                throw new MiddleNameLengthException();
            }
            if (string.IsNullOrEmpty(pilot.LastName))
            {
                throw new LastNameRequiredException();
            }
            if (pilot.LastName.Length > nameMaximumLength)
            {
                throw new LastNameMaximumLenghtException();
            }
            if (this.ComputeAge(pilot.BirthDate) <= minimumAge)
            {
                throw new MinimumAgeRequirement();
            }
            if (pilot.YearsOfExperience < yearsOfExperienceRequirement)
            {
                throw new YearsOfExperienceMinimumRequiredException();
            }
            pilot.PilotCode = PilotCodeGenerate(pilot);

            if (!Regex.IsMatch(pilot.PilotCode, pilotCodePattern))
            {
                throw new InvalidPilotCodeException();
            }

            var foundPilotCode = pilotRepository.RetrievePilotCode(pilot.PilotCode);
            if (foundPilotCode != null)
            {
                throw new NonUniquePilotCodeException();
            }

            pilot.DateCreated = DateTime.Now;
            pilot.DateCreated = DateTime.Now;
           
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

        public string PilotCodeGenerate(Pilot pilot)
        {
            string result = "";
            result = string.Concat(result, pilot.FirstName.Substring(0, 2));
            result = string.Concat(result, pilot.MiddleName.Substring(0, 2));
            result = string.Concat(result, pilot.LastName.Substring(0, 4));
            result = string.Concat(result, pilot.DateActivated.ToString("yy"));
            result = string.Concat(result, pilot.DateActivated.Month.ToString().PadLeft(2, '0'));
            result = string.Concat(result, pilot.DateActivated.Day.ToString().PadLeft(2, '0'));
            return result;
        }
    }
}