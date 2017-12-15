using BlastAsia.DigiBook.Domain.Appointments.Exceptions;
using BlastAsia.DigiBook.Domain.Contacts;
using BlastAsia.DigiBook.Domain.Employees;
using BlastAsia.DigiBook.Domain.Models.Appointments;
using System;

namespace BlastAsia.DigiBook.Domain.Appointments
{
    public class AppointmentService
    {
        public IAppointmentRepository appointmentRepository;
        public IEmployeeRepository employeeRepository;
        public IContactRepository contactRepository;

        public AppointmentService(IAppointmentRepository appointmentRepository, 
            IEmployeeRepository employeeRepository,
            IContactRepository contactRepository)
        {
            this.appointmentRepository = appointmentRepository;
            this.employeeRepository = employeeRepository;
            this.contactRepository = contactRepository;
        }
        public void Create(Appointment appointment)
        {
            if (appointment.AppointmentDate < DateTime.Now)
            {
                throw new AppointmentDateException("Appointment Date Is Less Than Date Today");
            }
            if (appointment.StartTime > appointment.EndTime)
            {
                throw new TimeInclusiveException("Start Time Should Be Less Than End Time");
            }
            if (appointment.StartTime == appointment.EndTime)
            {
                throw new TimeInclusiveException("Start Time Should Not Be Equal");
            }

            Appointment Result = null;
            var AppointmentId = appointmentRepository.Retrieve(appointment.AppointmentId);
            var GuestId = contactRepository.Retrieve(appointment.GuestId);
            var HostId = employeeRepository.Retrieve(appointment.HostId);
            
            if (GuestId == null)
            {
                throw new GuestIdException("No Guest Id");
            }

            if (HostId == null)
            {
                throw new EmployeeIdException("No Employee Id");
            }


            if (HostId != null && GuestId != null && AppointmentId == null)
            {
                Result = appointmentRepository
                    .Create(appointment);
            }
            else if (HostId != null && GuestId != null && AppointmentId != null)
            {
                Result = appointmentRepository
                    .Update(appointment.AppointmentId, appointment);
            }
          
        }
    }
}