﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BlastAsia.DigiBook.Domain.Appointments;
using BlastAsia.DigiBook.Domain.Models.Appointments;
using Microsoft.AspNetCore.JsonPatch;
using BlastAsia.DigiBook.API.Utils;
using Microsoft.AspNetCore.Cors;
using BlastAsia.DigiBook.Domain.Models;

namespace BlastAsia.DigiBook.API.Controllers
{
    [EnableCors("PrimeNgDemoApp")]
    [Produces("application/json")]
    //[Route("api/Appointments")]
    public class AppointmentsController : Controller
    {
        private readonly IAppointmentService appointmentService;
        private readonly IAppointmentRepository appointmentRepository;

        public AppointmentsController(IAppointmentService appointmentService, IAppointmentRepository appointmentRepository)
        {
            this.appointmentService = appointmentService;
            this.appointmentRepository = appointmentRepository;
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

        [HttpGet, ActionName("GetAppointments")]
        [Route("api/Appointments")]
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
        public IActionResult CreateAppointment([FromBody] Appointment appointment)
        {
            try
            {
                if (appointment == null)
                {
                    return BadRequest();
                }

                var result = this.appointmentService.Save(Guid.Empty, appointment);

                return CreatedAtAction("GetAppointments", new { id = appointment.AppointmentId }, appointment);
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
        public IActionResult UpdateAppointment([FromBody] Appointment appointment, Guid id)
        {
            try
            {
                if (appointment == null)
                {
                    return BadRequest();
                }

                var existingAppointment = appointmentRepository.Retrieve(id);
                if (existingAppointment == null)
                {
                    return NotFound();
                }

                existingAppointment.ApplyChanges(appointment);

                var result = this.appointmentService.Save(id, existingAppointment);

                return Ok(appointment);
            }
            catch (Exception)
            {
                return BadRequest();
            }          
        }

        [HttpPatch]
        [Route("api/Appointments")]
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
