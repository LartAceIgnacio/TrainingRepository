using System;
using System.Collections.Generic;
using System.Text;
using BlastAsia.DigiBook.Domain.Models.Departments;

namespace BlastAsia.DigiBook.Domain.Departments
{
    public class DepartmentService : IDepartmentService
    {
        private IDepartmentRepository departmentRepository;

        public DepartmentService(IDepartmentRepository departmentRepository)
        {
            this.departmentRepository = departmentRepository;
        }
        public Department Save(Department department)
        {
            if (string.IsNullOrEmpty(department.DepartmentName))
            {
                throw new DepartmentNameRequiredException("Department name is required."); 
            }

            departmentRepository.Retrieve(department.DeparmentId);

            departmentRepository.Save(department);

            return null;
        }
    }
}
