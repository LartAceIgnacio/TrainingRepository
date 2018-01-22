using System;
using BlastAsia.DigiBook.Domain.Models.Departments;

namespace BlastAsia.DigiBook.Domain.Departments
{
    public interface IDepartmentService
    {
        Department Save(Guid departmentId, Department department);
    }
}