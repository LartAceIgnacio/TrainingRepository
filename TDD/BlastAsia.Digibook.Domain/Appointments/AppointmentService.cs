using BlastAsia.Digibook.Domain.Contacts;
using BlastAsia.Digibook.Domain.Employees;
using BlastAsia.Digibook.Domain.Models.Appointments;
using System;

namespace BlastAsia.Digibook.Domain.Appointments
{
    public class AppointmentService
    {
        private IAppointmentRepository appointmentRepository;
        private IContactRepository contactRepository;
        private IEmployeeRepository employeeRepository;

        public AppointmentService(IAppointmentRepository appointmentRepository,IContactRepository contactRepository,
            IEmployeeRepository employeeRepository)
        {
            this.appointmentRepository = appointmentRepository;
            this.contactRepository = contactRepository;
            this.employeeRepository = employeeRepository;
        }

        public Appointment Set(Appointment appointment)
        {
            if(appointment.AppointmentDate < DateTime.Now)
            {
                throw new InvalidAppointmentDateException();
            }

            if(appointment.GuestId == Guid.Empty)
            {
                throw new UserDoesNotExistsException("Guest Id does not exist.");
            }

            if (appointment.HostId == Guid.Empty)
            {
                throw new UserDoesNotExistsException("Host Id does not exist.");
            }

            if(appointment.StartTime > appointment.EndTime)
            {
                throw new InvalidTimeRangeException("Appointment start time cannot be later than the appointment's end time");
            }

            if(appointment.StartTime == appointment.EndTime)
            {
                throw new InvalidTimeRangeException("Appointment start and end time cannot be the same.");
            }

            if (appointment.IsCanceled)
            {
                throw new AppointmentCanceledException();
            }

            if (appointment.IsDone)
            {
                throw new AppointmentDoneException();
            }

            var foundGuest = contactRepository.Retrieve(appointment.GuestId);
            if(foundGuest == null)
            {
                throw new UserDoesNotExistsException("Guest does not exists.");
            }

            var foundHost = employeeRepository.Retrieve(appointment.HostId);
            if(foundHost == null)
            {
                throw new UserDoesNotExistsException("Host does not exists.");
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
                result = appointmentRepository.Update(appointment, appointment.AppointmentId);
            }

            return result;
        }
    }
}