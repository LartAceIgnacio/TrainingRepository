using BlastAsia.DigiBook.Domain.Contacts;
using BlastAsia.DigiBook.Domain.Employees;
using BlastAsia.DigiBook.Domain.Models.Appointments;
using BlastAsia.DigiBook.Domain.Models.Contacts;
using BlastAsia.DigiBook.Domain.Models.Employees;
using System;

namespace BlastAsia.DigiBook.Domain.Appointments
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository appointmentRepository;
        private readonly IEmployeeRepository employeeRepository;
        private readonly IContactRepository contactRepository;
        public AppointmentService(IAppointmentRepository appointmentRepository
            ,IEmployeeRepository employeeRepository
            ,IContactRepository contactRepository)
        {
            this.appointmentRepository = appointmentRepository;
            this.employeeRepository = employeeRepository;
            this.contactRepository = contactRepository;
        }

        public Appointment Save(Guid id, Appointment appointment)
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
            var foundHost = employeeRepository.Retrieve(appointment.HostId);
            if (foundHost == null)
            {
                throw new InvalidHostIdException("Host Id is invalid");
            }

            var foundGuest = contactRepository.Retrieve(appointment.GuestId);
            if (foundGuest == null)
            {
                throw new InvalidGuestIdException("Guest Id is invalid");
            }

            var foundAppointment = appointmentRepository.Retrieve(id);
            if (foundAppointment == null)
            {
                result = appointmentRepository.Create(appointment);
            }
            else
            {
                result = appointmentRepository.Update(id, appointment);
            }
            return result;

        }
    }
}