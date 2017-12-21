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

        private readonly IContactService contactService;
        private readonly IContactRepository contactRepository;

        public ContactsController(IContactService contactService,
            IContactRepository contactRepository)
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
        public IActionResult CreateContact(
            //[Bind("FirstName","LastName","MobilePhone","StreetAddress","CitryAddress","ZipCode","Country",
            //"EmailAddress")] Contact contact)
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
