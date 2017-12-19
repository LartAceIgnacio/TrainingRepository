using System;
using BlastAsia.DigiBook.Domain.Contacts;
using BlastAsia.DigiBook.Domain.Employees;
using BlastAsia.DigiBook.Domain.Models.Appointments;
using BlastAsia.DigiBook.Domain.Appointments.Exceptions;

namespace BlastAsia.DigiBook.Domain.Appointments
{
    public class AppointmentService : IAppointmentService
    {
        private IAppointmentRepository _appointmentServiceRepository;
        private IContactRepository _contactRepository;
        private IEmployeeRepository _employeeRepository;

        public AppointmentService(
            IAppointmentRepository appointmentServiceRepository,
            IContactRepository contactRepository,
            IEmployeeRepository employeeRepository
            )
        {
            _appointmentServiceRepository = appointmentServiceRepository;
            _contactRepository = contactRepository;
            _employeeRepository = employeeRepository;
        }

        public Appointment Save(Guid id, Appointment appointment)
        {
            // appointment date validation
            if (appointment.AppointmentDate < DateTime.Now) throw new InvalidAppointmentDateException("Appointment Should not be less than date today");
            // inclusive appointmentTime
            if (appointment.EndTime < appointment.StartTime) throw new NotInclusiveStartAndEndTime("Appointment time should be inclusive");

            // Check if guest is already existing
            var existingGuest = _contactRepository.Retrieve(appointment.GuestId);
            if (existingGuest == null)
            {
                throw new InvalidGuestIdException("No Guest Record");
            }

            // Check if host is already existing
            var existingHost = _employeeRepository.Retrieve(appointment.HostId);
            if (existingHost == null)
            {
                throw new InvalidHostIdException("No Host Record");
            }

            // Return value instantiate
            Appointment result = null;

            // check if appointment is existing 
            var existingAppointment = _appointmentServiceRepository.Retrieve(appointment.AppointmentId);
            if (existingAppointment != null)
            {
                result = _appointmentServiceRepository.Update(appointment.AppointmentId, appointment);
            }
            else
            {
                result = _appointmentServiceRepository.Create(appointment);
            }

            return result;
        }
    }
}