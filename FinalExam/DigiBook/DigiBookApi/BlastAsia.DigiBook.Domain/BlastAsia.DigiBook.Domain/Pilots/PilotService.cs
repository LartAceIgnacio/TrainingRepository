using System;
using System.Text.RegularExpressions;
using BlastAsia.DigiBook.Domain.Models.Pilots;
using BlastAsia.DigiBook.Domain.Pilots.Exceptions;
using BlastAsia.DigiBook.Domain.Pilots.Pilots.Exceptions;

namespace BlastAsia.DigiBook.Domain.Pilots
{
    public class PilotService : IPilotService
    {

        private readonly string codeFormat = @"^(([A-Z]){8}|([A-Z]){6})([0-9]{2})([0]{1}[1-9]{1}|[1]{1}[0-2]{1})([0]{1}[1-9]{1}|[1-2]{1}[0-9]{1}|[3]{1}[0-1]{1})$\b";

        private IPilotRepository pilotRepository;

        public PilotService(IPilotRepository pilotRepository)
        {
            this.pilotRepository = pilotRepository;
        }

        public Pilot Save(Guid pilotId, Pilot pilot)
        {
            if (string.IsNullOrWhiteSpace(pilot.FirstName))
            {
                throw new RequiredException();
            }
            var a = Regex.IsMatch(pilot.PilotCode, codeFormat, RegexOptions.IgnoreCase);
            if (!Regex.IsMatch(pilot.PilotCode, codeFormat, RegexOptions.IgnoreCase))
            {
                throw new InvalidFormatException();
            }
            return pilotRepository.Create(pilot);
        }
    }
}