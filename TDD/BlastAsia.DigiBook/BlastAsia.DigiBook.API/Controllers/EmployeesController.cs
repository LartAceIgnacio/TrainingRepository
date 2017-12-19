using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BlastAsia.DigiBook.Domain.Employees;
using BlastAsia.DigiBook.Domain.Models.Employees;
using Microsoft.AspNetCore.JsonPatch;

namespace BlastAsia.DigiBook.API.Controllers
{

    [Produces("application/json")]
    [Route("api/Employees")]
    public class EmployeesController : Controller
    {
        private readonly IEmployeeService employeeService;
        private readonly IEmployeeRepository employeeRepository;

        public EmployeesController(IEmployeeService employeeService,
           IEmployeeRepository employeeRepository)
        {
            this.employeeService = employeeService;
            this.employeeRepository = employeeRepository;
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

        [HttpDelete]
        public IActionResult DeleteEmployee(Guid id)
        {
            this.employeeRepository.Delete(id);

            return Ok();
        }

        [HttpPost]
        public IActionResult CreateEmployee(
           [FromBody]Employee employee)
        {
            var result = this.employeeService.Save(Guid.Empty, employee);

            return CreatedAtAction("GetEmployees", new { id = result.EmployeeId }, result);
        }

        [HttpPut]
        public IActionResult UpdateEmployee(
            [FromBody] Employee employee, Guid id)
        {
            this.employeeService.Save(id, employee);

            return Ok(employee);
        }
        [HttpPatch]
        public IActionResult PatchEmployee(
            [FromBody]JsonPatchDocument patchedEmployee, Guid id)
        {
            if (patchedEmployee == null)
            {
                return BadRequest();
            }

            var employee = employeeRepository.Retrieve(id);
            if (employee == null)
            {
                return NotFound();
            }

            patchedEmployee.ApplyTo(employee);
            employeeService.Save(id, employee);

            return Ok(employee);
        }
    }
}