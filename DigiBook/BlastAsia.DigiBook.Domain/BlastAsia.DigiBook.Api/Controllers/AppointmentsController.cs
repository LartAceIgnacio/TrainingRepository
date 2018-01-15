using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BlastAsia.DigiBook.Domain.Appointments;
using BlastAsia.DigiBook.Domain.Models.Appointments;
using Microsoft.AspNetCore.JsonPatch;
using BlastAsia.DigiBook.Api.Utils;
using Microsoft.AspNetCore.Cors;
using BlastAsia.DigiBook.Domain.Models.Paginations;

namespace BlastAsia.DigiBook.Api.Controllers
{
    [EnableCors("day2app")]
    [Produces("application/json")]
    public class AppointmentsController : Controller
    {
        private readonly IAppointmentRepository appointmentRepository;
        private readonly IAppointmentService appointmentService;
        public AppointmentsController(IAppointmentService appointmentService, IAppointmentRepository appointmentRepository)
        {
            this.appointmentRepository = appointmentRepository;
            this.appointmentService = appointmentService;

        }

        [HttpGet]
        [Route("api/Appointments/{pageNumber}/{recordNumber}")]
        public IActionResult GetAppointments(int pageNumber, int recordNumber, DateTime? date)
        {
            try
            {
                var result = new Pagination<Appointment>();
                result = this.appointmentRepository.Retrieve(pageNumber, recordNumber, date);

                return Ok(result);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet, ActionName("GetAppointments")]
        [Route("api/Appointments")]
        public IActionResult GetAppointments(Guid? id)
        {
            try
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
            catch (Exception)
            {
                return BadRequest();
            }

        }

        [HttpPost]
        [Route("api/Appointments")]
        public IActionResult CreateAppointment([FromBody] Appointment appointment)
        {
            try
            {
                if (appointment == null)
                {
                    return BadRequest();
                }
                else
                {
                    var result = this.appointmentService.Save(Guid.Empty, appointment);

                    return CreatedAtAction("GetAppointments", new { id = appointment.AppointmentId }, appointment);
                }
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
            try
            {
                var retrieveAppointment = this.appointmentRepository.Retrieve(id);
                if (retrieveAppointment == null)
                {
                    return BadRequest();
                }
                else
                {
                    this.appointmentRepository.Delete(id);
                    return NoContent();
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }

        }
        [HttpPut]
        [Route("api/Appointments")]
        public IActionResult UpdateAppointment([FromBody] Appointment appointment, Guid id)
        {
            try
            {
                if (appointment == null)
                {
                    return BadRequest();
                }

                var existingAppointment = appointmentRepository.Retrieve(id);

                if (existingAppointment != null)
                {

                    existingAppointment.ApplyChanges(appointment);

                    this.appointmentService.Save(id, existingAppointment);

                    return Ok(appointment);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }

        }
        [HttpPatch]
        [Route("api/Appointments")]
        public IActionResult PatchAppointment([FromBody] JsonPatchDocument patchAppointment, Guid id)
        {
            try
            {
                if (patchAppointment == null)
                {
                    return BadRequest();
                }

                var appointment = appointmentRepository.Retrieve(id);
                if (appointment == null)
                {
                    return NotFound();
                }

                patchAppointment.ApplyTo(appointment);
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