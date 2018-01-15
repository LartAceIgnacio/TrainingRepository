using BlastAsia.DigiBook.Domain.Appointments;
using BlastAsia.DigiBook.Domain.Contacts;
using BlastAsia.DigiBook.Domain.Employees;
using BlastAsia.DigiBook.Domain.Models.Appointments;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain
{
    public class AppointmentService : IAppointmentService
    {
        public IAppointmentRepository appointmentRepository;
        public IEmployeeRepository employeeRepository;
        public IContactRepository contactRepository;

        public AppointmentService(IAppointmentRepository appointmentRepository, IEmployeeRepository employeeRepository, IContactRepository contactRepository)
        {
            this.appointmentRepository = appointmentRepository;
            this.employeeRepository = employeeRepository;
            this.contactRepository = contactRepository;
        }

        public Appointment Save(Guid id, Appointment appointment)
        {
            if (appointment.AppointmentDate < DateTime.Today)
                throw new AppointmentDateLessThanDateTodayException("Appointment Date should be greater than or equal to Date Today!");

            if (appointment.StartTime >= appointment.EndTime)
                throw new TimeInclusiveException("Start Time should be less than than to End Time!");

            if (appointment.EndTime <= appointment.StartTime)
                throw new TimeInclusiveException("End Time should be greater than than to Start Time!");

            Appointment resultAppointment = null;

            var foundGuest = contactRepository.Retrieve(appointment.GuestId);
            if (foundGuest == null) {
                throw new ContactIdNotExistedException("No Guest Found!");
            }

            var foundHost = employeeRepository.Retrieve(appointment.HostId);
            if (foundHost == null) {
                throw new EmployeeIdNotExistedException("No Host Found!");
            }

            //var foundAppointment = appointmentRepository.Retrieve(id);

            if (id == null || id == Guid.Empty) {
                resultAppointment = appointmentRepository.Create(appointment);
            }
            else {
                resultAppointment = appointmentRepository.Update(appointment.AppointmentId, appointment);
            }

            return resultAppointment;
        }
    }
}
