using System;
using BlastAsia.DigiBook.Domain.Contacts;
using BlastAsia.DigiBook.Domain.Employees;
using BlastAsia.DigiBook.Domain.Models.Appointments;
using BlastAsia.DigiBook.Domain.Models.Contacts;
using BlastAsia.DigiBook.Domain.Models.Employees;

namespace BlastAsia.DigiBook.Domain.Appointments
{
    public class AppointmentService
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

        public Appointment Save(Appointment appointment, Employee employee, Contact contact)
        {

            if (appointment.AppointmnetDate == null)
            {
                throw new AppointmentDateRequiredException("Appointment Date required");
            }
            else
            {
                if (appointment.AppointmnetDate < DateTime.Today)
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
                .Retrieve(appointment.AppointmentId);


            if (found == null)
            {
                if (employee.EmployeeId != null && contact.ContactId != null)
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