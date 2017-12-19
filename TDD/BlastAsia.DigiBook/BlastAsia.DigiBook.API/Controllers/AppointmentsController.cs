using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BlastAsia.DigiBook.Domain.Appointments;
using BlastAsia.DigiBook.Domain.Models.Appointments;
using Microsoft.AspNetCore.JsonPatch;
using BlastAsia.DigiBook.API.Utils;

namespace BlastAsia.DigiBook.API.Controllers
{
    [Route("api/[controller]")]
    public class AppointmentsController : Controller
    {
        private readonly IAppointmentService appointmentService;
        private readonly IAppointmentRepository appointmentRepository;

        public AppointmentsController(IAppointmentService appointmentService, IAppointmentRepository appointmentRepository)
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

        [HttpPost]
        public IActionResult CreateAppointment([FromBody] Appointment appointment)
        {
            var result = this.appointmentService.Save(Guid.Empty, appointment);
            return CreatedAtAction("GetAppointments", new { id = appointment.AppointmentId }, appointment);
        }

        [HttpDelete]
        public IActionResult DeleteAppointment(Guid id)
        {
            this.appointmentRepository.Delete(id);
            return NoContent();
        }

        [HttpPut]
        public IActionResult UpdateAppointment([FromBody] Appointment appointment, Guid id)
        {
            var existingAppointment = appointmentRepository.Retrieve(id);
            existingAppointment.ApplyChanges(appointment);
            this.appointmentService.Save(id, existingAppointment);
            return Ok(appointment);
        }

        [HttpPatch]
        public IActionResult PatchAppointment([FromBody]JsonPatchDocument patchAppointment, Guid id)
        {
            if (patchAppointment == null)
            {
                return BadRequest();
            }

            var appointment = appointmentRepository.Retrieve(id);
            if(appointment == null)
            {
                return NotFound();
            }

            patchAppointment.ApplyTo(appointment);
            appointmentService.Save(id, appointment);

            return Ok(appointment);
        }
    }
}
