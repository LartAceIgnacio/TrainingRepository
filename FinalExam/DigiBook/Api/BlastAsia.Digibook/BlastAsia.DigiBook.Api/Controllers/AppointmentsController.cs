using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BlastAsia.DigiBook.Domain.Appointments;
using BlastAsia.DigiBook.Domain.Models.Appointments;
using BlastAsia.DigiBook.Api.Utils;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Cors;
using BlastAsia.DigiBook.Domain.Models.Pagination;
using Microsoft.AspNetCore.Authorization;

namespace BlastAsia.DigiBook.Api.Controllers
{
    [EnableCors("DemoAppDay2")]
    [Produces("application/json")]
    public class AppointmentsController : Controller
    {
        private readonly IAppointmentRepository appointmentRepository;
        private readonly IAppointmentService appointmentService;
        public AppointmentsController(IAppointmentRepository appointmentRepository, IAppointmentService appointmentService)
        {
            this.appointmentRepository = appointmentRepository;
            this.appointmentService = appointmentService;
        }


        [HttpGet, ActionName("GetAppointments")]
        [Route("api/Appointments")]
        public IActionResult GetAppointment(Guid? id)
        {
            var result = new List<Appointment>();

            if (id == null)
            {
                result.AddRange(this.appointmentRepository.Retrieve());
            }
            else
            {
                result.Add(this.appointmentRepository.Retrieve(id.Value));
            }

            return Ok(result);
        }

        [HttpGet]
        [Route("api/Appointments/{pageNumber}/{recordNumber}/")]
        public IActionResult GetAppointment(int pageNumber, int recordNumber, string query)
        {
            try
            {
                var result = new Pagination<Appointment>();
                result =  this.appointmentRepository.Retrieve(pageNumber, recordNumber, query);
                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Authorize]
        [Route("api/Appointments")]
        public IActionResult CreateAppointment([FromBody] Appointment appointment)
        {
            try
            {
                if (appointment == null)
                {
                    return BadRequest();
                }

                var result = appointmentService.Save(Guid.Empty, appointment);
                //return Ok(result);
                return CreatedAtAction("GetAppointments", new { id = appointment.AppointmentId }, result);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpDelete]
        [Authorize]
        [Route("api/Appointments")]
        public IActionResult DeleteAppointment(Guid id)
        {
            var appointmentToDelete = appointmentRepository.Retrieve(id);
            if (appointmentToDelete == null)
            {
                return NotFound();
            }

            appointmentRepository.Delete(id);
            return NoContent();
        }

        [HttpPut]
        [Authorize]
        [Route("api/Appointments")]
        public IActionResult UpdateAppointment([FromBody] Appointment appointment, Guid id)
        {
            try
            {
                if (appointment == null)
                {
                    return BadRequest();
                }

                var appointmentToUpdate = appointmentRepository.Retrieve(id);

                if (appointmentToUpdate == null)
                {
                    return NotFound();
                }

                appointmentToUpdate.ApplyChanges(appointment);

                var result = appointmentService.Save(id, appointmentToUpdate);
                return Ok(result);

            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpPatch]
        [Authorize]
        [Route("api/Appointments")]
        public IActionResult PatchAppointment([FromBody]JsonPatchDocument patchedAppointment, Guid id)
        {
            try
            {
                if (patchedAppointment == null)
                {
                    return BadRequest();
                }
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