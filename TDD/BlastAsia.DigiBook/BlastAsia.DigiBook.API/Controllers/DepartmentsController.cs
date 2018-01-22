using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlastAsia.DigiBook.API.Utils;
using BlastAsia.DigiBook.Domain.Departments;
using BlastAsia.DigiBook.Domain.Models.Departments;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace BlastAsia.DigiBook.API.Controllers
{
    [Produces("application/json")]
    [Route("api/Departments")]
    public class DepartmentsController : Controller
    {
        private IDepartmentService departmentService;
        private IDepartmentRepository departmentRepository;

        public DepartmentsController(IDepartmentService departmentService, IDepartmentRepository departmentRepository)
        {
            this.departmentService = departmentService;
            this.departmentRepository = departmentRepository;
        }

        [HttpGet, ActionName("GetDepartments")]
        public IActionResult GetDepartments(Guid? id)
        {
            var result = new List<Department>();
            if (id == null)
            {
                result.AddRange(this.departmentRepository.Retrieve());
            }
            else
            {
                result.Add(this.departmentRepository.Retrieve(id.Value));
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
                return CreatedAtAction("GetDepartments", new { id = department.DepartmentId }, department);
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
        public IActionResult UpdateDepartment([FromBody]Department department, Guid id)
        {
            try
            {
                if (department == null)
                {
                    return BadRequest();
                }
                var existingDepartment = this.departmentRepository.Retrieve(id);
                if (existingDepartment == null)
                {
                    return NotFound();
                }
                existingDepartment.ApplyChanges(department);
                var result = this.departmentService.Save(id, existingDepartment);
                return Ok(department);
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }

        [HttpPatch]
        public IActionResult PatchDepartment([FromBody]JsonPatchDocument patchedDepartment, Guid id)
        {
            if (patchedDepartment == null)
            {
                return BadRequest();
            }
            var department = departmentRepository.Retrieve(id);
            if (department == null)
            {
                return NotFound();
            }
            patchedDepartment.ApplyTo(department);
            departmentService.Save(id, department);
            return Ok(department);
        }
    }
}