using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BlastAsia.DigiBook.Domain.Models.Contacts;
using BlastAsia.DigiBook.Domain.Contacts;
using Microsoft.AspNetCore.JsonPatch;
using BlastAsia.DigiBook.API.Utils;

namespace BlastAsia.DigiBook.API.Controllers
{
    [Produces("application/json")]
    [Route("api/Contacts")]
    public class ContactsController : Controller
    {
        private static List<Contact> contacts = new List<Contact>();
        private readonly IContactService contactService;
        private readonly IContactRepository contactRepository;

        public ContactsController(IContactRepository contactRepository, IContactService contactService)
        {
            this.contactRepository = contactRepository;
            this.contactService = contactService;
        }

        [HttpGet, ActionName("GetContacts")]
        public IActionResult GetContacts(Guid? id)
        {
            var result = new List<Contact>();
            if (id == null) {
                result.AddRange(this.contactRepository.Retrieve());
            }
            else {
                var contact = this.contactRepository.Retrieve(id.Value);
                result.Add(contact);
            }

            return Ok(result);
        }

        [HttpPost]
        public IActionResult CreateContact(
            [FromBody] Contact contact)
        {
            var result = this.contactService.Save(Guid.Empty, contact);

            return CreatedAtAction("GetContacts", new { id = contact.ContactId}, contact);
        }

        [HttpDelete]
        public IActionResult DeleteContact(Guid id)
        {
            var contactToDelete = this.contactRepository.Retrieve(id);
            if (contactToDelete != null) {
                this.contactRepository.Delete(id);
                return NoContent();
            }
            return NotFound();
        }

        [HttpPut]
        public IActionResult UpdateContact(
            [Bind("FirstName", "LastName", "MobilePhone", "StreetAddress", "CityAddress", "ZipCode", "Country", "EmailAddress")] Contact contact, Guid id)
        {
            var oldContact = this.contactRepository.Retrieve(id);
            if(oldContact == null) {
                return NotFound();
            }

            oldContact.ApplyNewChanges(contact);

            var result = this.contactService.Save(id, contact);

            return Ok(oldContact);
        }

        [HttpPatch]
        public IActionResult PatchContact([FromBody]JsonPatchDocument patchedContact, Guid id)
        {
            if(patchedContact == null) {
                return BadRequest();
            }

            var contact = contactRepository.Retrieve(id);
            if(contact == null) {
                return NotFound();
            }

            patchedContact.ApplyTo(contact);
            contactService.Save(id, contact);

            return Ok(contact);
        }
    }
}