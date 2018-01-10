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
using Microsoft.AspNetCore.Cors;
using BlastAsia.DigiBook.Domain.Models;

namespace BlastAsia.DigiBook.API.Controllers
{
    [EnableCors("Day2App")]
    [Produces("application/json")]
    //[Route("api/Contacts")]
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

        [HttpGet, ActionName("GetContactsWithPagination")]
        [Route("api/Contacts/{page}/{record}")]
        public IActionResult GetContactsWithPagination(int page, int record, string filter)
        {
            var result = new PaginationResult<Contact>();
            try {
                result = this.contactRepository.Retrieve(page, record, filter);
            }
            catch (Exception) {
                return BadRequest();
            }

            return Ok(result);
        }

        [HttpGet, ActionName("GetContacts")]
        [Route("api/Contacts/{id?}")]
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
        [Route("api/Contacts")]
        public IActionResult CreateContact([FromBody] Contact contact)
        {
            try {
                if(contact == null) {
                    return BadRequest();
                }

                var result = this.contactService.Save(Guid.Empty, contact);

                return CreatedAtAction("GetContacts", new { id = contact.ContactId }, contact);
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("api/Contacts/{id}")]
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
        [Route("api/Contacts/{id}")]
        public IActionResult UpdateContact(
            [FromBody] Contact contact, Guid id)
        {
            try {
                if(contact == null) {
                    return BadRequest();
                }

                var oldContact = this.contactRepository.Retrieve(id);
                if (oldContact == null) {
                    return NotFound();
                }

                oldContact.ApplyNewChanges(contact);

                var result = this.contactService.Save(id, oldContact);

                return Ok(oldContact);
            }
            catch (Exception) {
                return BadRequest();
            }
        }

        [HttpPatch]
        [Route("api/Contacts/{id}")]
        public IActionResult PatchContact([FromBody]JsonPatchDocument patchedContact, Guid id)
        {
            try {
                if (patchedContact == null) {
                    return BadRequest();
                }

                var contact = contactRepository.Retrieve(id);
                if (contact == null) {
                    return NotFound();
                }

                patchedContact.ApplyTo(contact);
                contactService.Save(id, contact);

                return Ok(contact);
            }
            catch (Exception) {
                return BadRequest();
            }
        }
    }
}