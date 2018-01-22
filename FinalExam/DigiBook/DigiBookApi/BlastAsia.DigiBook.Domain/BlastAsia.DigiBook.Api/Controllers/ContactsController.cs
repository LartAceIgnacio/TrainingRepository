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
using Microsoft.AspNetCore.Cors;
using BlastAsia.DigiBook.Domain.Models.Paginations;

namespace BlastAsia.DigiBook.Api.Controllers
{
    [EnableCors("day2app")]
    [Produces("application/json")]
    public class ContactsController : Controller
    {
        private readonly IContactRepository contactRepository;
        private readonly IContactService contactService;
        public ContactsController(IContactService contactService, IContactRepository contactRepository)
        {
            this.contactRepository = contactRepository;
            this.contactService = contactService;

        }
        [HttpGet]
        [Route("api/Contacts/{pageNumber}/{recordNumber}/")]
        public IActionResult GetContacts(int pageNumber, int recordNumber, string query)
        {
            try
            {
                var result = new Pagination<Contact>();
                result = this.contactRepository.Retrieve(pageNumber, recordNumber, query);

                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet, ActionName("GetContacts")]
        [Route("api/Contacts")]
        public IActionResult GetContacts(Guid? id)
        {
            try
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
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("api/Contacts")]
        public IActionResult CreateContact([FromBody] Contact contact)
        {
            try
            {
                if (contact == null)
                {
                    return BadRequest();
                }
                else
                {
                    var result = this.contactService.Save(Guid.Empty, contact);

                    return CreatedAtAction("GetContacts",
                        new { id = contact.ContactId }, result);
                }

            }
            catch (Exception)
            {
                return BadRequest();
            };
        }

        [HttpDelete]
        [Route("api/Contacts")]
        public IActionResult DeleteContact(Guid id)
        {
            try
            {
                var retrieveContact = this.contactRepository.Retrieve(id);
                if (retrieveContact == null)
                {
                    return BadRequest();
                }
                else
                {
                    this.contactRepository.Delete(id);
                    return NoContent();
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }


        }
        [HttpPut]
        [Route("api/Contacts")]
        public IActionResult UpdateContact(
          [FromBody] Contact contact, Guid id)
        {
            try
            {
                if (contact == null)
                {
                    return BadRequest();
                }
                var existingContact = contactRepository.Retrieve(id);

                if (existingContact != null)
                {
                    existingContact.ApplyChanges(contact);

                    var result = this.contactService.Save(id, existingContact);
                    return Ok(result);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }

        }
        [HttpPatch]
        [Route("api/Contacts")]
        public IActionResult PatchContact([FromBody] JsonPatchDocument patchContact, Guid id)
        {
            try
            {
                if (patchContact == null)
                {
                    return BadRequest();
                }

                var contact = contactRepository.Retrieve(id);
                if (contact == null)
                {
                    return NotFound();
                }

                patchContact.ApplyTo(contact);
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