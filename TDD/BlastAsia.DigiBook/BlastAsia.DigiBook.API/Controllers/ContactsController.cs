using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlastAsia.DigiBook.Api.Utils;
using BlastAsia.DigiBook.Domain.Contacts;
using BlastAsia.DigiBook.Domain.Models.Contacts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace BlastAsia.DigiBook.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Contacts")]
    public class ContactsController : Controller
    {
        private readonly IContactService contactService;
        private readonly IContactRepository contactRepository;

        public ContactsController(
            IContactRepository contactRepository
            , IContactService contactService)
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
        public IActionResult CreateContact(
            [Bind("FirstName", "LastName", "MobilePhone", "StreetAddress"
            , "CityAddress", "ZipCode", "Country", "EmailAddress")] Contact contact)
        {
            try
            {
                if (contact == null)
                {
                    return BadRequest();
                }
                var result = this.contactService.Save(Guid.Empty, contact);

                return CreatedAtAction("GetContacts", new { id = contact.ContactId }, result);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpDelete]
        public IActionResult DeleteContact(Guid id)
        {
            var deletedContact = contactRepository.Retrieve(id);
            if (deletedContact == null)
            {
                return NotFound();
            }

            this.contactRepository.Delete(id);

            return Ok();
        }

        [HttpPut]
        public IActionResult UpdateContact(
            [Bind("FirstName", "LastName", "MobilePhone", "StreetAddress"
            , "CityAddress", "ZipCode", "Country", "EmailAddress")] Contact modifiedContact, Guid id)
        {

            var contact = contactRepository.Retrieve(id);
            if (contact == null)
            {
                return BadRequest();
            }
            contact.ApplyChanges(modifiedContact);
            contactService.Save(id, contact);
            return Ok(contact);
        }

        [HttpPatch]
        public IActionResult PatchContact([FromBody]JsonPatchDocument patchedContact, Guid id)
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