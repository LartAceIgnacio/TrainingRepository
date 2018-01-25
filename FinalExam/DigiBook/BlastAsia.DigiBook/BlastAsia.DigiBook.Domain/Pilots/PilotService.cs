using BlastAsia.DigiBook.Domain.Models.Pilots;
using BlastAsia.DigiBook.Domain.Pilots;
using System;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

namespace BlastAsia.DigiBook.Domain.Pilots
{
    public class PilotService : IPilotService
    {
        private IPilotRepository pilotRepository;
        private readonly int NameMaximumLength = 60;
        private readonly int YearsOfExperience = 10;
        private int RequiredDateOfBirth = 21;

        private int startIndex = 0;
        private int length = 2;
        private int lNameLength = 4;
        private readonly string PilotCodePattern = @"[A-Z]{2}[A-Z]{0,2}[A-Z]{4}[0-9]{2}[0-9]{2}[0-9]{2}";

        public PilotService(IPilotRepository pilotRepository)
        {
            this.pilotRepository = pilotRepository;
        }
        public Pilot Save(Guid id, Pilot pilot)
        {
            if (string.IsNullOrEmpty(pilot.FirstName))
            {
                throw new PilotNameRequiredException("Pilot Name Required");
            }
            if (pilot.FirstName.Length > NameMaximumLength)
            {
                throw new MaximumLengthRequiredException("FirstName must be lessthan 60 Characters");
            }
            if (pilot.MiddleName.Length > NameMaximumLength)
            {
                throw new MaximumLengthRequiredException("MiddleName must be lessthan 60 Characters");
            }
            if (pilot.LastName.Length > NameMaximumLength)
            {
                throw new MaximumLengthRequiredException("LastName must be lessthan 60 Characters");
            }
            if (pilot.DateOfBirth == null)
            {
                throw new DateOfBirthRequiredException("Date Of Birth is Required");
            }
            if (pilot.YearsOfExperience == null)
            {
                throw new YearsOfExperienceRequiredException("Years of Experience Required");
            }
            if (pilot.YearsOfExperience < YearsOfExperience)
            {
                throw new YearsOfExperienceRequiredException("10 Years of Experience is required");
            }
            DateTime? birthDate = pilot.DateOfBirth;
            DateTime dateToday = DateTime.Now;
            if ((dateToday.Year - birthDate.Value.Year) < RequiredDateOfBirth)
            {

                throw new GreatherThanTwentyOneYearsRequiredException("Must be 21 Years or Older");
            }
            if (pilot.DateActivated == null)
            {
                throw new DateRequiredException("EmploymentDate is required");
            }

           
            
            if (pilot.PilotId == Guid.Empty)
            {

                pilot.PilotCode = GetPilotCode(pilot);
                pilot.DateCreated = DateTime.Now;

                if (!Regex.IsMatch(pilot.PilotCode, PilotCodePattern, RegexOptions.IgnoreCase))
                {
                    throw new InvalidFormatException("Invalid Format Pilot Code Required");
                }

                var foundPilotCode = pilotRepository.Retrieve(pilot.PilotCode);

                if (foundPilotCode != null)
                    {
                    throw new PilotCodeRequiredException("Unique Pilot Code Required");
                    }
            }
            else
            {
                pilot.DateModified = DateTime.Now;
            }
         

            Pilot result = null;

            var found = pilotRepository.Retrieve(pilot.PilotId);
            if (found == null)
            {

                result = pilotRepository.Create(pilot);

            }
            else
            {
                result = pilotRepository.Update(pilot.PilotId, pilot);
            }

            return result;
        }
        public string GetPilotCode(Pilot pilot)
        {
            var pilotCode = pilot.FirstName.Substring(startIndex, length)
                   + pilot.MiddleName.Substring(startIndex, length)
                   + pilot.LastName.Substring(startIndex, lNameLength)
                   + pilot.DateActivated.Value.ToString("yy")
                   + pilot.DateActivated.Value.ToString("MM").PadLeft(2, '0')
                   + pilot.DateActivated.Value.ToString("dd").PadLeft(2, '0');

            return pilotCode;
        }
    }
}