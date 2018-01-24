using BlastAsia.DigiBook.Domain.Inventories;
using BlastAsia.DigiBook.Domain.Models.Inventories;
using BlastAsia.DigiBook.Domain.Models.Pagination;
using System.Linq;

namespace BlastAsia.DigiBook.Infrastructure.Persistence.Repositories
{
    public class InventoryRepository : RepositoryBase<Inventory>, IInventoryRepository
    {
        private DigiBookDbContext context;
        public InventoryRepository(DigiBookDbContext context) : base(context)
        {
            this.context = context;
        }

        public Inventory CheckCode(string code)
        {
            return this.context.Set<Inventory>().FirstOrDefault(r => r.ProductCode == code);
        }

        public Pagination<Inventory> Retrieve(int pageNumber, int recordNumber, string searchKey)
        {
            Pagination<Inventory> result = new Pagination<Inventory>()
            {
                PageNumber = pageNumber < 0 ? 1 : pageNumber,
                RecordNumber = recordNumber < 0 ? 1 : recordNumber,
                TotalCount = this.context.Set<Inventory>().Where(r => r.IsActive == true).Count()
            };

            if (pageNumber < 0)
            {
                result.Result = this.context.Set<Inventory>().Where(r => r.IsActive == true).OrderBy(c => c.ProductName)
                    .Skip(0).Take(10).ToList();

                return result;
            }
            if (recordNumber < 0)
            {
                result.Result = this.context.Set<Inventory>().Where(r => r.IsActive == true).OrderBy(c => c.ProductName)
                    .Skip(0).Take(10).ToList();

                return result;
            }
            if (string.IsNullOrEmpty(searchKey))
            {
                result.Result = this.context.Set<Inventory>().Where(r => r.IsActive == true).OrderBy(c => c.ProductName)
                   .Skip(pageNumber).Take(recordNumber).ToList();

                return result;
            }
            else
            {
                result.Result = this.context.Set<Inventory>().Where(r => r.IsActive == true).Where(r => r.ProductName.Contains(searchKey) || r.ProductDescription.Contains(searchKey))
                                                 .OrderBy(c => c.ProductName)
                                                 .Skip(pageNumber)
                                                 .Take(recordNumber)
                                                 .ToList();

                result.TotalCount = result.Result.Count();

                return result;
            }
        }
    }
}