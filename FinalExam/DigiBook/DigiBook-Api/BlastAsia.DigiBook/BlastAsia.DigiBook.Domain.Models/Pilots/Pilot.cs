﻿using System;

namespace BlastAsia.DigiBook.Domain.Models.Pilots
{
    public class Pilot
    {
        public Guid PilotId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? DateActivated { get; set; }
        public int? YearsOfExperience { get; set; }
        public string PilotCode { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
    }
}