using BlastAsia.DigiBook.Domain.Employees;
using BlastAsia.DigiBook.Domain.Models.Employees;
using BlastAsia.DigiBook.Domain.Models.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlastAsia.DigiBook.Infrastracture.Persistence.Repositories
{
    public class EmployeeRepository
        : RepositoryBase<Employee>, IEmployeeRepository
    {
        private readonly IDigiBookDbContext context;

        public EmployeeRepository(IDigiBookDbContext context)
            : base(context)
        {
            this.context = context;
        }

        public Pagination<Employee> Retrieve(int pageNumber, int recordNumber, string keyWord)
        {
            Pagination<Employee> result = new Pagination<Employee>
            {
                PageNumber = pageNumber,
                RecordNumber = recordNumber,
                TotalCount = this.context.Set<Employee>().Count()
            };

            if (pageNumber < 0)
            {
                result.Result = this.context.Set<Employee>().Skip(0).Take(10).OrderBy(c => c.LastName).ToList();
                return result;
            }

            if (recordNumber < 0)
            {
                result.Result = this.context.Set<Employee>().Skip(0).Take(10).OrderBy(c => c.LastName).ToList();
                return result;
            }

            if (string.IsNullOrEmpty(keyWord))
            {
                result.Result = this.context.Set<Employee>().OrderBy(c => c.LastName)
                                                            .Skip(pageNumber)
                                                            .Take(recordNumber)
                                                            .ToList();
                return result;
            }
            else
            {
                result.Result = this.context.Set<Employee>().Where(r => r.FirstName.Contains(keyWord) || r.LastName.Contains(keyWord))
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
