using BlastAsia.DigiBook.Domain.Models.Departments;
using System;

namespace BlastAsia.DigiBook.Domain.Departments
{
    public interface IDepartmentService
    {
        Department Save(Guid id, Department department);
    }
}