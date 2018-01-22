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
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Authorization;
//using System.Web.Http.Routing;

namespace BlastAsia.DigiBook.API.Controllers
{
    [EnableCors("PrimeNgDemoApp")]
    [Produces("application/json")]
    [Route("api/Employee")]
    public class EmployeeController : Controller
    {
        private static List<Employee> employeeList = new List<Employee>();
        private IEmployeeRepository employeeRepo;
        private IEmployeeService employeeService;

        private IUrlHelperFactory urlHelperFactory;
        private IActionContextAccessor actionAccessor;

        private int PAGE_SIZE = 10;

        public EmployeeController(IEmployeeRepository employeeRepo, IEmployeeService employeeService
                                   , IUrlHelperFactory urlHelperFactory, IActionContextAccessor actionAccessor)
        {
            this.employeeRepo = employeeRepo;
            this.employeeService = employeeService;

            this.urlHelperFactory = urlHelperFactory;
            this.actionAccessor = actionAccessor;
        }

        [HttpGet, ActionName("GetEmployees")]
        public IActionResult GetEmployee(Guid? id, int page)
        {
            var result = new List<Employee>();

            var baseQuery = this.employeeRepo.Retrieve().OrderBy(o => o.Firstname);

            var PAGE_COUNT = baseQuery.Count();
            
            var TOTAL_PAGES = Math.Ceiling((double)PAGE_COUNT / PAGE_SIZE);
            
            
            if (id == null)
            {
                result = baseQuery.Skip(PAGE_SIZE * page)
                                   .Take(PAGE_SIZE)
                                   .ToList();
            }
            else
            {
                var emp = this.employeeRepo.Retrieve(id.Value);
                result.Add(emp);
            }

            return Ok(result);

        }


        [Route("draft"), ActionName("PagingEmployee")]
        public object Get(int page = 0)
        {

            var baseQuery = this.employeeRepo.Retrieve().OrderBy(o => o.Firstname);

            var PAGE_COUNT = baseQuery.Count();

            var TOTAL_PAGES = Math.Ceiling((double)PAGE_COUNT / PAGE_SIZE);

            var helper = this.urlHelperFactory.GetUrlHelper(this.actionAccessor.ActionContext);

            //var urlHelper = this.HttpContext.RequestServices.GetRequiredService<IUrlHelper>();

            var prevUrl = page > 0 ? helper.Action("PagingEmployee", "Employee", new { page = page - 1 }) : "";
            var nextUrl = page < TOTAL_PAGES - 1 ? helper.Action("PagingEmployee", "Employee", new { page = page + 1 }) : "";


            var results = baseQuery.Skip(PAGE_SIZE * page)
                                   .Take(PAGE_SIZE)
                                   .ToList();
            return new
            {
                TotalCount = PAGE_COUNT,
                TotalPage = TOTAL_PAGES,
                PrevPageUrl = prevUrl,
                NextPageUrl = nextUrl,
                Results = results
            };
        }


        [HttpPost]
        [Authorize]
        public IActionResult PostEmployee([FromBody] Employee employee)
        {

            try
            {
                var result = this.employeeService.Save(Guid.Empty, employee);


                return CreatedAtAction("GetEmployees", new { id = employee.Id, result });
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }

        [HttpDelete]
        [Authorize]
        public IActionResult DeleteEmployee(Guid id)
        {
            var result = this.employeeRepo.Retrieve(id);
            if (result == null) return NotFound();

            this.employeeRepo.Delete(id);
            return NoContent();
        }

        [HttpPut]
        [Authorize]
        public IActionResult UpdateEmployee([FromBody]Employee employee, Guid id)
        {
            try
            {
                var existingEmployee = this.employeeRepo.Retrieve(id);
                existingEmployee.ApplyChanges(employee);
                this.employeeService.Save(id, employee);

                return Ok();
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }


        [HttpPatch]
        [Authorize]
        public IActionResult PatchEmployee([FromBody]JsonPatchDocument patchedDocuments, Guid id)
        {
            if (patchedDocuments == null)
            {
                return BadRequest();
            }

            var employeeFound = this.employeeRepo.Retrieve(id);

            if (employeeFound == null) return NotFound();

            patchedDocuments.ApplyTo(employeeFound);
            this.employeeService.Save(id, employeeFound);

            return Ok(employeeFound);
        }
    }
}