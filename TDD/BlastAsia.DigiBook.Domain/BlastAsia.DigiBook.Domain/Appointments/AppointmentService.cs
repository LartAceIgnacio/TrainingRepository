using System;
using System.Collections.Generic;
using System.Text;
using BlastAsia.DigiBook.Domain.Models.Appointments;
using BlastAsia.DigiBook.Domain.Appointments.Exceptions;
using BlastAsia.DigiBook.Domain.Contacts.Interfaces;
using BlastAsia.DigiBook.Domain.Employees;

namespace BlastAsia.DigiBook.Domain.Appointments
{
    public class AppointmentService
    {
        private IAppointmentRepository _appointmentRepository;
        private IContactRepository _contactRepository;
        private IEmployeeRepository _employeeRepository;

        public AppointmentService(
            IAppointmentRepository appointmentRepository,
            IContactRepository contactRepository, 
            IEmployeeRepository employeeRepository)
        {
            _appointmentRepository = appointmentRepository;
            _employeeRepository = employeeRepository;
            _contactRepository = contactRepository;
        }

        public Appointment Save(Appointment appointment)
        {

            if (appointment.AppointmentDate < DateTime.Now)
            {
                throw new InvalidAppointmentDateException("Invalid Appointment date exception");
            }
            if (appointment.EndTime <= appointment.StartTime)
            {
                throw new InvalidStartAndEndTimeException("Invalid start and end time!");
            }


            Appointment result = null;

            var retrieveContact = _contactRepository.Retrieve(appointment.GuestId);
            var retrieveEmployee = _employeeRepository.Retrieve(appointment.HostId);
            var retrieveAppointment = _appointmentRepository.Retrieve(appointment.AppointmentId);

            if (retrieveContact == null)
            {
                throw new InvalidGuestIdException("Invalid Guest ID!");
            }
            if (retrieveEmployee == null)
            {
                throw new InvalidHostIdException("Invalid Host ID!");
            }
            if (retrieveAppointment != null)
            {
                result = _appointmentRepository.Update(appointment.AppointmentId);
            }
            else
            { 
                result = _appointmentRepository.Create(appointment);
            }

            
        
            return result;
        }
    }
}
