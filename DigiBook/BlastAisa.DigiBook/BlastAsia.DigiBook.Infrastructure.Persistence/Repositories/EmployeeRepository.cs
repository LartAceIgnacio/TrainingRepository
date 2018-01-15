using BlastAsia.DigiBook.Domain.Employees;
using BlastAsia.DigiBook.Domain.Models.Employees;
using BlastAsia.DigiBook.Domain.Models.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlastAsia.DigiBook.Infrastructure.Persistence.Repositories
{
    public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
    {
        private IDigiBookDbContext context;
        public EmployeeRepository(IDigiBookDbContext context) : base(context)
        {
            this.context = context;
        }

        public Pagination<Employee> Retrieve(int pageNumber, int recordNumber, string searchKey)
        {
            Pagination<Employee> result = new Pagination<Employee>()
            {
                PageNumber = pageNumber < 0 ? 1 : pageNumber,
                RecordNumber = recordNumber < 0 ? 1 : recordNumber,
                TotalCount = this.context.Set<Employee>().Count()
            };

            if (pageNumber < 0)
            {
                result.Result = this.context.Set<Employee>().OrderBy(c => c.LastName)
                    .Skip(0).Take(10).ToList();

                return result;
            }
            if (recordNumber < 0)
            {
                result.Result = this.context.Set<Employee>().OrderBy(c => c.LastName)
                    .Skip(0).Take(10).ToList();

                return result;
            }
            if (string.IsNullOrEmpty(searchKey))
            {
                result.Result = this.context.Set<Employee>().OrderBy(c => c.LastName)
                   .Skip(pageNumber).Take(recordNumber).ToList();

                return result;
            }
            else
            {
                result.Result = this.context.Set<Employee>().Where(r => r.FirstName.Contains(searchKey) || r.LastName.Contains(searchKey))
                                                 .OrderBy(c => c.LastName)
                                                 .Skip(pageNumber)
                                                 .Take(recordNumber)
                                                 .ToList();

                result.TotalCount = result.Result.Count();

                return result;
            }

        }
    }
}
