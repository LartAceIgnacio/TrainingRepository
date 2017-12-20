using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BlastAsia.Digibook.Domain.Appointments;
using BlastAsia.Digibook.Domain.Models.Appointments;
using BlastAsia.Digibook.API.Utils;
using Microsoft.AspNetCore.JsonPatch;

namespace BlastAsia.Digibook.API.Controllers
{
    [Produces("application/json")]
    [Route("api/Appointment")]
    public class AppointmentController : Controller
    {
        private readonly IAppointmentService appointmentService;
        private readonly IAppointmentRepository appointmentRepository;

        public AppointmentController(IAppointmentService appointmentService, IAppointmentRepository appointmentRepository)
        {
            this.appointmentService = appointmentService;
            this.appointmentRepository = appointmentRepository;
        }

        [HttpGet,ActionName("GetAppointments")]
        public IActionResult GetAppointments(Guid? id)
        {
            List<Appointment> appointmentList = new List<Appointment>();

            if (id == null)
            {
                appointmentList.AddRange(this.appointmentRepository.Retrieve());
            }
            else
            {
                appointmentList.Add(this.appointmentRepository.Retrieve(id.Value));
            }
            return Ok(appointmentList);
        }

        [HttpPost]
        public IActionResult CreateAppointment([FromBody] Appointment appointment)
        {
            this.appointmentService.Set(appointment);
            return CreatedAtAction("GetAppointments", new { id = appointment.AppointmentId }, appointment);
        }

        [HttpDelete]
        public IActionResult DeleteAppointment(Guid id)
        {
            var appointment = this.appointmentRepository.Retrieve(id);
            if(appointment == null)
            {
                return BadRequest();
            }
            else
            {
                this.appointmentRepository.Delete(id);
                return NoContent();
            }
        }

        [HttpPut]
        public IActionResult UpdateAppointment([FromBody] Appointment appointment, Guid? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            else { 
                var existingAppointment = this.appointmentRepository.Retrieve(id.Value);
                appointment.ApplyAppointmentChanges(existingAppointment);
                var result = this.appointmentService.Set(existingAppointment);
                return Ok(result);
            }
        }

        [HttpPatch]
        public IActionResult PatchAppointment([FromBody]JsonPatchDocument patchedAppointment, Guid id)
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
            this.appointmentService.Set(appointment);

            return Ok(appointment);
        }
    }
}