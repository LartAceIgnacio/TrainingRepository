using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BlastAsia.DigiBook.Domain.Models.Appointments;
using BlastAsia.DigiBook.Domain.Appointments;
using Microsoft.AspNetCore.JsonPatch;
using BlastAsia.DigiBook.Api.Utils;

namespace BlastAsia.DigiBook.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Appointments")]
    public class AppointmentsController : Controller
    {
        private static List<Appointment> appointments = new List<Appointment>();

        private readonly IAppointmentService appointmentService;
        private readonly IAppointmentRepository appointmentRepository;

        public AppointmentsController(IAppointmentService appointmentService,
            IAppointmentRepository appointmentRepository)
        {
            this.appointmentService = appointmentService;
            this.appointmentRepository = appointmentRepository;
        }

        [HttpGet]
        public IActionResult GetAppointments(Guid? id)
        {
            var result = new List<Appointment>();
            if(id == null)
            {
                result.AddRange(this.appointmentRepository.Retreive());
            }
            else
            {
                var appointment = this.appointmentRepository.Retrieve(id.Value);
                result.Add(appointment);
            }

            return Ok(result);
        }

        [HttpPost]
        public IActionResult CreateAppointment(
            [FromBody] Appointment appointment)
        {
            var result = this.appointmentService.Save(Guid.Empty, appointment);

            return CreatedAtAction("GetAppointments",
                new { id = appointment.appointmentId }, result);
        }

        [HttpDelete]
        public IActionResult DeleteAppointment(Guid id)
        {
            this.appointmentRepository.Delete(id);
            return NoContent();
        }

        [HttpPut]
        public IActionResult UpdateAppointment(
            [FromBody] Appointment appointment, Guid id)
        {
            var existingAppointment = appointmentRepository.Retrieve(id);

            existingAppointment.ApplyChanges(appointment);

            this.appointmentService.Save(id, appointment);

            return Ok(appointment);
        }

        [HttpPatch]
        public IActionResult PatchAppointment(
            [FromBody]JsonPatchDocument patchedAppointment, Guid id)
        {
            if(patchedAppointment == null)
            {
                return BadRequest();
            }
            var appointment = appointmentRepository.Retrieve(id);
            if(appointment == null)
            {
                return NoContent();
            }

            patchedAppointment.ApplyTo(appointment);
            appointmentService.Save(id, appointment);

            return Ok(appointment);
        }
    }
}