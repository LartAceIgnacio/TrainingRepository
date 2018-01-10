using BlastAsia.DigiBook.Domain.Models.Employees;
using System;
using System.Collections.Generic;

namespace BlastAsia.DigiBook.Domain.Employees
{
    public interface IEmployeeRepository
        : IRepository<Employee>
    {
        IEnumerable<Employee> Retrieve(int page, int record, string filter);
    }
}