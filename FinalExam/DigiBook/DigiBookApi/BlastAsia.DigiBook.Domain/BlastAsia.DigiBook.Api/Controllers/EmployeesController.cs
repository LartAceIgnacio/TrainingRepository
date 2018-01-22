using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using BlastAsia.DigiBook.Domain.Employees;
using BlastAsia.DigiBook.Domain.Models.Employees;
using BlastAsia.DigiBook.Api.Utils;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Cors;
using BlastAsia.DigiBook.Domain.Models.Paginations;

namespace BlastAsia.DigiBook.Api.Controllers
{
    [EnableCors("day2app")]
    [Produces("application/json")]
    //[Route("api/Employees")]
    public class EmployeesController : Controller
    {
        private readonly IEmployeeRepository employeeRepository;
        private readonly IEmployeeService employeeService;
        public EmployeesController(IEmployeeService employeeService, IEmployeeRepository employeeRepository)
        {
            this.employeeRepository = employeeRepository;
            this.employeeService = employeeService;

        }

        [HttpGet]
        [Route("api/Employees/{pageNumber}/{recordNumber}/")]
        public IActionResult GetEmployees(int pageNumber, int recordNumber, string query)
        {
            try
            {
                var result = new Pagination<Employee>();
                result = this.employeeRepository.Retrieve(pageNumber, recordNumber, query);

                return Ok(result);
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }

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
        [Route("api/Employees")]
        public IActionResult CreateEmployee([FromBody] Employee employee)
        {
            try
            {
                if(employee == null)
                {
                    return BadRequest();
                }
                else
                {
                    var result = this.employeeService.Save(Guid.Empty, employee);

                    return CreatedAtAction("GetEmployees", new { id = employee.EmployeeId }, employee);
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpDelete]
        [Route("api/Employees")]
        public IActionResult DeleteEmployee(Guid id)
        {
            try
            {
                var retrieveEmployee = this.employeeRepository.Retrieve(id);
                if (retrieveEmployee == null)
                {
                    return BadRequest();
                }
                else
                {
                    this.employeeRepository.Delete(id);
                    return NoContent();
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }
           
        }
        [HttpPut]
        [Route("api/Employees")]
        public IActionResult UpdateEmployee(
          [FromBody] Employee employee , Guid id)
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
                    return BadRequest();
                }
                else
                {
                    existingEmployee.ApplyChanges(employee);

                    this.employeeService.Save(id, existingEmployee);

                    return Ok(employee);
                }
               
            }
            catch (Exception)
            {

                return BadRequest();
            }
            
        }
        [HttpPatch]
        [Route("api/Employees")]
        public IActionResult PatchEmployee([FromBody] JsonPatchDocument patchEmployee, Guid id)
        {
            try
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
            catch (Exception)
            {
                return BadRequest();
            }
            
        }

    }
}