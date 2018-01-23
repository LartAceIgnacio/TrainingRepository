using System;

namespace BlastAsia.DigiBook.Domain.Pilots
{
    public class PilotCodeMustBe12CharactersRequiredException
        :ApplicationException
    {
        public PilotCodeMustBe12CharactersRequiredException(string message)
            :base(message)
        {

        }
    }
}