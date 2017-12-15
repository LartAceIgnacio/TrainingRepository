using BlastAsia.DigiBook.Domain.Appointments.Adapters;
using BlastAsia.DigiBook.Domain.Appointments.AppointmentExceptions;
using BlastAsia.DigiBook.Domain.Contacts;
using BlastAsia.DigiBook.Domain.Employees;
using BlastAsia.DigiBook.Domain.Models.Appointments;
using System;

namespace BlastAsia.DigiBook.Domain.Appointments.Services
{
    public class AppointmentService : IAppointmentService
    {
        private IContactRepository _contactRepo;
        private IEmployeeRepository _employeeRepo;
        private IAppointmentRepository _appointmentRepo;
        private IDateTimeWrapper _datetimewrapper;

        public AppointmentService(IContactRepository contactRepo, IEmployeeRepository employeeRepo
            , IAppointmentRepository appointmentRepo, IDateTimeWrapper datetimewrapper)
        {
            this._contactRepo = contactRepo;
            this._employeeRepo = employeeRepo;
            this._appointmentRepo = appointmentRepo;
            this._datetimewrapper = datetimewrapper;
        }

        public Appointment Save(Appointment appointment)
        {
            if (appointment.EndTime <= appointment.StartTime) throw new InvalidTimeScheduleException("Endtime must not less than start time");
            if (appointment.AppointmentDate < _datetimewrapper.GetNow()) throw new InvalidTimeScheduleException("Appointment date must be ahead of current date");

            Appointment resultAppointment = null;

            var foundAppointment = _appointmentRepo.Retrieve(appointment.AppointmentId);
            var foundGuest = _contactRepo.Retrieve(appointment.GuestId);
            var foundHost = _employeeRepo.Retrieve(appointment.HostId);


            if (foundHost == null) throw new HostRequiredException("Host is required.");
            if (foundGuest == null) throw new GuestRequiredException("Guest is required.");

            if (foundAppointment == null && foundGuest != null && foundHost != null)
            {
                resultAppointment = _appointmentRepo.Create(appointment);
            }

            return resultAppointment;
        }
    }
}
