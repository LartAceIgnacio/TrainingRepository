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

namespace BlastAsia.DigiBook.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Contacts")]
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
            [FromBody] Contact contact)
        {
            var result = this.contactService.Save(Guid.Empty, contact);
            return CreatedAtAction("GetContacts", new { id = contact.ContactId }, result);
        }


        [HttpDelete]
        public IActionResult DeleteContact(Guid id)
        {
            var contactToDelete = this.contactRepository.Retrieve(id);
            if (contactToDelete == null)
            {
                NotFound();
            }

            contactRepository.Delete(id);
            return NoContent();
        }

        [HttpPut]
        public IActionResult UpdateContact(
            [Bind("FirstName","LastName","MobilePhone","StreetAddress","CityAddress","ZipCode","Country","EmailAddress")]
                Contact contact,
                    Guid id
            )
        {
          

            var contactToEdit = this.contactRepository.Retrieve(id);

           
            if (contactToEdit == null)
            {
                return NotFound();
            }

            #region Refelction
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

           // contactToEdit.ContactId = id;

            var result = contactService.Save(id, contactToEdit);
            return Ok(result);
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