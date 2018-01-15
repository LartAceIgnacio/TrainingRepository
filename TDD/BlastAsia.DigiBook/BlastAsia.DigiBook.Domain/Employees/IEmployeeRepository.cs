using System;
using BlastAsia.DigiBook.Domain.Models;
using BlastAsia.DigiBook.Domain.Models.Employees;

namespace BlastAsia.DigiBook.Domain.Employees
{
    public interface IEmployeeRepository
        : IRepository<Employee>
    {
        PaginationResult<Employee> Retrieve(int page, int record, string filter);
    }
}