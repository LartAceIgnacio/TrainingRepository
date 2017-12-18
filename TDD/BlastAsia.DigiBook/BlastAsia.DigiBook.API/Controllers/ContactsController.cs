using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using BlastAsia.DigiBook.Domain.Models.Contacts;
using System.Linq;
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
        public IActionResult GetContacts(Guid? id)
        {
            var result = new List<Contact>();
            if (id == null)
            {

                result.AddRange(this.context.Contacts.ToList());
            }
            else
            {
                var contact = this.context.Contacts
                    .FirstOrDefault(c => c.ContactId == id);
                result.Add(contact);
            }


            return Ok(result);
        }
        [HttpPost]
        public IActionResult PostContact(
            [FromBody] Contact contact)
        {
            contact.ContactId = Guid.NewGuid();
            this.context.Contacts.Add(contact);
            this.context.SaveChanges();

            return CreatedAtAction("GetContacts", new { id = contact.ContactId }
           , contact);
        }

        [HttpDelete]
        public IActionResult DeleteContact(Guid id)
        {
            var contactToDelete = context.Contacts.Find(id);
            if (contactToDelete != null)
            {
                context.Contacts.Remove(contactToDelete);
                context.SaveChanges();
            }
            return Ok();
        }
        [HttpPut]
        public IActionResult UpdateContact(
            [FromBody] Contact contact ,Guid id)
        {
           
            this.context.Contacts.Update(contact);
                this.context.SaveChanges();
            
            return Ok(contact);
        }
    }
}