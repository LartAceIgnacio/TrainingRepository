using System;
using BlastAsia.DigiBook.Domain.Models.Departments;
using System.Collections.Generic;

namespace BlastAsia.DigiBook.Domain.Departments
{
    public interface IDepartmentRepository
    {
        Department Create(Department department);
        Department Retrieve(Guid Id);
        Department Update(Guid Id, Department department);
        IEnumerable<Department> Retrieve();
        void Delete(Guid departmentId);
    }
}