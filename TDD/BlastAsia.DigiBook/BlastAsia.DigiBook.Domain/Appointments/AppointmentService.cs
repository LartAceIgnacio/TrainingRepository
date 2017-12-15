using BlastAsia.DigiBook.Domain.Contacts;
using BlastAsia.DigiBook.Domain.Employees;
using BlastAsia.DigiBook.Domain.Models.Appointments;
using System;

namespace BlastAsia.DigiBook.Domain.Appointments
{
    public class AppointmentService
    {
        private IAppointmentRepository appointmentRepository;
        private IContactRepository contactRepository;
        private IEmployeeRepository employeeRepository;

        public AppointmentService(IAppointmentRepository appointmentRepository, IContactRepository contactRepository, IEmployeeRepository employeeRepository)
        {
            this.appointmentRepository = appointmentRepository;
            this.contactRepository = contactRepository;
            this.employeeRepository = employeeRepository;
        }

        

        public Appointment Save(Appointment appointment)
        {
            if (appointment.AppointmentDate < DateTime.Today)
            {
                throw new ValidAppointmentDateRequiredException("Scheduled appointment must be later than the current time today.");
            }
            if (appointment.StartTime > appointment.EndTime)
            {
                throw new InclusiveStartAndEndTimeException("Start time should be earlier than end time.");
            }
            if (appointment.StartTime == appointment.EndTime)
            {
                throw new InclusiveStartAndEndTimeException("Start time should be earlier than end time.");
            }

            var foundGuestId = contactRepository
                .Retrieve(appointment.GuestId);
            if (foundGuestId == null)
            {
                throw new GuestIdRequiredException("Guest Id is required.");
            }

            var foundHostId = employeeRepository
                .Retrieve(appointment.HostId);
            if (foundHostId == null)
            {
                throw new HostIdRequiredException("Host Id is required.");
            }

            Appointment result = null;
            var found = appointmentRepository
                .Retrieve(appointment.AppointmentId);

            if (found == null)
            {
                result = appointmentRepository.Create(appointment);
            }
            else
            {
                result = appointmentRepository
                    .Update(appointment.AppointmentId, appointment);
            }
            return result;

        }
    }
}