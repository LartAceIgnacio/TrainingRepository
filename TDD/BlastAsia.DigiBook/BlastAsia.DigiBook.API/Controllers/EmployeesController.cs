using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BlastAsia.DigiBook.Domain.Employees;
using BlastAsia.DigiBook.Domain.Models.Employees;
using Microsoft.AspNetCore.JsonPatch;
using BlastAsia.DigiBook.API.Utils;
using Microsoft.AspNetCore.Cors;

namespace BlastAsia.DigiBook.API.Controllers
{
    [EnableCors("DayTwoApp")]
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

        [Route("api/Employees/{id?}")]
        [HttpGet, ActionName("GetEmployees")]
        public IActionResult GetEmployees(Guid? id)
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

        [Route("api/Employees/{page}/{record}")]
        [HttpGet, ActionName("GetEmployees")]
        public IActionResult GetEmployeesWithPagination(int page, int record, string filter)
        {
            var result = new List<Employee>();
            
            result.AddRange(this.employeeRepository.Retrieve(page,record,filter));
            

            return Ok(result);
        }

        [Route("api/Employees")]
        [HttpPost]
        public IActionResult CreateEmployee([FromBody] Employee employee)
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
        [Route("api/Employees/{id}")]
        [HttpDelete]
        public IActionResult DeleteEmployee(Guid id)
        {
            var foundDeleteId = employeeRepository.Retrieve(id);
            if (foundDeleteId == null)
            {
                return NotFound();
            }

            this.employeeRepository.Delete(id);

            return NoContent();
        }
        [Route("api/Employees/{id}")]
        [HttpPut]
        public IActionResult UpdateEmployee(
            [FromBody] Employee employee, Guid id)
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

            this.employeeService.Save(id, employee);

            return Ok(employee);
        }
        [Route("api/Employees/{id}")]
        [HttpPatch]
        public IActionResult PatchEmployee(
            [FromBody] JsonPatchDocument patchedEmployee, Guid id)
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