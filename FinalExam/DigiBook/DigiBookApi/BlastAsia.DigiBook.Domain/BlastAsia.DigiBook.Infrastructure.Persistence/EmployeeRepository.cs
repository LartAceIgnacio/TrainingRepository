using BlastAsia.DigiBook.Domain.Employees;
using BlastAsia.DigiBook.Domain.Models.Employees;
using BlastAsia.DigiBook.Domain.Models.Paginations;
using System.Linq;

namespace BlastAsia.DigiBook.Infrastructure.Persistence
{
    public class EmployeeRepository : RepositoryBase<Employee>,IEmployeeRepository
    {

        private IDigiBookDbContext context;
        
        public EmployeeRepository(IDigiBookDbContext context)
            :base(context)
        {
            this.context = context;
        }

        public Pagination<Employee> Retrieve(int pageNumber, int recordNumber, string key)
        {
            Pagination<Employee> result = new Pagination<Employee>
            {
                PageNumber = pageNumber,
                RecordNumber = recordNumber,
                TotalCount = this.context.Set<Employee>().Count()
            };

            if (pageNumber < 0)
            {
                result.Results = this.context.Set<Employee>()
                    .Skip(0)
                    .Take(10)
                    .OrderBy(c => c.LastName)
                    .ToList();
                return result;
            }
            if (recordNumber < 0)
            {
                result.Results = this.context.Set<Employee>()
                    .Skip(0)
                    .Take(10)
                    .OrderBy(c => c.LastName)
                    .ToList();
                return result;
            }
            if (string.IsNullOrEmpty(key))
            {
                result.Results = this.context.Set<Employee>()
                    .OrderBy(c => c.LastName)
                    .Skip(pageNumber)
                    .Take(recordNumber)
                    .ToList();
                return result;
            }
            else
            {
                result.Results = this.context.Set<Employee>()
                    .Where(c => c.FirstName.Contains(key) || c.LastName.Contains(key))
                    .Skip(pageNumber)
                    .Take(recordNumber)
                    .ToList()
                    .OrderBy(c => c.LastName);

                result.TotalCount = result.Results.Count();
                return result;
            }


        }
    }
}
