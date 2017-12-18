using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BlastAsia.DigiBook.Domain.Models.Contacts;
using BlastAsia.DigiBook.Infrastructure.Persistence;
using BlastAsia.DigiBook.Domain.Contacts;

namespace BlastAsia.DigiBook.API.Controllers
{
    [Produces("application/json")]
    [Route("api/Contacts")]
    public class ContactsController : Controller
    {
        private static List<Contact> contacts;
            //= new List<Contact>() {
            //     new Contact {
            //        ContactId = Guid.NewGuid(),
            //        Firstname = "Matt",
            //        Lastname = "Mendez",
            //        CityAddress = "QC"
            //    },
            //    new Contact {
            //        ContactId = Guid.NewGuid(),
            //        Firstname = "Em",
            //        Lastname = "Magadia",
            //        CityAddress = "QC"
            //    },
            //    new Contact {
            //        ContactId = Guid.NewGuid(),
            //        Firstname = "Chris",
            //        Lastname = "Manuel",
            //        CityAddress = "QC"
            //    }
            //};
        private DigiBookDbContext context;
        private IContactRepository c;

        public ContactsController(DigiBookDbContext/*IDigiBookDbContext*/ context)
        {
            this.context = context;
            //if (contacts.Count == 0)
            
        }
        

        [HttpGet, ActionName("GetContacts")]
        public IActionResult GetContactsById(Guid? id)
        {

            var result = new List<Contact>();
            if (id == null)
            {
                result.AddRange(this.context.Contacts.ToList());
            }
            else
            {
                var contact = this.context.Contacts.FirstOrDefault(c => c.ContactId == id);
                result.Add(contact);
            }
            
            return Ok(result);
        }

        [HttpPost]
        public IActionResult PostContact([FromBody] Contact contact)
        {
            contact.ContactId = Guid.NewGuid();
            this.context.Contacts.Add(contact);
            this.context.SaveChanges();

            return CreatedAtAction("GetContacts", new { id = contact.ContactId, contact });
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
        public IActionResult UpdateConntact([FromBody] Contact contact, Guid id)
        {
            
            this.context.Contacts.Update(contact);
            this.context.SaveChanges();

            return Ok(contact);
        }
    }
}