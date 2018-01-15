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
using BlastAsia.DigiBook.Api.Utils;
using Microsoft.AspNetCore.Cors;
using BlastAsia.DigiBook.Domain.Models;
using Microsoft.AspNetCore.Authorization;

namespace BlastAsia.DigiBook.API.Controllers
{
    [EnableCors("DayTwoApp")]
    [Produces("application/json")]
    //[Route("api/Contacts")]
    public class ContactsController : Controller
    {
        private static List<Contact> contacts = new List<Contact>();
        private readonly IContactService contactService;
        private readonly IContactRepository contactRepository;

        public ContactsController(IContactService contactService, 
            IContactRepository contactRepository)
        {
            this.contactService = contactService;
            this.contactRepository = contactRepository;
        }

        [HttpGet, ActionName("GetContactsWithPagination")]
        [Route("api/Contacts/{page}/{record}")]
        public IActionResult GetContactsWithPagination(int page, int record, string filter)
        {

            var result = new PaginationClass<Contact>();
            try
            {
                result = this.contactRepository.Retrieve(page, record, filter);
            }
            catch (Exception)
            {
                return BadRequest();
            }

            return Ok(result);
        }

        [HttpGet, ActionName("GetContacts")]
        [Route("api/Contacts/{id?}")]
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
        [Route("api/Contacts")]
        [Authorize]
        public IActionResult CreateContact([FromBody]Contact contact)
        {
            try
            {
                if (contact == null)
                {
                    return BadRequest();
                }

                var result = this.contactService.Save(Guid.Empty, contact);

                return CreatedAtAction("GetContacts",
                    new { id = contact.ContactId }, result);
            }

            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpDelete]
        [Route("api/Contacts/{id}")]
        [Authorize]
        public IActionResult DeleteContact(Guid id)
        {
            var contactToDelete = this.contactRepository.Retrieve(id);
            if (contactToDelete == null)
            {
                return NotFound();
            }

            contactRepository.Delete(id);
            return NoContent();
        }   

        [HttpPut]
        [Route("api/Contacts/{id}")]
        [Authorize]
        public IActionResult UpdateContact([FromBody] Contact contact, Guid id)
        {
            if (contact == null)
            {
                return BadRequest();
            }

            var contactToUpdate = contactRepository.Retrieve(id);
            if (contactToUpdate == null)
            {
                return NotFound();
            }
            contactToUpdate.ApplyChanges(contact);
            this.contactService.Save(id, contactToUpdate);
            return Ok(contact);
        }

        [HttpPatch]
        [Route("api/Contacts/{id}")]
        [Authorize]
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
