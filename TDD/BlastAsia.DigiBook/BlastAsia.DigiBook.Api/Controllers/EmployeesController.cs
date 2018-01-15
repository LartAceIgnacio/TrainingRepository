﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlastAsia.DigiBook.Domain.Employees;
using BlastAsia.DigiBook.Domain.Models.Employees;
using BlastAsia.DigiBook.Api.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using BlastAsia.DigiBook.Domain.Models.Records;

namespace BlastAsia.DigiBook.Api.Controllers
{
    [EnableCors("DemoApp")]
    [Produces("application/json")]
    [Route("api/Employees")]
    public class EmployeesController : Controller
    {
        private readonly IEmployeeService employeeService;
        private readonly IEmployeeRepository employeeRepository;

        public EmployeesController(IEmployeeRepository employeeRepository, IEmployeeService employeeService)
        {
            this.employeeRepository = employeeRepository;
            this.employeeService = employeeService;
        }

        [HttpGet, ActionName("GetEmployeesRecord")]
        [Route("{page}/{record}")]
        public IActionResult GetEmployeesWithPagination(int page, int record, string filter)
        {
            var result = new Record<Employee>();
            try
            {
                result = this.employeeRepository.Fetch(page, record, filter);
            }
            catch (Exception)
            {
                return BadRequest();
            }

            return Ok(result);
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
        public IActionResult CreateEmployee( [FromBody] Employee employee)
        {
            try
            {
                if (employee == null)
                {
                    return BadRequest();
                }
                var result = this.employeeService.Save(Guid.Empty, employee);

                return CreatedAtAction("GetEmployees", new { id = employee.EmployeeId }, result);
            }
            catch(Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpDelete]
        public IActionResult DeleteEmployee(Guid id)
        {
            var deletedEmployee = employeeRepository.Retrieve(id);
            if (deletedEmployee == null)
            {
                return NotFound();
            }
            this.employeeRepository.Delete(id);

            return Ok();
        }

        [HttpPut]
        public IActionResult UpdateEmployee(
            [FromBody] Employee modifiedEmployee, Guid id)
        {
            var employee = employeeRepository.Retrieve(id);
            if (employee == null)
            {
                return BadRequest();
            }
            employee.ApplyChanges(modifiedEmployee);
            employeeService.Save(id, employee);
            return Ok(employee);
        }

        [HttpPatch]
        public IActionResult PatchEmployee([FromBody]JsonPatchDocument patchedEmployee, Guid id)
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