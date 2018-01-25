using BlastAsia.DigiBook.Domain.Models;
using BlastAsia.DigiBook.Domain.Models.Employees;

namespace BlastAsia.DigiBook.Domain.Employees
{
    public interface IEmployeeRepository
        : IRepository<Employee>
    {
        Pagination<Employee> Retrieve(int pageNo, int numRec, string filterValue);
    }
}