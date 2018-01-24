using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BlastAsia.DigiBook.Domain.Models;
using BlastAsia.DigiBook.Domain.Models.Contacts;
using BlastAsia.DigiBook.Infrastructure.Persistence;
using BlastAsia.DigiBook.Domain.Contacts;
using Microsoft.AspNetCore.JsonPatch;
using BlastAsia.DigiBook.API.Utils;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;

namespace BlastAsia.DigiBook.API.Controllers
{
    [EnableCors("demoApp")]
    [Produces("application/json")]
    
    public class ContactsController : Controller
    {
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
            var result = new Pagination<Contact>();
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

                return CreatedAtAction("GetContacts",
                    new { id = contact.ContactId }, result);
            }
            catch (Exception exc)
            {
                return BadRequest(exc.Message);
            }
          
        }

        [HttpDelete]
        [Route("api/Contacts/{id}")]
        [Authorize]
        public IActionResult DeleteContact(Guid id)
        {
            var contactToDelete = this.contactRepository.Retrieve(id);
            if (contactToDelete != null)
            {
                this.contactRepository.Delete(id);
                return NoContent();
            }
            return NotFound();
        }
     

        [HttpPut]
        [Route("api/Contacts/{id}")]
        [Authorize]
        public IActionResult UpdateContact(
            //  [Bind("FirstName","LastName","MobilePhone","StreetAddress","CitryAddress","ZipCode","Country",
            //"EmailAddress")] Contact contact, Guid id)
            [FromBody] Contact contact,Guid id)
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

                var result = this.contactService.Save(id, contact);

                return Ok(existingContact);
            }
            catch (Exception)
            {
                return BadRequest();
            }

            //    this.contactService.Save(id,existingContact);
            //return Ok(contact);
        }

        [HttpPatch]
        [Route("api/Contacts/{id}")]
        [Authorize]
        public IActionResult PatchContact(
            [FromBody]JsonPatchDocument patchedContact, Guid id)
        {
            try
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
            catch (Exception)
            {
                return BadRequest();
            }
        }
        
    }
}
