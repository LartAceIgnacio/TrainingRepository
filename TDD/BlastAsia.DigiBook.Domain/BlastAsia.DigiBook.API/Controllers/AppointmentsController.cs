﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlastAsia.DigiBook.Domain;
using BlastAsia.DigiBook.Domain.Appointments;
using BlastAsia.DigiBook.Domain.Models.Appointments;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using BlastAsia.DigiBook.API.Utils;

namespace BlastAsia.DigiBook.API.Controllers
{
    [Produces("application/json")]
    [Route("api/Appointments")]
    public class AppointmentsController : Controller
    {
        public static List<Appointment> appointment = new List<Appointment>();
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
            if (id == null) {
                result.AddRange(this.appointmentRepository.Retrieve());
            }
            else {
                var employee = this.appointmentRepository.Retrieve(id.Value);
                result.Add(employee);
            }

            return Ok(result);
        }

        [HttpPost]
        public IActionResult CreateAppointment([FromBody] Appointment appointment)
        {
            try {
                if(appointment == null) {
                    return BadRequest();
                }

                var result = this.appointmentService.Save(Guid.Empty, appointment);

                return CreatedAtAction("GetAppointments", new { id = appointment.AppointmentId }, appointment);
            }
            catch (Exception) {
                return BadRequest();
            }
        }

        [HttpDelete]
        public IActionResult DeleteAppointment(Guid id)
        {
            var appointmentToDelete = this.appointmentRepository.Retrieve(id);
            if (appointmentToDelete != null) {
                this.appointmentRepository.Delete(id);
                return NoContent();
            }

            return NotFound();
        }

        [HttpPut]
        public IActionResult UpdateAppointment(
            [FromBody] Appointment appointment, Guid id)
        {
            try {
                if (appointment == null) {
                    return BadRequest();
                }

                var oldAppointment = this.appointmentRepository.Retrieve(id);
                if (oldAppointment == null) {
                    return NotFound();
                }
                oldAppointment.ApplyNewChanges(appointment);

                var result = this.appointmentService.Save(id, oldAppointment);

                return Ok(oldAppointment);
            }
            catch (Exception) {
                return BadRequest();
            }
        }

        [HttpPatch]
        public IActionResult PatchAppointment([FromBody]JsonPatchDocument patchedAppointment, Guid id)
        {
            try {
                if (patchedAppointment == null) {
                    return BadRequest();
                }

                var appointment = appointmentRepository.Retrieve(id);
                if (appointment == null) {
                    return NotFound();
                }

                patchedAppointment.ApplyTo(appointment);
                appointmentService.Save(id, appointment);

                return Ok(appointment);
            }
            catch (Exception) {
                return BadRequest();
            }
        }
    }
}