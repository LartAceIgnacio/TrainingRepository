using BlastAsia.DigiBook.Domain.Luigis;
using BlastAsia.DigiBook.Domain.Models;
using BlastAsia.DigiBook.Domain.Models.Luigis;
using System.Linq;

namespace BlastAsia.DigiBook.Infrastructure.Persistence.Repositories
{
    public class LuigiRepository
        : RepositoryBase<Luigi>, ILuigiRepository
    {
        public LuigiRepository(IDigiBookDbContext context)
            : base(context)
        {
        }
        public Pagination<Luigi> Retrieve(int pageNo, int numRec, string filterValue)
        {
            Pagination<Luigi> result = new Pagination<Luigi>();
            if (string.IsNullOrEmpty(filterValue))
            {
                result.Results = context.Set<Luigi>().OrderBy(x => x.FirstName).ThenBy(x => x.LastName)
                    .Skip(pageNo).Take(numRec).ToList();

                if (result.Results.Count > 0)
                {
                    result.TotalRecords = context.Set<Luigi>().Count();
                    result.PageNo = pageNo;
                    result.PageRecord = numRec;
                }
            }
            else
            {
                result.Results = context.Set<Luigi>().Where(x => x.FirstName.ToLower().Contains(filterValue.ToLower()) ||
                    x.LastName.ToLower().Contains(filterValue.ToLower()))
                    .OrderBy(x => x.FirstName).ThenBy(x => x.LastName)
                    .Skip(pageNo).Take(numRec).ToList();

                if (result.Results.Count > 0)
                {
                    result.TotalRecords = context.Set<Luigi>().Where(x => x.FirstName.ToLower().Contains(filterValue.ToLower()) ||
                        x.LastName.ToLower().Contains(filterValue.ToLower())).Count();
                    result.PageNo = pageNo;
                    result.PageRecord = numRec;
                }
            }

            return result;
        }
    }
}