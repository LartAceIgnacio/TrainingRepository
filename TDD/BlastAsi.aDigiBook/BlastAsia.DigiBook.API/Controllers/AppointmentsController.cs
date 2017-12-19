using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlastAsia.DigiBook.Domain.Appointments;
using BlastAsia.DigiBook.Domain.Models.Appointments;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace BlastAsia.DigiBook.API.Controllers
{
    [Produces("application/json")]
    [Route("api/Appointments")]
    public class AppointmentsController : Controller
    {
        private readonly IAppointmentService appointmentService;
        private readonly IAppointmentRepository appointmentRepository;
        public AppointmentsController(IAppointmentService appointmentService, IAppointmentRepository appointmentRepository)
        {
            this.appointmentService = appointmentService;
            this.appointmentRepository = appointmentRepository;
        }

        [HttpGet, ActionName("GetApppointments")]
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
                return Ok(appointment);
            }
            return Ok(result);
        }

        [HttpPost]
        public IActionResult CreateAppointment(
            [FromBody] Appointment appointment)
        {
            var result = this.appointmentService.Save(Guid.Empty,appointment);

            return CreatedAtAction("GetAppointments", new { id = result.AppointmentId }, result);
        }

        [HttpDelete]
        public IActionResult DeleteAppointment(Guid id)
        {
            this.appointmentRepository.Delete(id);
            return NoContent();
        }

        [HttpPatch]
        public IActionResult PatchAppointment(
            [FromBody] JsonPatchDocument patchedAppointment, Guid id)
        {
            if (patchedAppointment == null)
            {
                return BadRequest();
            }

            var appointment = appointmentRepository.Retrieve(id);
            if (appointment == null)
            {
                return NotFound();
            }

            patchedAppointment.ApplyTo(appointment);
            appointmentService.Save(id, appointment);

            return Ok(appointment);
        }

        [HttpPut]
        public IActionResult UpdateAppointment(
            //[ Bind("AppointmentDate", "GuestId", "HostId", "StartTime",
            //    "EndTime","IsCancelled","IsDone", "Notes" )]
            [FromBody] Appointment appointment, Guid id)
        {
            this.appointmentService.Save(id, appointment);
            return Ok(appointment);
        }
    }
}