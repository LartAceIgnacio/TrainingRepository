using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlastAsia.DigiBook.Domain.Departments;
using BlastAsia.DigiBook.Domain.Models.Departments;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlastAsia.DigiBook.API.Controllers
{
    [Produces("application/json")]
    [Route("api/Departments")]
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

        // GET: api/Departments
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
        
        // POST: api/Departments
        [HttpPost]
        public IActionResult CreateDepartment([FromBody]Department department)
        {
            try
            {
                if(department == null)
                {
                    return BadRequest();
                }
                var result = this.departmentService.Save(Guid.Empty, department);

                return CreatedAtAction("GetDeparments",
                    new { id= department.DepartmentId }, result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        // PUT: api/Departments/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
