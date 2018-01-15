using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BlastAsia.DigiBook.Domain.Employees;
using BlastAsia.DigiBook.Domain.Models.Employees;
using Microsoft.AspNetCore.JsonPatch;
using BlastAsia.DigiBook.API.Utils;
using Microsoft.AspNetCore.Cors;
using BlastAsia.DigiBook.Domain.Models;
using Microsoft.AspNetCore.Authorization;

namespace BlastAsia.DigiBook.API.Controllers
{
    [EnableCors("PrimeNgDemoApp")]
    [Produces("application/json")]
    //[Route("api/Employees")]
    public class EmployeesController : Controller
    {
        private readonly IEmployeeService employeeService;
        private readonly IEmployeeRepository employeeRepository;

        public EmployeesController(IEmployeeService employeeService, IEmployeeRepository employeeRepository)
        {
            this.employeeService = employeeService;
            this.employeeRepository = employeeRepository;
        }

        [HttpGet, ActionName("GetEmployeesWithPagination")]
        [Route("api/Employees/{page}/{record}")]
        public IActionResult GetEmployeesWithPagination(int page, int record, string filter)
        {
            var result = new PaginationResult<Employee>();
            try
            {
                result = this.employeeRepository.Retrieve(page, record, filter);
            }
            catch (Exception)
            {
                return BadRequest();
            }

            return Ok(result);
        }

        //[HttpGet, ActionName("GetEmployees")]
        [HttpGet, ActionName("GetEmployees")]
        [Route("api/Employees")]
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
        [Authorize]
        [Route("api/Employees")]
        public IActionResult CreateEmployee(
            [FromBody]
            Employee employee)
        {
            try
            {
                if (employee == null)
                {
                    return BadRequest();
                }

                var result = this.employeeService.Save(Guid.Empty, employee);

                return CreatedAtAction("GetEmployees", new { id = employee.EmployeeId }, employee);
            }

            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpDelete]
        [Authorize]
        [Route("api/Employees")]
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
        [Authorize]
        [Route("api/Employees")]
        public IActionResult UpdateEmployee(
            [FromBody]
            Employee employee, Guid id)
        {
            try
            {
                if (employee == null)
                {
                    return BadRequest();
                }

                var existingEmployee = employeeRepository.Retrieve(id);
                if (existingEmployee == null)
                {
                    return NotFound();
                }

                existingEmployee.ApplyChanges(employee);

                var result = this.employeeService.Save(id, existingEmployee);

                return Ok(employee);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPatch]
        [Authorize]
        [Route("api/Employees")]
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
