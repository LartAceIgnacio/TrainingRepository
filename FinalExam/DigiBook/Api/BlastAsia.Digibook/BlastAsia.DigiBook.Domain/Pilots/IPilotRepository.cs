﻿using System;
using BlastAsia.DigiBook.Domain.Models.Pilots;

namespace BlastAsia.DigiBook.Domain.Pilots
{
    public interface IPilotRepository
    {
         Pilot Create(Pilot pilot);
    }
}