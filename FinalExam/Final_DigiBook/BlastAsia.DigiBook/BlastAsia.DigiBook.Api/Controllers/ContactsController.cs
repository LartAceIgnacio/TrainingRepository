using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlastAsia.DigiBook.Api.Utils;
using BlastAsia.DigiBook.Domain.Contacts;
using BlastAsia.DigiBook.Domain.Models.Contacts;
using BlastAsia.DigiBook.Domain.Models.Records;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace BlastAsia.DigiBook.Api.Controllers
{
    [EnableCors("DemoApp")]
    [Produces("application/json")]
    [Route("api/Contacts")]
    public class ContactsController : Controller
    {
        private readonly IContactService contactService;
        private readonly IContactRepository contactRepository;

        public ContactsController(IContactRepository contactRepository, IContactService contactService)
        {
            this.contactRepository = contactRepository;
            this.contactService = contactService;
        }

        [HttpGet, ActionName("GetContactsRecord")]
        [Route("{page}/{record}")]
        public IActionResult GetEmployeesWithPagination(int page, int record, string filter)
        {
            var result = new Record<Contact>();
            try
            {
                result = this.contactRepository.Fetch(page, record, filter);
            }
            catch (Exception)
            {
                return BadRequest();
            }

            return Ok(result);
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
            [FromBody] Contact contact)
        {
            try
            {
                if(contact == null)
                {
                    return BadRequest();
                }
                var result = this.contactService.Save(Guid.Empty, contact);

                return CreatedAtAction("GetContactsRecord", new { id = contact.ContactId }, result);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpDelete]
        [Route("{id}")]
        [Authorize]
        public IActionResult DeleteContact(Guid id)
        {
            var deletedContact = contactRepository.Retrieve(id);
            if(deletedContact == null)
            {
                return NotFound();
            }

            this.contactRepository.Delete(id);

            return Ok();
        }

        [HttpPut]
        [Route("{id}")]
        [Authorize]
        public IActionResult UpdateContact(
            [FromBody] Contact modifiedContact, Guid id)
        {
            
            var contact = contactRepository.Retrieve(id);
            if(contact == null)
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