
using BlastAsia.DigiBook.Domain.Appointments;
using BlastAsia.DigiBook.Domain.Appointments.Exceptions;
using BlastAsia.DigiBook.Domain.Contacts;
using BlastAsia.DigiBook.Domain.Employees;
using BlastAsia.DigiBook.Domain.Models.Appointments;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Appointments
{
    public class AppointmentService : IAppointmentService
    {
        private IAppointmentRepository appointmentRepository;
        public IEmployeeRepository employeeRepository;
        public IContactRepository contactRepository;

        public AppointmentService(IAppointmentRepository appointmentRepository, IEmployeeRepository employeeRepository, 
            IContactRepository contactRepository)
        {
            this.appointmentRepository = appointmentRepository;
            this.employeeRepository = employeeRepository;
            this.contactRepository = contactRepository;
        }

        public Appointment Save(Guid id,Appointment appointment)
        {
            //Appointment Date
            if (appointment.AppointmentDate <= DateTime.Today)
            {
                throw new AppointmentDateLessThanDateTodayException("Appointment Date should be greater or equal to Date Today!");
            }
            if (appointment.StartTime >= appointment.EndTime)
            {
                throw new AppointmentDateException("Start Time should be less than than to End Time!");
            }
            if (appointment.EndTime <= appointment.StartTime)
            {
                throw new AppointmentDateException("End Time should be greater than to Start Time!");
            }

            
            Appointment resultAppointment = null;

            // Repositories
            var foundGuest = contactRepository.Retrieve(appointment.GuestId);
            var foundHost = employeeRepository.Retrieve(appointment.HostId);

            // Check if GuestID is not null
            if (foundGuest == null)
            {
                throw new ContactIdRequiredException("Contact ID is required!");
            }
            // Check if HostID is not null
            if (foundHost == null)
            {
                throw new EmployeeIdRequiredException("Employee ID is required!");
            }
           

            var foundAppointment = appointmentRepository.Retrieve(id);

            // Create Appointment if ID of Appointment is not exist but the 2 ID exist

            if (foundAppointment == null)
            {
                resultAppointment = appointmentRepository.Create(appointment);
            }
            // Update Appointment if all ID is existing
           if (foundAppointment != null)
            {
                resultAppointment = appointmentRepository.Update(id, appointment);
            }

            return resultAppointment;

        }
    }
}

