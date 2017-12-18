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
        private static List<Contact> contacts 
            = new List<Contact>();

        private readonly DigiBookDbContext context;

        public ContactsController(DigiBookDbContext context)
        {
            this.context = context;

            //if (contacts.Count == 0)
            //{
            //    contacts.Add(
            //    new Contact
            //    {
            //        ContactId = Guid.NewGuid(),
            //        FirstName = "Abbie",
            //        LastName = "Olarte",
            //        MobilePhone = "09981642039",
            //        StreetAddress = "#9 Kakawati Street, Pangarap Village",
            //        CityAddress = "Caloocan City",
            //        ZipCode = 1427,
            //        Country = "Philippines",
            //        EmailAddress = "abbieolarte@yahoo.com",
            //        IsActive = false,
            //        DateActivated = new Nullable<DateTime>()
            //    }
            //        );
            //    contacts.Add(
            //        new Contact
            //        {
            //            ContactId = Guid.NewGuid(),
            //            FirstName = "Blanche",
            //            LastName = "Olarte",
            //            MobilePhone = "09981642039",
            //            StreetAddress = "#9 Kakawati Street, Pangarap Village",
            //            CityAddress = "Caloocan City",
            //            ZipCode = 1427,
            //            Country = "Philippines",
            //            EmailAddress = "abbieolarte@gmail.com",
            //            IsActive = false,
            //            DateActivated = new Nullable<DateTime>()
            //        }
            //            );
            //    contacts.Add(
            //        new Contact
            //        {
            //            ContactId = Guid.NewGuid(),
            //            FirstName = "Angela",
            //            LastName = "Olarte",
            //            MobilePhone = "09981642039",
            //            StreetAddress = "#9 Kakawati Street, Pangarap Village",
            //            CityAddress = "Caloocan City",
            //            ZipCode = 1427,
            //            Country = "Philippines",
            //            EmailAddress = "abbieolarte@gmail.com",
            //            IsActive = false,
            //            DateActivated = new Nullable<DateTime>()
            //        }
            //           );
            //}
        }

        [HttpGet, ActionName("GetContacts")]
        public IActionResult GetContactById(Guid? id)
        {
            var result = new List<Contact>();
            if(id == null)
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
        public IActionResult PostContact([FromBody] Contact contact)
        {
            contact.ContactId = Guid.NewGuid();
            this.context.Contacts.Add(contact);
            this.context.SaveChanges();

            return CreatedAtAction("GetContacts", new { id = contact.ContactId }, contact);
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
        public IActionResult UpdateContact([FromBody] Contact contact, Guid id)
        {
            this.context.Contacts.Update(contact);
            this.context.SaveChanges();

            return Ok(contact);
        }
    }
}