using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BlastAsia.DigiBook.Domain.Models.Contacts;
using BlastAsia.DigiBook.Infrastructure.Persistence;

namespace BlastAsia.DigiBook.API.Controllers
{
    [Produces("application/json")]
    [Route("api/Contacts")]
    public class ContactsController : Controller
    {
        private static List<Contact> contacts = new List<Contact>();
        private readonly DigiBookDbContext context;
        public ContactsController(DigiBookDbContext context)
        {
            this.context = context;

        }
        [HttpGet, ActionName("GetContacts")]
        public IActionResult GetContact(Guid? id)
        {
            var result = new List<Contact>();
            if (id == null)
            {
                result.AddRange(this.context.Contact.ToList());
            }
            else
            {
                var contact = this.context.Contact
               .FirstOrDefault(c => c.ContactId == id);

                result.Add(contact);
            }

            return Ok(result);
        }
        [HttpPost]
        public IActionResult PostContact([FromBody] Contact contact)
        {
            contact.ContactId = Guid.NewGuid();
            this.context.Add(contact);
            this.context.SaveChanges();

            return CreatedAtAction("GetContacts", new { id = contact.ContactId }, contact);
        }
        [HttpDelete]
        public IActionResult DeleteContact(Guid id)
        {
            var contactToDelete = context.Contact.Find(id);
            if (contactToDelete != null)
            {
                context.Contact.Remove(contactToDelete);
                context.SaveChanges();
            }
            return Ok();
        }

        [HttpPut]
        public IActionResult UpdateContact([FromBody] Contact contact, Guid id)
        {
            this.context.Contact.Update(contact);
            this.context.SaveChanges();

            return Ok(contact);
        }
    }
}
