using System;
using BlastAsia.DigiBook.Domain.Models.Employees;
using System.Linq;
using BlastAsia.DigiBook.Domain.Employees;
using BlastAsia.DigiBook.Domain.Contacts;
using BlastAsia.DigiBook.Domain.Models.Appointments;

namespace BlastAsia.DigiBook.Domain.Appointments
{
    public class AppointmentService : IAppointmentService
    {
        private IAppointmentRepository appointmentRepository;
        private IEmployeeRepository employeeRepository;
        private IContactRepository contactRepository;


        public AppointmentService(
            IAppointmentRepository appointmentRepository,
            IEmployeeRepository employeeRepository,
            IContactRepository contactRepository)
        {
            this.appointmentRepository = appointmentRepository;
            this.employeeRepository = employeeRepository;
            this.contactRepository = contactRepository;
        }

        public Appointment Save(Guid id, Appointment appointment)
        {
            if(appointment.AppointmentDate <DateTime.Today)
            {
                throw new InvalidAppointmentDateException("Appointment date should be greater than or equal today.");
            }
            if (appointment.StartTime >= appointment.EndTime)
            {
                throw new InvalidTimeException("Invalid time input.");
            }
           
            Appointment result = null;
            
            var foundGuestId = contactRepository.Retrieve(appointment.GuestId);

            if (foundGuestId == null)
            {
                throw new InvalidGuestIdException("Guest ID should not be empty.");
            }


            var foundEmployeeId = employeeRepository.Retrieve(appointment.HostId);

            if (foundEmployeeId == null)
            {
                throw new InvalidEmployeeIdException("Employee ID should not be empty.");
            }

            var foundAppointmentId = appointmentRepository.Retrieve(id);

            if (foundAppointmentId == null)
            {
                result = appointmentRepository.Create(appointment);
            }
            else
            {
                foundAppointmentId.AppointmentDate = appointment.AppointmentDate;
                foundAppointmentId.appointmentId = appointment.appointmentId;
                foundAppointmentId.EndTime = appointment.EndTime;
                foundAppointmentId.GuestId = appointment.GuestId;
                foundAppointmentId.HostId = appointment.HostId;
                foundAppointmentId.StartTime = appointment.StartTime;
                foundAppointmentId.IsCancelled = appointment.IsCancelled;
                foundAppointmentId.IsDone = appointment.IsDone;
                foundAppointmentId.Notes = appointment.Notes;
                result = appointmentRepository.Update(foundAppointmentId.appointmentId, foundAppointmentId);
            }

            return result;
        }
    }
}