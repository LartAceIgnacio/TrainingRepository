using BlastAsia.DigiBook.Domain.Models.Employees;

namespace BlastAsia.DigiBook.Domain.Employees.Services
{
    public interface IEmployeeService
    {
        Employee Save(Employee employee);
    }
}