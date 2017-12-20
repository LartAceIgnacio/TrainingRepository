using BlastAsia.Digibook.Domain.Employees;
using BlastAsia.Digibook.Domain.Models.Employees;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.Digibook.Infrastracture.Persistence.Repositories
{
    public class EmployeeRepository: RepositoryBase<Employee>,IEmployeeRepository
    {
        public EmployeeRepository(IDigiBookDbContext context) : base(context)
        {
        }
    }
}
