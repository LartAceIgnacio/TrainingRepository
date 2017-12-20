using System;
using BlastAsia.DigiBook.Domain.Contacts;
using BlastAsia.DigiBook.Domain.Employees;
using BlastAsia.DigiBook.Domain.Models.Appointments;
using BlastAsia.DigiBook.Domain.Appointments.Exceptions;

namespace BlastAsia.DigiBook.Domain.Appointments
{
    public class AppointmentService : IAppointmentService
    {
        private IAppointmentRepository appointmentServiceRepository;
        private IContactRepository contactRepository;
        private IEmployeeRepository employeeRepository;

        public AppointmentService(
            IAppointmentRepository appointmentServiceRepository,
            IContactRepository contactRepository,
            IEmployeeRepository employeeRepository
            )
        {
            this.appointmentServiceRepository = appointmentServiceRepository;
            this.contactRepository = contactRepository;
            this.employeeRepository = employeeRepository;
        }

        public Appointment Save(Guid id, Appointment appointment)
        {
            // appointment date validation
            if (appointment.AppointmentDate < DateTime.Now) throw new InvalidAppointmentDateException("Appointment Should not be less than date today");
            // inclusive appointmentTime
            if (appointment.EndTime < appointment.StartTime) throw new NotInclusiveStartAndEndTime("Appointment time should be inclusive");

            // Check if guest is already existing
            var existingGuest = this.contactRepository.Retrieve(appointment.GuestId);
            if (existingGuest == null)
            {
                throw new InvalidGuestIdException("No Guest Record");
            }

            // Check if host is already existing
            var existingHost = this.employeeRepository.Retrieve(appointment.HostId);
            if (existingHost == null)
            {
                throw new InvalidHostIdException("No Host Record");
            }

            // Return value instantiate
            Appointment result = null;

            // check if appointment is existing 
            var existingAppointment = this.appointmentServiceRepository.Retrieve(appointment.AppointmentId);
            if (existingAppointment != null)
            {
                result = this.appointmentServiceRepository.Update(appointment.AppointmentId, appointment);
            }
            else
            {
                result = this.appointmentServiceRepository.Create(appointment);
            }

            return result;
        }
    }
}