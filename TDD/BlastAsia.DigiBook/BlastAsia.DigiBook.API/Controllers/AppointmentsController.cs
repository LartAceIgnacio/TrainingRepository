using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BlastAsia.DigiBook.Domain.Appointments;
using BlastAsia.DigiBook.Domain.Models.Appointments;
using Microsoft.AspNetCore.JsonPatch;

namespace BlastAsia.DigiBook.API.Controllers
{
    [Produces("application/json")]
    [Route("api/Appointments")]
    public class AppointmentsController : Controller
    {
        private readonly IAppointmentService appointmentService;
        private readonly IAppointmentRepository appointmentRepository;

        public AppointmentsController(IAppointmentService appointmentService,
           IAppointmentRepository appointmentRepository)
        {
            this.appointmentService = appointmentService;
            this.appointmentRepository = appointmentRepository;
        }

        [HttpGet, ActionName("GetAppointments")]
        public IActionResult GetAppointments(Guid? id)
        {
            var result = new List<Appointment>();
            if (id == null)
            {
                result.AddRange(this.appointmentRepository.Retrieve());
            }
            else
            {
                var appointment = this.appointmentRepository.Retrieve(id.Value);
                result.Add(appointment);
            }

            return Ok(result);
        }

        [HttpDelete]
        public IActionResult DeleteAppointment(Guid id)
        {
            this.appointmentRepository.Delete(id);

            return Ok();
        }

        [HttpPost]
        public IActionResult CreateAppointment(
           [FromBody]Appointment appointment)
        {
            var result = this.appointmentService.Save(Guid.Empty, appointment);

            return CreatedAtAction("GetAppointments", new { id = result.AppointmentId }, result);
        }

        [HttpPut]
        public IActionResult UpdateAppointment(
            [FromBody] Appointment appointment, Guid id)
        {
            this.appointmentService.Save(id, appointment);

            return Ok(appointment);
        }
        [HttpPatch]
        public IActionResult PatchAppointment(
            [FromBody]JsonPatchDocument PatchAppointment, Guid id)
        {
            if (PatchAppointment == null)
            {
                return BadRequest();
            }

            var appointment = appointmentRepository.Retrieve(id);
            if (appointment == null)
            {
                return NotFound();
            }

            PatchAppointment.ApplyTo(appointment);
            appointmentService.Save(id, appointment);

            return Ok(appointment);
        }
    }
}