using BlastAsia.DigiBook.Domain.Employees;
using BlastAsia.DigiBook.Domain.Models;
using BlastAsia.DigiBook.Domain.Models.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlastAsia.DigiBook.Infrastructure.Persistence.Repositories
{
    public class EmployeeRepository
        : RepositoryBase<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(IDigiBookDbContext context)
            :base(context)
        {

        }

        public PaginationClass<Employee> Retrieve(int pageNo, int numRec, string filterValue)
        {
            PaginationClass<Employee> result = new PaginationClass<Employee>();
            if (string.IsNullOrEmpty(filterValue))
            {
                result.Results = context.Set<Employee>().OrderBy(x => x.FirstName).ThenBy(x => x.LastName)
                    .Skip(pageNo).Take(numRec).ToList();

                if (result.Results.Count > 0)
                {
                    result.TotalRecords = context.Set<Employee>().Count();
                    result.PageNo = pageNo;
                    result.RecordPage = numRec;
                }

                return result;
            }
            else
            {
                result.Results = context.Set<Employee>().Where(x => x.FirstName.ToLower().Contains(filterValue.ToLower()) ||
                    x.LastName.ToLower().Contains(filterValue.ToLower()))
                    .OrderBy(x => x.FirstName).ThenBy(x => x.LastName)
                    .Skip(pageNo).Take(numRec).ToList();

                if (result.Results.Count > 0)
                {
                    result.TotalRecords = context.Set<Employee>().Where(x => x.FirstName.ToLower().Contains(filterValue.ToLower()) ||
                        x.LastName.ToLower().Contains(filterValue.ToLower())).Count();
                    result.PageNo = pageNo;
                    result.RecordPage = numRec;
                }

                return result;
            }
        }
    }
}
