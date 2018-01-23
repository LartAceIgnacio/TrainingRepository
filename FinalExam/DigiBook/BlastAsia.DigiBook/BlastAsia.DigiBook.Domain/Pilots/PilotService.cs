using BlastAsia.DigiBook.Domain.Models.Pilots;
using System;
using static System.Net.Mime.MediaTypeNames;

namespace BlastAsia.DigiBook.Domain.Pilots
{
    public class PilotService : IPilotService
    {
        private IPilotRepository pilotRepository;
        private readonly int NameMaximumLength = 60;
        private readonly int YearsOfExperience = 10;
        private int RequiredDateOfBirth = 21;

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

            Pilot result = null;

            var found = pilotRepository.Retrieve(pilot.PilotId);
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