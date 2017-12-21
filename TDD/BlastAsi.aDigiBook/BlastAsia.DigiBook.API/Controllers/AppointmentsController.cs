using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlastAsia.DigiBook.Domain.Appointments;
using BlastAsia.DigiBook.Domain.Models.Appointments;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using BlastAsia.DigiBook.API.Utils;

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
            try
            {
                if (appointment == null)
                {
                    return BadRequest();
                }
                var result = this.appointmentService.Save(Guid.Empty, appointment);
                return CreatedAtAction("GetContacts", new { id = result.AppointmentId }, result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete]
        public IActionResult DeleteAppointment(Guid id)
        {
            var contactToDelete = this.appointmentRepository.Retrieve(id);
            if (contactToDelete != null)
            {
                this.appointmentRepository.Delete(id);
                return NoContent();
            }
            return NotFound();
        }

        [HttpPatch]
        public IActionResult PatchAppointment(
            [FromBody] JsonPatchDocument patchedAppointment, Guid id)
        {
            try
            {
                if (patchedAppointment == null)
                {
                    return BadRequest();
                }

                var appointment = this.appointmentRepository.Retrieve(id);
                if (appointment == null)
                {
                    return NotFound();
                }

                patchedAppointment.ApplyTo(appointment);
                appointmentService.Save(id, appointment);

                return Ok(appointment);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        public IActionResult UpdateAppointment(
            //[ Bind("AppointmentDate", "GuestId", "HostId", "StartTime",
            //    "EndTime","IsCancelled","IsDone", "Notes" )]
            [FromBody] Appointment appointment, Guid id)
        {
            try
            {
                if (appointment == null)
                {
                    return BadRequest();
                }

                var oldAppointment = this.appointmentRepository.Retrieve(id);
                if (oldAppointment == null)
                {
                    return NotFound();
                }

                oldAppointment.ApplyChanges(appointment);

                var result = this.appointmentService.Save(id, appointment);

                return Ok(oldAppointment);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}