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
using Microsoft.AspNetCore.Cors;
using BlastAsia.DigiBook.Domain.Models.Pagination;

namespace BlastAsia.DigiBook.Api.Controllers
{
    [EnableCors("day2app")]
    [Produces("application/json")]

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
        [Route("api/Appointments/{pageNumber}/{recordNumber}/")]
        public IActionResult GetContact(int pageNumber, int recordNumber, string query)
        {
            try
            {
                var result = new Pagination<Appointment>();
                result = this.appointmentRepository.Retrieve(pageNumber, recordNumber, query);
                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("api/Appointments")]
        public IActionResult GetAppointments(Guid? id)
        {
            var result = new List<Appointment>();
            if (id == null)
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
        [Route("api/Appointments")]
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

                return CreatedAtAction("GetAppointments",
                    new { id = appointment.AppointmentId }, result);
            }

            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpDelete]
        [Route("api/Appointments")]
        public IActionResult DeleteAppointment(Guid id)
        {
            var appointmentToDelete = this.appointmentRepository.Retrieve(id);
            if (appointmentToDelete == null)
            {
                return NotFound();
            }
            this.appointmentRepository.Delete(id);
            return NoContent();
        }

        [HttpPut]
        [Route("api/Appointments")]
        public IActionResult UpdateAppointment(
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

                return Ok(appointment);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPatch]
        [Route("api/Appointments")]
        public IActionResult PatchAppointment(
            [FromBody]JsonPatchDocument patchedAppointment, Guid id)
        {
            try
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
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}