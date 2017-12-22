using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlastAsia.DigiBook.Domain.Departments;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BlastAsia.DigiBook.Domain.Models.Departments;
using BlastAsia.DigiBook.API.Utils;

namespace BlastAsia.DigiBook.API.Controllers
{
    [Produces("application/json")]
    [Route("api/Departments")]
    public class DepartmentsController : Controller
    {
        private readonly IDepartmentService departmentService;
        private readonly IDepartmentRepository departmentRepository;

        public DepartmentsController(IDepartmentRepository departmentRepository, IDepartmentService departmentService)
        {
            this.departmentRepository = departmentRepository;
            this.departmentService = departmentService;
        }

        [HttpGet, ActionName("GetDepartments")]
        public IActionResult GetDepartments(Guid? id)
        {
            var result = new List<Department>();
            if(id == null)
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
        public IActionResult CreateDepartment([FromBody]Department department)
        {
            try
            {
                if (department == null)
                {
                    return BadRequest();
                }
                var result = this.departmentService.Save(Guid.Empty, department);

                return CreatedAtAction("GetDepartments", new { id = department.DepartmentId}, department);
            }

            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpDelete]
        public IActionResult DeleteDepartment(Guid id)
        {
            var departmentToDelete = this.departmentRepository.Retrieve(id);
            if (departmentToDelete == null)
            {
                return NotFound();
            }
            this.departmentRepository.Delete(id);
            return NoContent();
        }

        [HttpPut]
        public object UpdateDepartment(Department department, Guid id)
        {
            try
            {
                var existingDepartment = departmentRepository.Retrieve(id);
                
                existingDepartment.ApplyChanges(department);
                this.departmentService.Save(department.DepartmentId, department);

                return Ok(department);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}