using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BlastAsia.DigiBook.Domain.Models.Contacts;
using BlastAsia.DigiBook.Domain.Contacts;
using BlastAsia.DigiBook.Insfrastracture.Persistence;
using Microsoft.AspNetCore.JsonPatch;
using System.Reflection;
using BlastAsia.DigiBook.Api.Utils;
using Microsoft.AspNetCore.Cors;
using BlastAsia.DigiBook.Domain.Models.Pagination;

namespace BlastAsia.DigiBook.Api.Controllers
{
    [EnableCors("PrimeNgDemoApp")]
    [Produces("application/json")]
    //[Route("api/Contacts")]
    public class ContactsController : Controller
    {
        private readonly IContactRepository contactRepository;
        private readonly IContactService contactService;
        public ContactsController(IContactRepository contactRepository, IContactService contactService)
        {
            this.contactRepository = contactRepository;
            this.contactService = contactService;
        }

        [HttpGet, ActionName("GetContacts")]
        [Route("api/Contacts")]
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

        [HttpGet]
        [Route("api/Contacts/{pageNumber}/{recordNumber}/")]
        public IActionResult GetContact(int pageNumber, int recordNumber, string query)
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

        [HttpPost]
        [Route("api/Contacts")]
        public IActionResult CreateContact(
            [FromBody] Contact contact)
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
        [Route("api/Contacts")]
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
        [Route("api/Contacts")]
        public IActionResult UpdateContact(
                [FromBody]
                Contact contact,
                    Guid id
            )
        {
            try
            {

                if (contact == null)
                {
                    return BadRequest();
                }

                var contactToEdit = this.contactRepository.Retrieve(id);
                if (contactToEdit == null)
                {
                    return NotFound();
                }

                #region Reflection
                //Type type = typeof(Contact);
                //PropertyInfo[] properties = type.GetProperties();
                //foreach (PropertyInfo property in properties)
                //{
                //    Console.WriteLine("{0} = {1}", property.Name, property.GetValue(contact, null));
                //    var x = property.GetValue(contact, null);

                //    contactToEdit.GetType().GetProperty(property.Name).SetValue(contactToEdit, x);

                //}
                #endregion

                contactToEdit.ApplyChanges(contact);

                var result = contactService.Save(id, contactToEdit);
                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest();
            }


        }

   

        [HttpPatch]
        [Route("api/Contacts")]
        public IActionResult PatchContact([FromBody]JsonPatchDocument patchedContact, Guid id)
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