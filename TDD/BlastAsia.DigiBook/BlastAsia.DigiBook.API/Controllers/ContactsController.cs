using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BlastAsia.DigiBook.Domain.Models.Contacts;
using BlastAsia.DigiBook.Infrastructure.Persistence;
using BlastAsia.DigiBook.Domain.Contacts;
using Microsoft.AspNetCore.JsonPatch;
using BlastAsia.DigiBook.API.Utils;

namespace BlastAsia.DigiBook.API.Controllers
{
    [Produces("application/json")]
    [Route("api/Contacts")]
    public class ContactsController : Controller
    {
        private static List<Contact> contacts;
        //   //= new List<Contact>() {
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
        private IContactService contactService;
        private IContactRepository contactRepository;

        public ContactsController(IContactService contactService, IContactRepository contactRepository)
        {
            this.contactService = contactService;
            this.contactRepository = contactRepository;

        }


        [HttpGet, ActionName("GetContacts")]
        public IActionResult GetContacts(Guid? id)
        {

            var result = new List<Contact>();
            if (id == null)
            {
                result.AddRange(this.contactRepository.Retrieve());
            }
            else
            {
                var contact = this.contactRepository.Retrieve(id.Value);
                result.Add(contact);
            }

            return Ok(result);
        }

        [HttpPost]
        public IActionResult PostContact([FromBody] Contact contact)
        {

            try
            {
                if (contact == null)
                {
                    return BadRequest();
                }

                var result = this.contactService.Save(Guid.Empty, contact);
                return CreatedAtAction("GetContacts", new { id = contact.ContactId, result });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpDelete]
        public IActionResult DeleteContact(Guid id)
        {

            var result = this.contactRepository.Retrieve(id);
            if (result == null) return NotFound();

            this.contactRepository.Delete(id);
            return NoContent();
        }

        [HttpPut]
        public IActionResult UpdateContact(/*[Bind("Firstname", "Lastname", "MobilePhone", "StreetAddress", "CityAddress",
            "ZipCode", "Country", "EmailAddress")]*/[FromBody] Contact contact, Guid id)
        {

            try
            {
                var existingEmployee = this.contactRepository.Retrieve(id);

                existingEmployee.ApplyChanges(contact);

                this.contactService.Save(id, contact);
                return Ok();
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }

        [HttpPatch]
        public IActionResult PatchContact([FromBody]JsonPatchDocument patchedContact, Guid id)
        {
            if (patchedContact == null)
            {
                return BadRequest();
            }

            var contact = contactRepository.Retrieve(id);

            if (contact == null) return NotFound();

            patchedContact.ApplyTo(contact);
            contactService.Save(id, contact);
            return Ok(contact);
        }
    }
}