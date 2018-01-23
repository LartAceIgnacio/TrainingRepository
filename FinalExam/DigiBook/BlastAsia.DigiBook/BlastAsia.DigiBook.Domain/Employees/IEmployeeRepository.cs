

using BlastAsia.DigiBook.Domain.Models.Employees;
using BlastAsia.DigiBook.Domain.Models;
using System;

namespace BlastAsia.DigiBook.Domain.Employees
{
    public interface IEmployeeRepository
        : IRepository<Employee>
    {
        Pagination<Employee> Retrieve(int pageNo, int numRec, string filterValue);
    }

}