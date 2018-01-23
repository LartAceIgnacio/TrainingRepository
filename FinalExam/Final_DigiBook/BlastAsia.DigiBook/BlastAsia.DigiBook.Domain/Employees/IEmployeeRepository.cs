using System;
using BlastAsia.DigiBook.Domain.Models.Employees;
using BlastAsia.DigiBook.Domain.Models.Records;

namespace BlastAsia.DigiBook.Domain.Employees
{
    public interface IEmployeeRepository:
        IRepository<Employee>
    {
        Record<Employee> Fetch(int pageNo, int numRec, string filterValue);
    }
}