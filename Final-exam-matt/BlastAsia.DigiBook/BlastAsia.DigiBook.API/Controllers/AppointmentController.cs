using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BlastAsia.DigiBook.Domain.Appointments.Services;
using BlastAsia.DigiBook.Domain.Employees;
using BlastAsia.DigiBook.Domain.Contacts;
using BlastAsia.DigiBook.Domain.Models.Appointments;
using Microsoft.AspNetCore.JsonPatch;

namespace BlastAsia.DigiBook.API.Controllers
{
    [Produces("application/json")]
    [Route("api/Appointment")]
    public class AppointmentController : Controller
    {
        private IAppointmentService appointmentService;
        private IAppointmentRepository appointmentRepo;
        private IEmployeeRepository employeeRepo;
        private IContactRepository contactRepo;

        public AppointmentController(IAppointmentService appointmentService
            , IAppointmentRepository appointmentRepo, IEmployeeRepository employeeRepo
            , IContactRepository contactRepo)
        {
            this.appointmentService = appointmentService;
            this.appointmentRepo = appointmentRepo;
            this.employeeRepo = employeeRepo;
            this.contactRepo = contactRepo;
        }

        [HttpGet, ActionName("GetAppointments")]
        public IActionResult GetAppointments(Guid? id)
        {
            var result = new List<Appointment>();

            if (id == null)
            {
                result.AddRange(appointmentRepo.Retrieve());
            }
            else
            {
                var found = appointmentRepo.Retrieve(id.Value);
                if (found == null) return NoContent();
                result.Add(found);

            }
            
            return Ok(result);
        }

        [HttpPost]
        public IActionResult PostAppointment([FromBody] Appointment appointment)
        {
            try
            {
                if (appointment == null) return BadRequest();
                
                var result = appointmentService.Save(Guid.Empty, appointment);
                return CreatedAtAction("GetAppointments", new { id = appointment.AppointmentId, result});
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpDelete]
        public IActionResult DeleteAppoinment(Guid id)
        {
            try
            {
                var result = appointmentRepo.Retrieve(id);
                if (result == null) return NotFound();

                appointmentRepo.Delete(id);
                return NoContent();
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }

        [HttpPatch]
        public IActionResult PatchAppoinment([FromBody]JsonPatchDocument patchAppointment, Guid id)
        {
            if (patchAppointment == null) return BadRequest();

            var found = appointmentRepo.Retrieve(id);

            if (found == null) return NotFound();
            patchAppointment.ApplyTo(found);
            appointmentService.Save(id, found);

            return Ok(found);

        }
    }
}