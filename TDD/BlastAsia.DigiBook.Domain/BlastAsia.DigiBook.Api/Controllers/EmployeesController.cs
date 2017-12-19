using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BlastAsia.DigiBook.Domain.Employees;
using BlastAsia.DigiBook.Domain.Models.Employees;
using BlastAsia.DigiBook.Api.Utils;
using Microsoft.AspNetCore.JsonPatch;

namespace BlastAsia.DigiBook.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Employees")]
    public class EmployeesController : Controller
    {
        private readonly IEmployeeRepository employeeRepository;
        private readonly IEmployeeService employeeService;
        public EmployeesController(IEmployeeService employeeService, IEmployeeRepository employeeRepository)
        {
            this.employeeRepository = employeeRepository;
            this.employeeService = employeeService;

        }
        [HttpGet, ActionName("GetEmployees")]
        public IActionResult GetEmployees(Guid? id)
        {
            var result = new List<Employee>();
            if (id == null)
            {

                result.AddRange(this.employeeRepository.Retrieve());
            }
            else
            {
                var employee = this.employeeRepository.Retrieve(id.Value);
                result.Add(employee);
            }

            return Ok(result);
        }

        [HttpPost]
        public IActionResult EmployeeEmployee([FromBody] Employee employee)
        {
            var result = this.employeeService.Save(Guid.Empty, employee);

            return CreatedAtAction("GetEmployees", new { id = employee.EmployeeId }, employee);
        }

        [HttpDelete]
        public IActionResult DeleteEmployee(Guid id)
        {

            this.employeeRepository.Delete(id);
            return NoContent();
        }
        [HttpPut]
        public IActionResult UpdateEmployee(
          [Bind("FirstName", "LastName", "MobilePhone", "StreetAddress", "CityAddress",
            "ZipCode", "Country", "EmailAddress")] Employee employee , Guid id)
        {
            var existingEmployee = employeeRepository.Retrieve(id);

            existingEmployee.ApplyChanges(employee);

            this.employeeService.Save(id, existingEmployee);

            return Ok(employee);
        }
        [HttpPatch]
        public IActionResult PatchEmployee([FromBody] JsonPatchDocument patchEmployee, Guid id)
        {
            if (patchEmployee == null)
            {
                return BadRequest();
            }

            var employee = employeeRepository.Retrieve(id);
            if (employee == null)
            {
                return NotFound();
            }

            patchEmployee.ApplyTo(employee);
            employeeService.Save(id, employee);
            return Ok(employee);

        }

    }
}