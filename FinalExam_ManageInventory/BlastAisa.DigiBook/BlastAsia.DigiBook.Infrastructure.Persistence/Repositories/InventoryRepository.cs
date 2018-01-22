using BlastAsia.DigiBook.Domain.Inventories;
using BlastAsia.DigiBook.Domain.Models.Inventories;

namespace BlastAsia.DigiBook.Infrastructure.Persistence.Repositories
{
    public class InventoryRepository : RepositoryBase<Inventory>, IInventoryRepository
    {
        private DigiBookDbContext context;
        public InventoryRepository(DigiBookDbContext context) : base(context)
        {
            this.context = context;
        }
    }
}