using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BlastAsia.DigiBook.Domain.Models.Contacts;
using BlastAsia.DigiBook.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using BlastAsia.DigiBook.Domain.Contacts;
using Microsoft.AspNetCore.JsonPatch;
using BlastAsia.DigiBook.API.Utils;
using Microsoft.AspNetCore.Cors;
using BlastAsia.DigiBook.Domain.Models;
using Microsoft.AspNetCore.Authorization;

namespace BlastAsia.DigiBook.API.Controllers
{
    [EnableCors("PrimeNgDemoApp")]
    [Produces("application/json")]
   // [Route("api/Contacts")]
    public class ContactsController : Controller
    {
        private readonly IContactService contactService;
        private readonly IContactRepository contactRepository;

        public ContactsController(IContactService contactService, IContactRepository contactRepository)
        {
            this.contactService = contactService;
            this.contactRepository = contactRepository;
        }

        [HttpGet, ActionName("GetContactsWithPagination")]
        [Route("api/Contacts/{page}/{record}")]
        public IActionResult GetContactsWithPagination(int page, int record, string filter)
        {
            //   var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var result = new PaginationResult<Contact>();
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
        [Route("api/Contacts")]
        public IActionResult GetContacts(Guid? id)
        {
            var result = new List<Contact>();
            if(id == null)
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
        [Authorize]
        [Route("api/Contacts")]
        public IActionResult CreateContact(
            [FromBody]
            Contact contact)
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
        [Authorize]
        [Route("api/Contacts")]
        public IActionResult DeleteContact(Guid id)
        {
            var contactToDelete = this.contactRepository.Retrieve(id);
            if (contactToDelete == null)
            {
                return NotFound();
            }
            this.contactRepository.Delete(id);
            return NoContent();
        }

        [HttpPut]
        [Authorize]
        [Route("api/Contacts")]
        public IActionResult UpdateContact(
            [FromBody]
            Contact contact, Guid id)
        {
            try
            {
                if (contact == null)
                {
                    return BadRequest();
                }

                var existingContact = contactRepository.Retrieve(id);
                if (existingContact == null)
                {
                    return NotFound();
                }

                existingContact.ApplyChanges(contact);

                var result = this.contactService.Save(id, existingContact);

                return Ok(contact);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPatch]
        [Authorize]
        [Route("api/Contacts")]
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