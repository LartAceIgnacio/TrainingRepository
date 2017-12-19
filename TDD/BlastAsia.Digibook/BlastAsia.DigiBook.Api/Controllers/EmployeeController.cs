using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BlastAsia.DigiBook.Domain.Models.Employees;
using BlastAsia.DigiBook.Domain.Employees;
using BlastAsia.DigiBook.Api.Utils;
using Microsoft.AspNetCore.JsonPatch;

namespace BlastAsia.DigiBook.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Employee")]
    public class EmployeeController : Controller
    {
        private static List<Employee> employees = new List<Employee>();

        private readonly IEmployeeRepository employeeRepository;
        private readonly IEmployeeService employeeService;
        public EmployeeController(IEmployeeRepository employeeRepository, IEmployeeService employeeService)
        {
            this.employeeRepository = employeeRepository;
            this.employeeService = employeeService;
        }


        [HttpGet, ActionName("GetEmployees")]
        public IActionResult GetEmployee(Guid? id)
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
        public IActionResult CreateEmployee([FromBody] Employee employee)
        {
            var result = this.employeeService.Save(Guid.Empty, employee);
            return CreatedAtAction("GetEmployees", new { id = employee.EmployeeId }, result);
        }

        [HttpDelete]
        public IActionResult DeleteEmployee(Guid id)
        {
            var employeeToDelete = employeeRepository.Retrieve(id);

            if (employeeToDelete == null)
            {
                return NotFound();
            }

            employeeRepository.Delete(id);
            return NoContent();
        }

        [HttpPut]
        public IActionResult UpdateEmployee(
            [Bind("FirstName", "LastName", "MobilePhone", "EmailAddress", "PhotoUrl", "OfficePhone", "Extension")]
                Employee employee,
                    Guid id
            )
        {
            var employeeToUpdate = employeeRepository.Retrieve(id);

            if (employeeToUpdate == null)
            {
                return NotFound();
            }

            employeeToUpdate.ApplyChanges(employee);

            var result = employeeService.Save(id, employeeToUpdate);
            return Ok(result);

        }

        [HttpPatch]
        public IActionResult PatchEmployee([FromBody]JsonPatchDocument patchedEmployee, Guid id)
        {
            if (patchedEmployee == null)
            {
                return BadRequest();
            }

            var contact = employeeRepository.Retrieve(id);
            if (contact == null)
            {
                return NotFound();
            }

            patchedEmployee.ApplyTo(contact);
            employeeService.Save(id, contact);

            return Ok(contact);
        }
    }
}