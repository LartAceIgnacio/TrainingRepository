using BlastAsia.DigiBook.Domain.Models.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlastAsia.DigiBook.API.Utils
{
    public static class EmployeeExtension
    {
        public static Employee ApplyNewChanges(this Employee oldEmployee, Employee newEmployee)
        {
            oldEmployee.EmailAddress = newEmployee.EmailAddress;
            oldEmployee.Extension = newEmployee.Extension;
            oldEmployee.FirstName = newEmployee.FirstName;
            oldEmployee.LastName = newEmployee.LastName;
            oldEmployee.MobilePhone = newEmployee.MobilePhone;
            oldEmployee.OfficePhone = newEmployee.OfficePhone;
            oldEmployee.Photo = newEmployee.Photo;
            oldEmployee.PhotoByte = newEmployee.PhotoByte;

            return oldEmployee;
        }
    }
}
