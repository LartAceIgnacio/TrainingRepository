using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BlastAsia.Digibook.Domain.Models.Employees;
using BlastAsia.Digibook.Domain.Employees;
using Microsoft.AspNetCore.JsonPatch;
using System.IO;
using BlastAsia.Digibook.API.Utils;

namespace BlastAsia.Digibook.API.Controllers
{
    [Produces("application/json")]
    [Route("api/Employee")]
    public class EmployeeController : Controller
    {
        private static List<Employee> employees = new List<Employee>();
        private readonly IEmployeeService employeeService;
        private readonly IEmployeeRepository employeeRepository;

        public EmployeeController(IEmployeeService employeeService, IEmployeeRepository employeeRepository)
        {
            this.employeeService = employeeService;
            this.employeeRepository = employeeRepository;
        }

        [HttpGet, ActionName("GetEmployees")]
        public IActionResult GetEmployee(Guid? id)
        {
            var result = new List<Employee>();

            if(id == null)
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
            employee.Photo = new MemoryStream();
            this.employeeService.Save(employee);

            return CreatedAtAction("GetEmployees", new { id = employee.EmployeeId }, employee);
        }

        [HttpDelete]
        public IActionResult DeleteEmployee(Guid id)
        {
            var employee = this.employeeRepository.Retrieve(id);
            if (employee == null)
            {
                return BadRequest();
            }
            else
            {
                this.employeeRepository.Delete(id);
                return NoContent();
            }
        }

        [HttpPut]
        public IActionResult UpdateEmployee([FromBody] Employee employee, Guid id)
        {
            var existingEmployee = employeeRepository.Retrieve(id);

            employee.ApplyEmployeeChanges(existingEmployee);

            var result = this.employeeService.Save(existingEmployee);

            return Ok(result);
        }

        [HttpPatch]
        public IActionResult PatchEmployee([FromBody]JsonPatchDocument patchedEmployee, Guid id)
        {
            if (patchedEmployee == null)
            {
                return BadRequest();
            }
            var employee = this.employeeRepository.Retrieve(id);
            if (employee == null)
            {
                return NotFound();
            }
            patchedEmployee.ApplyTo(employee);
            this.employeeService.Save(employee);

            return Ok(employee);
        }
    }
}