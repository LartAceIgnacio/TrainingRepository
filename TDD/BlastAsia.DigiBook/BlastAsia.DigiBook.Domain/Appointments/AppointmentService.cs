using BlastAsia.DigiBook.Domain.Contacts;
using BlastAsia.DigiBook.Domain.Employees;
using BlastAsia.DigiBook.Domain.Models.Appointments;
using BlastAsia.DigiBook.Domain.Models.Contacts;
using BlastAsia.DigiBook.Domain.Models.Employees;
using System;

namespace BlastAsia.DigiBook.Domain.Appointments
{
    public class AppointmentService
    {
        private IAppointmentRepository appointmentRepository;
        private IEmployeeRepository employeeRepository;
        private IContactRepository contactRepository;
        public AppointmentService(IAppointmentRepository appointmentRepository
            ,IEmployeeRepository employeeRepository
            ,IContactRepository contactRepository)
        {
            this.appointmentRepository = appointmentRepository;
            this.employeeRepository = employeeRepository;
            this.contactRepository = contactRepository;
        }

        public Appointment Save(Guid EmployeeId, Guid ContactId, Appointment appointment)
        {
            if (appointment.EndTime < appointment.StartTime)
            {
                throw new InclusiveTimeRequiredException("Inclusive Time is required");
            }
            if (appointment.AppointmentDate < DateTime.Today)
            {
                throw new AppointmentDateRequiredException("Appointment Date is invalid");
            }
            
            Appointment result = null;
            var foundGuest = contactRepository.Retrieve(appointment.GuestId);
            var foundHost = employeeRepository.Retrieve(appointment.HostId);
            var foundAppointment = appointmentRepository.Retrieve(appointment.AppointmentId);
            if (foundHost == null)
            {
                throw new InvalidHostIdException("Host Id is invalid");
            }
            if (foundGuest == null)
            {
                throw new InvalidGuestIdException("Guest Id is invalid");
            }
            if (foundAppointment == null)
            {
                result = appointmentRepository.Create(EmployeeId, ContactId, appointment);
            }
            else
            {
                result = appointmentRepository.Update(appointment.AppointmentId, appointment);
            }
            return result;

        }
    }
}