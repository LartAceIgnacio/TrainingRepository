using BlastAsia.DigiBook.Domain.Models.Employees;

namespace BlastAsia.DigiBook.API.Utils
{
    public static class EmployeeExtension
    {
        public static Employee ApplyChanges(
            this Employee employee
            , Employee form)
        {
            employee.EmailAddress = form.EmailAddress;
            employee.Extension = form.Extension;
            employee.FirstName = form.FirstName;
            employee.LastName = form.LastName;
            employee.MobilePhone = form.MobilePhone;
            employee.OfficePhone = form.OfficePhone;
            employee.Photo = form.Photo;

            return employee;
        }
    }
}