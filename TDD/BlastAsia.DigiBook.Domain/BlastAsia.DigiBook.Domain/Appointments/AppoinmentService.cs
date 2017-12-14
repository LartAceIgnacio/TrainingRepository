using System;
using BlastAsia.DigiBook.Domain.Models.Appoinments;
using BlastAsia.DigiBook.Domain.Employees;
using BlastAsia.DigiBook.Domain.Contacts;

namespace BlastAsia.DigiBook.Domain.Appointments
{
    public class AppoinmentService
    {
        public IAppoinmentRepository appoinmentRepository;
        public IEmployeeRepository employeeRepository;
        public IContactRepository contactRepository;

        public AppoinmentService(IAppoinmentRepository appoinmentRepository, IEmployeeRepository employeeRepository, IContactRepository contactRepository)
        {
            this.appoinmentRepository = appoinmentRepository;
            this.employeeRepository = employeeRepository;
            this.contactRepository = contactRepository;
        }

        public void Save(Appoinment appoinment)
        {
            if (appoinment.AppointmentDate < DateTime.Today)
                throw new AppointmentDateLessThanDateTodayException("Appointment Date should be greater than or equal to Date Today!");

            if(appoinment.StartTime >= appoinment.EndTime)
                throw new TimeInclusiveException("Start Time should be less than than to End Time!");

            if (appoinment.EndTime <= appoinment.StartTime)
                throw new TimeInclusiveException("End Time should be greater than than to Start Time!");

            Appoinment resultAppointment = null;

            var foundGuest = contactRepository.Retrieve(appoinment.GuestId);
            var foundHost = employeeRepository.Retrieve(appoinment.HostId);

            if (foundGuest == null) {
                throw new ContactIdNotExistedException("No Guest Found!");
            }

            if (foundHost == null) {
                throw new EmployeeIdNotExistedException("No Host Found!");
            }
            
            var foundAppointment = appoinmentRepository.Retrieve(appoinment.AppointmentId);

            if (foundHost != null && foundGuest != null && foundAppointment == null) {
                resultAppointment = appoinmentRepository.Create(appoinment);
            }
            else if (foundHost != null && foundGuest != null && foundAppointment != null) {
                resultAppointment = appoinmentRepository.Update(appoinment.AppointmentId, appoinment);
            }
        }
    }
}