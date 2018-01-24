using BlastAsia.DigiBook.Domain.Models.Names;
using BlastAsia.DigiBook.Domain.Names;

namespace BlastAsia.DigiBook.Infrastructure.Persistence.Repositories
{
    public class NameRepository : RepositoryBase<Name>, INameRepository
    {
        private DigiBookDbContext context;
        public NameRepository(DigiBookDbContext context) : base(context)
        {
            this.context = context;
        }
    }
}