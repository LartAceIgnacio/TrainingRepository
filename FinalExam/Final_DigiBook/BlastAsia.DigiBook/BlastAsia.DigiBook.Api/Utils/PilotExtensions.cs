using BlastAsia.DigiBook.Domain.Models.Pilots;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlastAsia.DigiBook.Api.Utils
{
    public static class PilotExtension
    {
        public static Pilot ApplyChanges(this Pilot pilot, Pilot from)
        {
            pilot.FirstName = from.FirstName;
            pilot.LastName = from.LastName;
            pilot.MiddleName = from.MiddleName;
            pilot.BirthDate = from.BirthDate;
            pilot.YearsOfExperience = from.YearsOfExperience;
            pilot.DateModified = DateTime.Now;
            return pilot;
        }
    }
}
