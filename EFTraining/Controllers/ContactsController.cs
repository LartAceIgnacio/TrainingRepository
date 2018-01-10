using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFTraining.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EFTraining.Controllers
{
    [Route("api/[controller]")]
    public class ContactsController : Controller
    {
        private DigiBookDbContext context;
        public ContactsController(DigiBookDbContext context)
        {
            this.context = context;
        }
        // GET api/values
        [HttpGet]
        public IActionResult Get()
        {
            var result = this.context.Contacts.ToList();
            return Ok(result);
        }

        // GET api/values/5
        [HttpGet("{id}/Appointments")]
        public IActionResult Get(Guid id)
        {
            var result = this.context.Contacts.Where(c => c.ContactId == id).Include(c => c.Appointments).ToList();
            return Ok(result);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
