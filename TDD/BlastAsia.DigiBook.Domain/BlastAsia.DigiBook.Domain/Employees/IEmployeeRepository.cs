using BlastAsia.DigiBook.Domain.Models;
using BlastAsia.DigiBook.Domain.Models.Employees;
using System;
using System.Collections.Generic;

namespace BlastAsia.DigiBook.Domain.Employees
{
    public interface IEmployeeRepository: IRepository<Employee>
    {
        PaginationResult<Employee> Retrieve(int pageNo, int numRec, string filterValue);
    }
}