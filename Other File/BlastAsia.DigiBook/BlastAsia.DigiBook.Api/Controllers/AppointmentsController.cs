using System;
using System.Collections.Generic;
using System.Linq;
using BlastAsia.DigiBook.Api.Utils;
using BlastAsia.DigiBook.Domain.Appointments;
using BlastAsia.DigiBook.Domain.Models.Appointments;
using BlastAsia.DigiBook.Domain.Models.Records;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace BlastAsia.DigiBook.Api.Controllers
{
    [EnableCors("DemoApp")]
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

        [HttpGet, ActionName("GetAppointmentsRecord")]
        [Route("{page}/{record}")]
        public IActionResult GetEmployeesWithPagination(int page, int record, string filter)
        {
            var result = new Record<Appointment>();
            try
            {
                result = this.appointmentRepository.Fetch(page, record, filter);
            }
            catch (Exception)
            {
                return BadRequest();
            }

            return Ok(result);
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
        [Authorize]
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

                return CreatedAtAction("GetAppointments", new { id = appointment.AppointmentId }, result);
            }
            catch(Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpDelete]
        [Route("{id}")]
        [Authorize]
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
        [Route("{id}")]
        [Authorize]
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
            if (patchedAppointment== null)
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