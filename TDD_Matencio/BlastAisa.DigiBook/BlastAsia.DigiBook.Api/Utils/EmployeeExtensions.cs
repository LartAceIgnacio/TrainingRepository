using BlastAsia.DigiBook.Domain.Models.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlastAsia.DigiBook.Api.Utils
{
    public static class EmployeeExtensions
    {
        public static Employee ApplyChanges(this Employee employee,
           Employee from)
        {
            employee.firstName = from.firstName;
            employee.lastName = from.lastName;
            employee.mobilePhone = from.mobilePhone;
            employee.emailAddress = from.emailAddress;
            employee.photo = from.photo;
            employee.officePhone = from.officePhone;
            employee.extension = from.extension;

            return employee;
        }
    }
}
