using BlastAsia.DigiBook.Domain.Employees;
using BlastAsia.DigiBook.Domain.Models.Employees;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace BlastAsia.DigiBook.Infrastructure.Persistence.Repositories
{
    public class EmployeeRepository
        :RepositoryBase<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(IDigiBookDbContext context)
            :base(context)
        {

        }
        public IEnumerable<Employee> Retrieve(int pageNo, int numRec, string filterValue)
        {
            List<Employee> result = new List<Employee>();
            if (string.IsNullOrEmpty(filterValue))
            {
                result = context.Set<Employee>().OrderBy(x => x.FirstName).ThenBy(x => x.LastName)
                    .Skip(pageNo).Take(numRec).ToList();

                if (result.Count > 0)
                {
                    result[0].TotalRecords = context.Set<Employee>().Count();

                }

                return result;
            }
            else
            {
                result = context.Set<Employee>().Where(x => x.FirstName.ToLower().Contains(filterValue.ToLower()) ||
                    x.LastName.ToLower().Contains(filterValue.ToLower()))
                    .OrderBy(x => x.FirstName).ThenBy(x => x.LastName)
                    .Skip(pageNo).Take(numRec).ToList();

                if (result.Count > 0)
                {
                    result[0].TotalRecords = context.Set<Employee>().Where(x => x.FirstName.ToLower().Contains(filterValue.ToLower()) ||
                        x.LastName.ToLower().Contains(filterValue.ToLower())).Count();
                }

                return result;
            }
        }
    }
}
