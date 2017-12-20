using BlastAsia.DigiBook.Domain.Contacts;
using BlastAsia.DigiBook.Domain.Models.Contacts;

namespace BlastAsia.DigiBook.Infrastructure.Persistence.Repositories
{
    public class ContactRepository
        : RepositoryBase<Contact>, IContactRepository
    {
       public ContactRepository(IDigiBookDbContext context)
            : base(context)
        {

        }
    }
}
