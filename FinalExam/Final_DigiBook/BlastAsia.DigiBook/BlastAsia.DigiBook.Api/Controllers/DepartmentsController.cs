
using System.Collections.Generic;

using BlastAsia.DigiBook.Domain.Departments;

using BlastAsia.DigiBook.Domain.Models.Departments;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace BlastAsia.DigiBook.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Departments")]
    public class DepartmentsController : Controller
    {
        private readonly IDepartmentRepository departmentRepository;
        private readonly IDepartmentService departmentService;

        public DepartmentsController(IDepartmentRepository departmentRepository, IDepartmentService departmentService)
        {
            this.departmentRepository = departmentRepository;
            this.departmentService = departmentService;
        }
        // GET: api/Departments
        [HttpGet]
        public OkObjectResult Get()
        {
            var result = new List<Department>();
            result.AddRange(this.departmentRepository.Retrieve());
            return Ok(result);
             
        }

        // GET: api/Departments/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }
        
        // POST: api/Departments
        [HttpPost]
        public void Post([FromBody]string value)
        {
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
