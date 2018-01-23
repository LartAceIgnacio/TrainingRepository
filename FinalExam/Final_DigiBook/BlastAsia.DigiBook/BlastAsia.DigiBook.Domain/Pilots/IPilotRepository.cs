﻿using System;
using System.Collections.Generic;
using BlastAsia.DigiBook.Domain.Models.Pilots;

namespace BlastAsia.DigiBook.Domain.Pilots
{
    public interface IPilotRepository
        : IRepository<Pilot>
    {
        Pilot RetrievePilotCode(String pilotCode);
    }
}