using BlastAsia.DigiBook.Domain.Models.Airports;
using BlastAsia.DigiBook.Domain.Models.Paginations;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Airports
{
    public interface IAirportRepository
    {
        Pagination<Airport> pagination(int pageNumber, int recordNumber, string query);
    }
}
