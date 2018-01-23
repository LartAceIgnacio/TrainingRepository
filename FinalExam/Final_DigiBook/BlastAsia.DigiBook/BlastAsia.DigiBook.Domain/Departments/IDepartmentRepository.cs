using System;
using System.Collections.Generic;
using BlastAsia.DigiBook.Domain.Models.Departments;

namespace BlastAsia.DigiBook.Domain.Departments
{
    public interface IDepartmentRepository
    {
        Department Create(Department department);
        Department Retrieve(Guid departmentHead);
        IEnumerable<Department> Retrieve();
        Department Update(Guid departmentId, Department department);
        void Delete(Guid departmentId);
    }
}