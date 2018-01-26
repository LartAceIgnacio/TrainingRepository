using BlastAsia.DigiBook.Domain.Models.Pilots;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Pilots
{
    public class PilotCodeGenerator
    {
        public string PilotCodeGenerate(Pilot pilot)
        {
            string result = "";
            result = string.Concat(result, pilot.FirstName.ToUpper().Substring(0, 2));
            if (!string.IsNullOrEmpty(pilot.MiddleName))
            {
                result = string.Concat(result, pilot.MiddleName.ToUpper().Substring(0, 2));
            }
            result = string.Concat(result, pilot.LastName.ToUpper().Substring(0, 4));
            result = string.Concat(result, pilot.DateActivated.ToString("yy"));
            result = string.Concat(result, pilot.DateActivated.Month.ToString().PadLeft(2, '0'));
            result = string.Concat(result, pilot.DateActivated.Day.ToString().PadLeft(2, '0'));
            return result;
        }
    }
}
