using System;
using BlastAsia.DigiBook.Domain.Models.Inventories;
using BlastAsia.DigiBook.Domain.Inventories;

namespace BlastAsia.DigiBook.Infrastructure.Persistence.Repositories
{
    public class InventoryRepository 
        : RepositoryBase<Inventory>, IInventoryRepository
    {
        private DigiBookDbContext dbContext;

        public InventoryRepository(IDigiBookDbContext context)
            :base(context)
        {
        }

    }
}