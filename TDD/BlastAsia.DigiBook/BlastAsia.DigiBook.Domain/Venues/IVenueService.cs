using BlastAsia.DigiBook.Domain.Models.Venues;

namespace BlastAsia.DigiBook.Domain.Venues
{
    public interface IVenueService
    {
        Venue Save(Venue venue);
    }
}