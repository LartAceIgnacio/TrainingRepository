using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlastAsia.DigiBook.Domain.Employees;
using BlastAsia.DigiBook.Domain.Models.Employees;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using BlastAsia.DigiBook.API.Utils;

namespace BlastAsia.DigiBook.API.Controllers
{
    [Produces("application/json")]
    [Route("api/Employees")]
    public class EmployeesController : Controller
    {
        public static List<Employee> employee = new List<Employee>();
        private readonly IEmployeeService employeeService;
        private readonly IEmployeeRepository employeeRepository;

        public EmployeesController(IEmployeeService employeeService, IEmployeeRepository employeeRepository)
        {
            this.employeeService = employeeService;
            this.employeeRepository = employeeRepository;
        }

        [HttpGet, ActionName("GetEmployees")]
        public IActionResult GetEmployees(Guid? id)
        {
            var result = new List<Employee>();
            if (id == null) {
                result.AddRange(this.employeeRepository.Retrieve());
            }
            else {
                var employee = this.employeeRepository.Retrieve(id.Value);
                result.Add(employee);
            }

            return Ok(result);
        }

        [HttpPost]
        public IActionResult CreateEmployee([FromBody] Employee employee)
        {
            try {
                if(employee == null) {
                    return BadRequest();
                }

                var result = this.employeeService.Save(Guid.Empty, employee);

                return CreatedAtAction("GetEmployees", new { id = employee.EmployeeId }, employee);
            }
            catch (Exception) {
                return BadRequest();
            }
        }

        [HttpDelete]
        public IActionResult DeleteEmployee(Guid id)
        {
            var employeeToDelete = this.employeeRepository.Retrieve(id);
            if(employeeToDelete != null) {
                this.employeeRepository.Delete(id);
                return NoContent();
            }

            return NotFound();
        }

        [HttpPut]
        public IActionResult UpdateEmployee(
            [FromBody] Employee employee, Guid id)
        {
            try {
                if(employee == null) {
                    return BadRequest();
                }

                var oldEmployee = this.employeeRepository.Retrieve(id);
                if (oldEmployee == null) {
                    return NotFound();
                }
                oldEmployee.ApplyNewChanges(employee);

                var result = this.employeeService.Save(id, oldEmployee);

                return Ok(oldEmployee);
            }
            catch (Exception) {
                return BadRequest();
            }
        }

        [HttpPatch]
        public IActionResult PatchEmployee([FromBody]JsonPatchDocument patchedEmployee, Guid id)
        {
            try {
                if (patchedEmployee == null) {
                    return BadRequest();
                }

                var employee = employeeRepository.Retrieve(id);
                if (employee == null) {
                    return NotFound();
                }

                patchedEmployee.ApplyTo(employee);
                employeeService.Save(id, employee);

                return Ok(employee);
            }
            catch (Exception) {
                return BadRequest();
            }
        }
    }
}