using System;
using System.Text.RegularExpressions;
using BlastAsia.DigiBook.Domain.Models.Pilots;
using BlastAsia.DigiBook.Domain.Pilots.Exceptions;

namespace BlastAsia.DigiBook.Domain.Pilots
{
    public class PilotService : IPilotService
    {
        private IPilotRepository repo;
        private string regex = @"[A-Za-z]{8}[0-9]{6}$";
        private readonly int maxNameCharacter = 60;
        private readonly int ageRequirement = 21;
        public PilotService(IPilotRepository repo)
        {
            this.repo = repo;
        }

        public Pilot Save(Guid id, Pilot pilot)
        {
            if (string.IsNullOrEmpty(pilot.FirstName))
            {
                throw new InvalidNameException("First Name is required!");
            }
            if (pilot.FirstName.Length > maxNameCharacter)
            {
                throw new InvalidNameException("First Name Should not exceeds 60 character!");
            }
            if (!(string.IsNullOrEmpty(pilot.MiddleName)) && (pilot.MiddleName.Length > maxNameCharacter))
            {
                throw new InvalidNameException("Middle Name Should not exceeds 60 character!");
            }
            if (string.IsNullOrEmpty(pilot.LastName))
            {
                throw new InvalidNameException("Last Name is required!");
            }
            if (pilot.LastName.Length > maxNameCharacter)
            {
                throw new InvalidNameException("First Name Should not exceeds 60 character!");
            }
            if (pilot.DateOfBirth == null)
            {
                throw new InvalidDateException("Date of Birth is required!");
            }

            DateTime? birthDate = pilot.DateOfBirth;
            DateTime dateToday = DateTime.Now;
            //var age = (birthDate.Value.Year - dateToday.Year);
            if((dateToday.Year - birthDate.Value.Year) < ageRequirement)
            {
                throw new InvalidDateException("Age is not qualified!");
            }

            if (pilot.YearsOfExperience == null)
            {
                throw new InvalidYearsOfExperienceException("Years of experience is required!");
            }

            if (pilot.YearsOfExperience < 10)
            {
                throw new InvalidYearsOfExperienceException("Years of experience is not enough!");
            }

            if (pilot.DateActivated == null) {
                throw new InvalidDateException("Date Activated is required!");
            }

            var namePart = (pilot.FirstName.Substring(0, 2) + pilot.MiddleName.Substring(0, 2) + pilot.LastName.Substring(0, 4)).ToUpper();
            var datePart = pilot.DateActivated.Value.ToString("yy") + pilot.DateActivated.Value.Month.ToString().PadLeft(2, '0') + pilot.DateActivated.Value.Day.ToString().PadLeft(2, '0');

            if (!(Regex.IsMatch(namePart + datePart, regex))) {
                throw new InvalidPilotCodeException("Invalid Pilot Code!");
            }

            //var result = Regex.IsMatch(existingCode, regex);
            //Assert.IsTrue(result);

            return repo.Create(pilot);
        }
    }
}