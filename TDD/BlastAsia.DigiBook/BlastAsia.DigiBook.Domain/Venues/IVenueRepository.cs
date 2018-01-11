﻿using BlastAsia.DigiBook.Domain.Appointments;
using BlastAsia.DigiBook.Domain.Models;
using BlastAsia.DigiBook.Domain.Models.Venues;

namespace BlastAsia.DigiBook.Domain.Venues
{
    public interface IVenueRepository 
    : IRepository<Venue>
    {
        Pagination<Venue> Retrieve(int pageNo, int numRec, string filterValue);
        
    }
}