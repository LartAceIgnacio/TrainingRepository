using System;
using BlastAsia.DigiBook.Domain.Contacts;
using BlastAsia.DigiBook.Domain.Employees;
using BlastAsia.DigiBook.Domain.Models.Appointments;
using BlastAsia.DigiBook.Domain.Models.Contacts;
using BlastAsia.DigiBook.Domain.Models.Employees;

namespace BlastAsia.DigiBook.Domain.Appointments
{
    public class AppointmentService : IAppointmentService
    {
        private IAppointmentRepository appointmentRepository;
        private IEmployeeRepository employeeRepository;
        private IContactRepository contactRepository;
        public AppointmentService(IAppointmentRepository appointmentRepository
            ,IEmployeeRepository employeeRepository
            , IContactRepository contactRepository)
        {
            this.appointmentRepository = appointmentRepository;
            this.employeeRepository = employeeRepository;
            this.contactRepository = contactRepository;
        }

        public Appointment Save(Guid id, Appointment appointment)
        {

            if (appointment.AppointmentDate == null)
            {
                throw new AppointmentDateRequiredException("Appointment Date required");
            }
            else
            {
                if (appointment.AppointmentDate < DateTime.Today)
                {
                    throw new AppointmentDateRequiredException("Appointment Date must greater than today");
                }
            }
            if (appointment.StartTime == null)
            {
                throw new StartDateRequiredException("Start Date required");
            }
            
            if (appointment.EndTime == null)
            {
                throw new EndDateRequiredException("Start Date required");
            }

            if (appointment.StartTime > appointment.EndTime)
            {
                throw new StartDateRequiredException("Start Date must less than End Time");

            }

 
            Appointment result = null;

            var found = appointmentRepository
                .Retrieve(id);

            var foundHost = employeeRepository.Retrieve(appointment.HostId);
            var foundGuest = contactRepository.Retrieve(appointment.GuestId);

            if(foundHost == null)
            {
                throw new HostIdRequiredException("Host id is required");
            }

            if (foundGuest == null)
            {
                throw new GuestIdRequiredException("Guest ID is required");
            }

            if (found == null)
            {
                if (appointment.GuestId != null && appointment.HostId != null)
                {
                    result = appointmentRepository.Create(appointment);
                }
                
            }
            else
            {
                result = appointmentRepository.Update(appointment.AppointmentId, appointment);
            }

            return result;


            
        }
    }
}