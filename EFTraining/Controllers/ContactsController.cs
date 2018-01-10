using System;
using Microsoft.AspNetCore.Mvc;
using EFTraining.Data;
using EFTraining.Data.Models;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace EFTraining.Data
{
    [Route("api/contacts")]
    public class ContactsController : Controller
    {
        private DigiBookDbContext context;

        public ContactsController(DigiBookDbContext context)
        {   
            this.context = context;
        }

       [HttpGet("")]
        public IActionResult GetContacts()
        {
            var contacts = context.Contacts
            .ToList();
            return Json(contacts);
        }
        [HttpGet("{id}/appointments")]
        public IActionResult GetContactAppointments(Guid id)
        {
            var contacts = context.Contacts
            .Where( c=> c.ContactId == id)
            .Include(c=>c.Appointments)
            .ToList();
            return Json(contacts);
        }
    }
}