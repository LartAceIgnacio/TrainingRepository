using BlastAsia.DigiBook.Domain.Models.Pilots;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlastAsia.DigiBook.API.Utils
{
    public static class PilotExtension
    {
        public static Pilot ApplyChanges(this Pilot pilot,
            Pilot from)
        {
            pilot.FirstName = from.FirstName;
            pilot.MiddleName = from.MiddleName;
            pilot.LastName = from.LastName;
            pilot.DateOfBirth = from.DateOfBirth;
            pilot.YearsOfExperience = from.YearsOfExperience;
            pilot.DateActivated = from.DateActivated;
            pilot.DateCreated = from.DateCreated;
            pilot.DateModified = from.DateModified;

            return pilot;
        }
            
    }
}
