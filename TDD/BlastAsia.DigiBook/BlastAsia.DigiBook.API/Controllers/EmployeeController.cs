using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BlastAsia.DigiBook.Domain.Models.Employees;
using BlastAsia.DigiBook.Domain.Employees;
using BlastAsia.DigiBook.Domain.Employees.Services;
using BlastAsia.DigiBook.API.Utils;
using Microsoft.AspNetCore.JsonPatch;

namespace BlastAsia.DigiBook.API.Controllers
{
    [Produces("application/json")]
    [Route("api/Employee")]
    public class EmployeeController : Controller
    {
        private static List<Employee> employeeList = new List<Employee>();
        private IEmployeeRepository employeeRepo;
        private IEmployeeService employeeService;

        public EmployeeController(IEmployeeRepository employeeRepo, IEmployeeService employeeService)
        {
            this.employeeRepo = employeeRepo;
            this.employeeService = employeeService;
        }


        [HttpGet, ActionName("GetEmployees")]
        public IActionResult GetAllEmployee(Guid? id)
        {
            var result = new List<Employee>();

            if (id == null)
            {
                result.AddRange(this.employeeRepo.Retrieve());
            }
            else
            {
                var emp = this.employeeRepo.Retrieve(id.Value);
                result.Add(emp);
            }

            return Ok(result);
        }

        [HttpPost]
        public IActionResult PostEmployee([FromBody] Employee employee)
        {
            this.employeeRepo.Create(employee);
            var result = this.employeeService.Save(Guid.Empty, employee);
            

            return CreatedAtAction("GetEmployees", new { id = employee.Id, employee });
        }

        [HttpDelete]
        public IActionResult DeleteEmployee(Guid id)
        {
            this.employeeRepo.Delete(id);
            return NoContent();
        }

        [HttpPut]
        public IActionResult UpdateEmployee([FromBody]Employee employee, Guid id)
        {
            var existingEmployee = this.employeeRepo.Retrieve(id);
            existingEmployee.ApplyChanges(employee);
            this.employeeService.Save(id, existingEmployee);

            return Ok();
        }


        [HttpPatch]
        public IActionResult PatchEmployee([FromBody]JsonPatchDocument patchedDocuments, Employee employee)
        {
            if (patchedDocuments == null)
            {
                return BadRequest();
            }

            var employeeFound = this.employeeRepo.Retrieve(employee.Id);

            if (employeeFound == null) return NotFound();
            patchedDocuments.ApplyTo(employeeFound);
            this.employeeService.Save(employee.Id, employee);

            return Ok(employee);
        }
    }
}