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
using Microsoft.AspNetCore.Cors;
using BlastAsia.DigiBook.Domain.Models;
using Microsoft.AspNetCore.Authorization;

namespace BlastAsia.DigiBook.API.Controllers
{
    [EnableCors("DayTwoApp")]
    [Produces("application/json")]
    //[Route("api/Appointments")]
    public class AppointmentsController : Controller
    {
        private readonly IAppointmentService appointmentService;
        private readonly IAppointmentRepository appointmentRepository;

        public AppointmentsController(IAppointmentService appointmnetService, IAppointmentRepository appointmnetRepository)
        {

            this.appointmentService = appointmnetService;
            this.appointmentRepository = appointmnetRepository;


        }

        [HttpGet, ActionName("GetAppointmentsWithPagination")]
        [Route("api/Appointments/{page}/{record}")]
        public IActionResult GetAppointmentsWithPagination(int page, int record, string filter)
        {
            var result = new PaginationResult<Appointment>();
            try
            {
                result = this.appointmentRepository.Retrieve(page, record, filter);
            }
            catch (Exception)
            {
                return BadRequest();
            }

            return Ok(result);
        }

        [HttpGet, ActionName("GetContacts")]
        [Route("api/Appointments/{id?}")]
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
        [Route("api/Appointments")]
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

                return CreatedAtAction("GetContacts",
                    new { id = appointment.AppointmentId }, result);
            }

            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpDelete]
        [Route("api/Appointments/{id}")]
        [Authorize]
        public IActionResult DeleteAppointment(Guid id)
        {
            var foundDeleteId = appointmentRepository.Retrieve(id);
            if (foundDeleteId == null)
            {
                return NotFound();
            }

            this.appointmentRepository.Delete(id);

            return NoContent();
        }

        [HttpPut]
        [Route("api/Appointments/{id}")]
        [Authorize]
        public IActionResult UpdateAppointment(
            [FromBody] Appointment appointment, Guid id)
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

            this.appointmentService.Save(id, appointment);

            return Ok(appointment);
        }

        [HttpPatch]
        [Route("api/Appointments/{id}")]
        [Authorize]
        public IActionResult PatchAppointment(
            [FromBody]JsonPatchDocument patchedContact, Guid id)
        {
            if (patchedContact == null)
            {
                return BadRequest();
            }

            var appointment = appointmentRepository.Retrieve(id);
            if (appointment == null)
            {
                return NotFound();
            }

            patchedContact.ApplyTo(appointment);
            appointmentService.Save(id, appointment);

            return Ok(appointment);
        }
    }
}