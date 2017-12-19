using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BlastAsia.DigiBook.Domain.Models.Contacts;
using BlastAsia.DigiBook.Infrastructure.Persistence;
using BlastAsia.DigiBook.Domain.Contacts.Interfaces;
using BlastAsia.DigiBook.Domain.Contacts;
using Microsoft.AspNetCore.JsonPatch;
using BlastAsia.DigiBook.Api.Utils;

namespace BlastAsia.DigiBook.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Contacts")]
    public class ContactsController : Controller
    {
        private readonly IContactRepository contactRepository;
        private readonly IContactService contactService;
        public ContactsController(IContactService contactService, IContactRepository contactRepository)
        {
            this.contactRepository = contactRepository;
            this.contactService = contactService;

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
        public IActionResult CreateContact([FromBody] Contact contact)
        {
            var result = this.contactService.Save(Guid.Empty, contact);

            return CreatedAtAction("GetContacts", new { id = contact.ContactId }, contact);
        }

        [HttpDelete]
        public IActionResult DeleteContact(Guid id)
        {

            this.contactRepository.Delete(id);
            return NoContent();
        }
        [HttpPut]
        public IActionResult UpdateContact(
          [Bind("FirstName", "LastName", "MobilePhone", "StreetAddress", "CityAddress", 
            "ZipCode", "Country", "EmailAddress")] Contact contact, Guid id)
        {
            var existingContact = contactRepository.Retrieve(id);

            existingContact.ApplyChanges(contact);

            this.contactService.Save(id, existingContact);

            return Ok(contact);
        }
        [HttpPatch]
        public IActionResult PatchContact([FromBody] JsonPatchDocument patchContact, Guid id)
        {
            if(patchContact == null)
            {
                return BadRequest();
            }

            var contact = contactRepository.Retrieve(id);
            if(contact == null)
            {
                return NotFound();
            }

            patchContact.ApplyTo(contact);
            contactService.Save(id, contact);
            return Ok(contact);

        }

    }
}