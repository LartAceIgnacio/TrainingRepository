using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BlastAsia.Digibook.Domain.Models.Contacts;
using BlastAsia.Digibook.Infrastracture.Persistence;
using BlastAsia.Digibook.Domain.Contacts;
using BlastAsia.Digibook.Infrastracture.Persistence.Repositories;
using Microsoft.AspNetCore.JsonPatch;
using BlastAsia.Digibook.API.Utils;

namespace BlastAsia.Digibook.API.Controllers
{
    [Produces("application/json")]
    [Route("api/Contacts")]
    public class ContactsController : Controller
    {
        private static List<Contact> contacts = new List<Contact>();
        private readonly IContactService contactService;
        private readonly IContactRepository contactRepository;


        public ContactsController(IContactService contactService, IContactRepository contactRepository)
        {
            this.contactService = contactService;
            this.contactRepository = contactRepository;
        }

        [HttpGet, ActionName("GetContacts")]
        public IActionResult GetContact(Guid? id)
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
            contact.ContactId = Guid.NewGuid();
            this.contactService.Save(Guid.Empty, contact);

            return CreatedAtAction("GetContacts", new { id = contact.ContactId }, contact);
        }

        [HttpDelete]
        public IActionResult DeleteContact(Guid id)
        {
            var contactToDelete = this.contactRepository.Retrieve(id);
            if (contactToDelete != null)
            {
                this.contactRepository.Delete(id);
            }
            return NoContent();
        }

        [HttpPut]
        public IActionResult UpdateContact(
            [FromBody] Contact contact, Guid id)
        {
            var existingContact = contactRepository.Retrieve(id);

            existingContact.ApplyChanges(contact);

            this.contactService.Save(id, existingContact);

            return Ok(contact);
        }

        [HttpPatch]
        public IActionResult PatchContact(
           [FromBody]JsonPatchDocument patchedContact, Guid id)
        {
            if (patchedContact == null)
            {
                return BadRequest();
            }
            var contact = contactRepository.Retrieve(id);
            if (contact == null)
            {
                return NotFound();
            }
            patchedContact.ApplyTo(contact);
            contactService.Save(id, contact);

            return Ok(contact);
        }

    }
}