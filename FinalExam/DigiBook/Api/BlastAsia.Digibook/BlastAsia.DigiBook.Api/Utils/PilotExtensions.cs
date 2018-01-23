using BlastAsia.DigiBook.Domain.Models.Pilots;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlastAsia.DigiBook.Api.Utils
{
    public static class PilotExtensions
    {
        public static Pilot ApplyChanges(this Pilot pilot, Pilot from)
        {
            pilot.FirstName = from.FirstName;
            pilot.MiddleName = from.MiddleName;
            pilot.LastName = from.LastName;
            pilot.DateModified = from.DateModified;
            pilot.DateOfBirth = from.DateOfBirth;
            pilot.YearsOfExperience = from.YearsOfExperience;

            return pilot;
    }
    }
}
