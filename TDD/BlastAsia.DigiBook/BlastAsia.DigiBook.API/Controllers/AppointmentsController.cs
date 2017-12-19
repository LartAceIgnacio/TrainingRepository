using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BlastAsia.DigiBook.Domain.Appointments;
using BlastAsia.DigiBook.Domain.Models.Appointments;
using Microsoft.AspNetCore.JsonPatch;
using BlastAsia.DigiBook.API.Utils;

namespace BlastAsia.DigiBook.API.Controllers
{
    [Produces("application/json")]
    [Route("api/Appointments")]
    public class AppointmentsController : Controller
    {
        private readonly IAppointmentService appointmnetService;
        private readonly IAppointmentRepository appointmnetRepository;

        public AppointmentsController(IAppointmentService appointmnetService, IAppointmentRepository appointmnetRepository)
        {

            this.appointmnetService = appointmnetService;
            this.appointmnetRepository = appointmnetRepository;


        }

        [HttpGet, ActionName("GetContacts")]
        public IActionResult GetAppointments(Guid? id)
        {
            var result = new List<Appointment>();
            if (id == null)
            {

                result.AddRange(this.appointmnetRepository.Retrieve());
            }
            else
            {
                var appointment = this.appointmnetRepository.Retrieve(id.Value);
                result.Add(appointment);
            }


            return Ok(result);
        }
        [HttpPost]
        public IActionResult CreateAppointment(
            [FromBody] Appointment appointment)
        {
            var result = this.appointmnetService.Save(Guid.Empty, appointment);

            return CreatedAtAction("GetAppointments",
                new { id = result.AppointmentId }, result);
        }

        [HttpDelete]
        public IActionResult DeleteAppointment(Guid id)
        {
            this.appointmnetRepository.Delete(id);

            return NoContent();
        }
        [HttpPut]
        public IActionResult UpdateContact(
            [FromBody] Appointment appointment, Guid id)
        {

            var oldAppointment = this.appointmnetRepository.Retrieve(id);
            oldAppointment.ApplyChanges(appointment);


            this.appointmnetService.Save(id, appointment);

            return Ok(appointment);
        }

        [HttpPatch]
        public IActionResult PatchAppointment(
            [FromBody]JsonPatchDocument patchedContact, Guid id)
        {
            if (patchedContact == null)
            {
                return BadRequest();
            }

            var appointment = appointmnetRepository.Retrieve(id);
            if (appointment == null)
            {
                return NotFound();
            }

            patchedContact.ApplyTo(appointment);
            appointmnetService.Save(id, appointment);

            return Ok(appointment);
        }
    }
}