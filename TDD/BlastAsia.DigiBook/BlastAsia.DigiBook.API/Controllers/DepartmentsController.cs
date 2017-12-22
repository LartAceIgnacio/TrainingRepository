using BlastAsia.DigiBook.Domain.Departments;
using BlastAsia.DigiBook.Domain.Models.Departments;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace BlastAsia.DigiBook.API.Controllers
{
    [Produces("application/json")]
    [Route("api/Contacts")]
    public class DepartmentsController : Controller
    {
        private readonly IDepartmentService departmentService;
        private readonly IDepartmentRepository departmentRepository;

        public DepartmentsController(IDepartmentService departmentService,
            IDepartmentRepository departmentRepository)
        {
            this.departmentService = departmentService;
            this.departmentRepository = departmentRepository;
        }

        [HttpGet, ActionName("GetDepartments")]
        public IActionResult GetDepartment(Guid? id)
        {
            var result = new List<Department>();
            if (id == null)
            {
                result.AddRange(this.departmentRepository.Retrieve());
            }
            else
            {
                var department = this.departmentRepository.Retrieve(id.Value);
                result.Add(department);
            }

           return Ok(result);
        }

        [HttpPost]
        public IActionResult CreateDepartment(
            [FromBody] Department department)
        {
            try
            {
                if (department == null)
                {
                    return BadRequest();
                }
                var result = this.departmentService.Save(Guid.Empty, department);

                return CreatedAtAction("GetDepartments",
                    new { id = department.DepartmentId }, result);
            }
            catch (Exception exc)
            {
                return BadRequest(exc.Message);
            }

        }


    }
}