using BlastAsia.DigiBook.Domain.Models;
using BlastAsia.DigiBook.Domain.Models.Reservations;

namespace BlastAsia.DigiBook.Domain.Reservations
{
    public interface IReservationRepository : IRepository<Reservation>
    {
        PaginationResult<Reservation> Retrieve(int page, int record, string filter);
    }
}