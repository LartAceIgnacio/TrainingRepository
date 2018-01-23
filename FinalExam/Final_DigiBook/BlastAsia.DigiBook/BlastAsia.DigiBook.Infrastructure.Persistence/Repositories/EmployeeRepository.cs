using BlastAsia.DigiBook.Domain.Employees;
using BlastAsia.DigiBook.Domain.Models.Employees;
using BlastAsia.DigiBook.Domain.Models.Records;
using System.Linq;

namespace BlastAsia.DigiBook.Infrastructure.Persistence.Repositories
{
    public class EmployeeRepository
        : RepositoryBase<Employee>, IEmployeeRepository
    { 
       public EmployeeRepository(IDigiBookDbContext context)
            : base(context)
        {
        }
       public Record<Employee> Fetch(int pageNo, int numRec, string filterValue)
       {
            Record<Employee> fetchResult = new Record<Employee>();
            if (string.IsNullOrEmpty(filterValue))
            {
                fetchResult.Result = context.Set<Employee>().OrderBy(x => x.FirstName).ThenBy(x => x.LastName)
                    .Skip(pageNo).Take(numRec).ToList();

                if (fetchResult.Result.Count > 0)
                {
                    fetchResult.TotalRecord = context.Set<Employee>().Count();
                    fetchResult.PageNo = pageNo;
                    fetchResult.RecordPage = numRec;
                }

                return fetchResult;
            }
            else
            {
                fetchResult.Result = context.Set<Employee>().Where(x => x.FirstName.ToLower().Contains(filterValue.ToLower()) ||
                    x.LastName.ToLower().Contains(filterValue.ToLower()))
                    .OrderBy(x => x.FirstName).ThenBy(x => x.LastName)
                    .ToList();

                if (fetchResult.Result.Count > 0)
                {
                    fetchResult.TotalRecord = context.Set<Employee>().Where(x => x.FirstName.ToLower().Contains(filterValue.ToLower()) ||
                        x.LastName.ToLower().Contains(filterValue.ToLower())).Count();
                    fetchResult.PageNo = pageNo;
                    fetchResult.RecordPage = numRec;
                }

                return fetchResult;
            }
       }
    }
}


