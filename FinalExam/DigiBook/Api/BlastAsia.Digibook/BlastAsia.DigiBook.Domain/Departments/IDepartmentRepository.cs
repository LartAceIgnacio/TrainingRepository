using BlastAsia.DigiBook.Domain.Models.Departments;
using System;

namespace BlastAsia.DigiBook.Domain.Departments
{
    public interface IDepartmentRepository
        :IRepository<Department>
    {
        //Department Create(Department department);
        //Department Retrieve(Guid id);
        //Department Update(Guid id, Department department);
    }
}