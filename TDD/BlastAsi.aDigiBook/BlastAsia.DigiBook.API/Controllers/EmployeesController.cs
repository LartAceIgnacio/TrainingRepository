﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlastAsia.DigiBook.Domain.Employees;
using BlastAsia.DigiBook.Domain.Models.Employees;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace BlastAsia.DigiBook.API.Controllers
{
    [Produces("application/json")]
    [Route("api/Employees")]
    public class EmployeesController : Controller
    {
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
        public IActionResult CreateEmployee(
            [FromBody] Employee employee)
        {
            var result = this.employeeService.Save(Guid.Empty, employee);
            return CreatedAtAction("GetEmployees", new { id = result.EmployeeId }, result);
        }


        [HttpDelete]
        public IActionResult DeleteEmployee(Guid id)
        {
            this.employeeRepository.Delete(id);
            return NoContent();
        }

        [HttpPatch]
        public IActionResult PatchEmployee(
            [FromBody] JsonPatchDocument patchedEmployee, Guid id)
        {
            if(patchedEmployee == null)
            {
                return BadRequest();
            }

            var employee = this.employeeRepository.Retrieve(id);
            if(employee == null)
            {
                return NotFound();
            }

            patchedEmployee.ApplyTo(employee);
            employeeService.Save(id, employee);

            return Ok(employee);
        }

        [HttpPut]
        public IActionResult UpdateEmployee(
             [Bind("FirstName", "LastName", "MobilePhone", "EmailAddress",
                "OfficePhone", "Extension", "EmployeeId")] Employee employee, Guid id)
        {
            this.employeeRepository.Update(id, employee);
            return Ok(employee);
        }
    }
}