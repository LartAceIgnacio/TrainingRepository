using BlastAsia.DigiBook.Domain.Models.Pilots;
using BlastAsia.DigiBook.Domain.Pilots.Pilots;
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
        private readonly string PilotCodePattern = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" + @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
         @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";

        public PilotService(IPilotRepository pilotRepository)
        {
            this.pilotRepository = pilotRepository;
        }
        public Pilot Save(Guid id,Pilot pilot)
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
            //if (!Regex.IsMatch(pilot.PilotCode, PilotCodePattern, RegexOptions.IgnoreCase))
            //{
            //    throw new PilotCodeRequiredException("Pilot Code is Required");
            //}

            Pilot result = null;

            var found = pilotRepository.Retrieve(pilot.PilotId);
            if(found == null)
            {   

                pilot.PilotCode = pilot.FirstName.Substring(startIndex, length)
                    + pilot.MiddleName.Substring(startIndex, length)
                    + pilot.LastName.Substring(startIndex, lNameLength)
                    + pilot.DateActivated.Value.ToString("yy")
                    + pilot.DateActivated.Value.ToString("mm").PadLeft(2,'0')
                    + pilot.DateActivated.Value.ToString("dd").PadLeft(2,'0');

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