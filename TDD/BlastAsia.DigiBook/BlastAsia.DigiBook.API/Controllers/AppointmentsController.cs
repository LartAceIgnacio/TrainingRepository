using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlastAsia.DigiBook.API.Utils;
using BlastAsia.DigiBook.Domain.Appointments;
using BlastAsia.DigiBook.Domain.Contacts;
using BlastAsia.DigiBook.Domain.Employees;
using BlastAsia.DigiBook.Domain.Models.Appointments;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace BlastAsia.DigiBook.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Appointments")]
    public class AppointmentsController : Controller
    {
        private readonly IAppointmentService appointmentService;
        private readonly IAppointmentRepository appointmentRepository;

        public AppointmentsController(IAppointmentRepository appointmentRepository, IAppointmentService appointmentService)
        {
            this.appointmentRepository = appointmentRepository;
            this.appointmentService = appointmentService;
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
        public IActionResult CreateAppointment(
            [Bind] Appointment appointment)
        {
            try
            {
                if (appointment == null)
                {
                    return BadRequest();
                }
                var result = this.appointmentService.Save(Guid.Empty, appointment);

                return CreatedAtAction("GetAppointments", new { id = appointment.AppointmentId }, result);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpDelete]
        public IActionResult DeleteAppointment(Guid id)
        {
            var deletedAppointment = appointmentRepository.Retrieve(id);
            if (deletedAppointment == null)
            {
                return NotFound();
            }
            this.appointmentRepository.Delete(id);

            return Ok();
        }

        [HttpPut]
        public IActionResult UpdateAppointment(
            [FromBody] Appointment modifiedAppointment, Guid id)
        {
            var appointment = appointmentRepository.Retrieve(id);
            if (appointment == null)
            {
                return BadRequest();
            }
            appointment.ApplyChanges(modifiedAppointment);
            appointmentService.Save(id, appointment);
            return Ok(appointment);
        }

        [HttpPatch]
        public IActionResult PatchAppointment([FromBody]JsonPatchDocument patchedAppointment, Guid id)
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
    }
}
