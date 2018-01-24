using System;
using BlastAsia.DigiBook.Domain.Models.Inventories;
using BlastAsia.DigiBook.Domain.Inventories;
using BlastAsia.DigiBook.Domain.Models;
using System.Linq;

namespace BlastAsia.DigiBook.Infrastructure.Persistence.Repositories
{
    public class InventoryRepository
        : RepositoryBase<Inventory>, IInventoryRepository
    {
        private DigiBookDbContext dbContext;

        public InventoryRepository(IDigiBookDbContext context)
            : base(context)
        {
        }

        public Inventory CheckProductCode(string code)
        {
            return this.context.Set<Inventory>().FirstOrDefault(x => x.ProductCode == code);
        }

        public PaginationClass<Inventory> Retrieve(int pageNo, int numRec, string filterValue)
        {
            PaginationClass<Inventory> result = new PaginationClass<Inventory>();
            if (string.IsNullOrEmpty(filterValue))
            {
                result.Results = context.Set<Inventory>().Where(x => x.IsActive == true)
                    .OrderBy(x => x.ProductName).ThenBy(x => x.ProductDescription)
                    .Skip(pageNo).Take(numRec).ToList();

                if (result.Results.Count > 0)
                {
                    result.TotalRecords = context.Set<Inventory>().Where(x => x.IsActive == true).Count();
                    result.PageNo = pageNo;
                    result.RecordPage = numRec;
                }

                return result;
            }
            else
            {
                result.Results = context.Set<Inventory>().Where(x => x.ProductName.ToLower().Contains(filterValue.ToLower()) ||
                    x.ProductDescription.ToLower().Contains(filterValue.ToLower())).Where(x => x.IsActive == true)
                    .OrderBy(x => x.ProductName).ThenBy(x => x.ProductDescription)
                    .Skip(pageNo).Take(numRec).ToList();

                if (result.Results.Count > 0)
                {
                    result.TotalRecords = context.Set<Inventory>().Where(x => x.ProductName.ToLower().Contains(filterValue.ToLower()) ||
                    x.ProductDescription.ToLower().Contains(filterValue.ToLower())).Where(x => x.IsActive == true).Count();
                    result.PageNo = pageNo;
                    result.RecordPage = numRec;
                }

                return result;
            }
        }
    }
}