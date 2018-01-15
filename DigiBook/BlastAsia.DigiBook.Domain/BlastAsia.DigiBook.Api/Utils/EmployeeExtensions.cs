using BlastAsia.DigiBook.Domain.Models.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlastAsia.DigiBook.Api.Utils
{
    public static class EmployeeExtensions
    {
        public static Employee ApplyChanges(this Employee employee, Employee from)
        {
            employee.FirstName = from.FirstName;
            employee.LastName = from.LastName;
            employee.EmailAddress = from.EmailAddress;
            employee.Extension = from.Extension;
            employee.Photo = from.Photo;
            employee.MobilePhone = from.MobilePhone;
            employee.OfficePhone = from.OfficePhone;

            return employee;
        }
    }
}
