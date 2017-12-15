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
            //check business rules
            if(appointment.AppointmentDate < DateTime.Today)
            {
                throw new ValidAppointmentDateRequiredException("Valid appointment date required.");
            }
            if(appointment.StartTime > appointment.EndTime)
            {
                throw new InclusiveStartTimeEndTimeRequiredException("Start time should be less than end time.");
            }
            if (appointment.EndTime < appointment.StartTime)
            {
                throw new InclusiveStartTimeEndTimeRequiredException("End time should be greater than end time.");
            }
            if (appointment.StartTime == appointment.EndTime)
            {
                throw new InclusiveStartTimeEndTimeRequiredException("Start time and end time should not be equal.");
            }

            var foundGuestId = contactRepository.Retrieve(appointment.GuestId);
            if (foundGuestId == null)
            {
                throw new GuestIdDoesNotExistException("Guest id not found.");
            }

            var foundHostId = employeeRepository.Retrieve(appointment.HostId);
            if (foundHostId == null)
            {
                throw new HostIdDoesNotExistException("Host id not found.");
            }

            Appointment currentAppointment = null;
            var foundAppointmentId = appointmentRepository.Retrieve(appointment.AppointmentId);

            //check if there is an existing appointment
            if (foundAppointmentId == null) //none existing
            {
                currentAppointment = appointmentRepository.Create(appointment);
            }
            else //existing
            {
                currentAppointment = appointmentRepository.Update(appointment.AppointmentId, appointment);
            }

            return currentAppointment;
        }
    }
}