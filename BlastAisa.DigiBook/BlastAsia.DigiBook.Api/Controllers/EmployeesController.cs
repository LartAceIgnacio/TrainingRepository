using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BlastAsia.DigiBook.Domain.Models.Employees;
using BlastAsia.DigiBook.Domain.Employees;
using Microsoft.AspNetCore.JsonPatch;
using BlastAsia.DigiBook.Api.Utils;
using Microsoft.AspNetCore.Cors;

namespace BlastAsia.DigiBook.Api.Controllers
{
    [EnableCors("day2app")]
    [Produces("application/json")]
    [Route("api/Employees")]
    public class EmployeesController : Controller
    {
        private static List<Employee> employees = new List<Employee>();

        private readonly IEmpolyeeService employeeService;
        private readonly IEmployeeRepository employeeRepository;

        public EmployeesController(IEmpolyeeService empolyeeService,
            IEmployeeRepository employeeRepository)
        {
            this.employeeService = empolyeeService;
            this.employeeRepository = employeeRepository;
        }

        [HttpGet]
        public IActionResult GetEmployees(Guid? id)
        {
          
                var result = new List<Employee>();
                if (id == null)
                {
                    result.AddRange(this.employeeRepository.Retreive());
                }
                else
                {
                    var employee = this.employeeRepository.Retrieve(id.Value);
                    result.Add(employee);
                }

                return Ok(result);
           
        }

        [HttpPost]
        public IActionResult CreateEmployee(
            [FromBody] Employee employee)
        {
            try
            {
                if (employee == null)
                {
                    return BadRequest();
                }

                var result = this.employeeService.Save(Guid.Empty, employee);

                return CreatedAtAction("GetEmployees",
                    new { id = employee.EmployeeId }, result);
            }

            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpDelete]
        public IActionResult DeleteEmployee(Guid id)
        {
            var employeeToDelete = this.employeeRepository.Retrieve(id);
            if (employeeToDelete == null)
            {
                return NotFound();
            }
            this.employeeRepository.Delete(id);
            return NoContent();
        }

        [HttpPut]
        public IActionResult UpdateEmployee(
            [FromBody] Employee employee, Guid id)
        {
            try
            {
                if (employee == null)
                {
                    return BadRequest();
                }

                var oldEmployee = this.employeeRepository.Retrieve(id);
                if (oldEmployee == null)
                {
                    return NotFound();
                }

                oldEmployee.ApplyChanges(employee);

                var result = this.employeeService.Save(id, employee);

                return Ok(oldEmployee);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPatch]
        public IActionResult PatchEmployee(
            [FromBody]JsonPatchDocument patchedEmployee, Guid id)
        {
            try
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
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}