using BlastAsia.DigiBook.Domain.Employees;
using BlastAsia.DigiBook.Domain.Models.Employees;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Infrastracture.Persistence.Repositories
{
    public class EmployeeRepository
        : RepositoryBase<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(IDigiBookDbContext context) 
            : base(context)
        {

        }
    }
}
