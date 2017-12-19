using BlastAsia.DigiBook.Domain.Models.Departments;

namespace BlastAsia.DigiBook.Domain.Departments
{
    public interface IDepartmentRepository
    {
        Department Create(Department department);
    }
}