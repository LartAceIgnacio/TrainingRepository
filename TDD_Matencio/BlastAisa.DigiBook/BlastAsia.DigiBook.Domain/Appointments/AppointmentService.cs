using System;
using BlastAsia.DigiBook.Domain.Models.Employees;
using System.Linq;
using BlastAsia.DigiBook.Domain.Employees;
using BlastAsia.DigiBook.Domain.Contacts;
using BlastAsia.DigiBook.Domain.Models.Appointments;

namespace BlastAsia.DigiBook.Domain.Appointments
{
    public class AppointmentService
    {
        private IAppointmentRepository appointmentRepository;
        private IEmployeeRepository employeeRepository;
        private IContactRepository contactRepository;


        public AppointmentService(
            IAppointmentRepository appointmentRepository,
            IEmployeeRepository employeeRepository,
            IContactRepository contactRepository)
        {
            this.appointmentRepository = appointmentRepository;
            this.employeeRepository = employeeRepository;
            this.contactRepository = contactRepository;
        }

        public Appointment Save(Appointment appointment)
        {
            if(appointment.AppointmentDate <DateTime.Today)
            {
                throw new InvalidAppointmentDateException("Appointment date should be greater than or equal today.");
            }
            if (appointment.StartTime >= appointment.EndTime)
            {
                throw new InvalidTimeException("Invalid time input.");
            }
           
            Appointment result = null;
            
            var foundGuestId = contactRepository.Retrieve(appointment.GuestId);

            if (foundGuestId == null)
            {
                throw new InvalidGuestIdException("Guest ID should not be empty.");
            }


            var foundEmployeeId = employeeRepository.Retrieve(appointment.HostId);

            if (foundEmployeeId == null)
            {
                throw new InvalidEmployeeIdException("Employee ID should not be empty.");
            }

            var foundAppointmentId = appointmentRepository.Retrieve(appointment.appointmentId);

            if (foundAppointmentId == null)
            {
                result = appointmentRepository.Create(appointment);
            }
            else
            {
                result = appointmentRepository.Update(appointment.appointmentId, appointment);
            }

            return result;
        }
    }
}