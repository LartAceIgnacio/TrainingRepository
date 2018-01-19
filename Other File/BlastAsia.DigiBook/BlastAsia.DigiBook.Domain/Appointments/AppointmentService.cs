using System;
using BlastAsia.DigiBook.Domain.Models.Appointments;
using BlastAsia.DigiBook.Domain.Contacts;
using BlastAsia.DigiBook.Domain.Employees;

namespace BlastAsia.DigiBook.Domain.Appointments
{
    public class AppointmentService: IAppointmentService
    {
        private IAppointmentRepository appointmentRepository;
        private IContactRepository guestRepository;
        private IEmployeeRepository hostRepository;

        public AppointmentService(IAppointmentRepository appointmentRepository, IContactRepository guestRepository, IEmployeeRepository hostRepository)
        {
            this.appointmentRepository = appointmentRepository;
            this.guestRepository = guestRepository;
            this.hostRepository = hostRepository;
        }

        public Appointment Save(Guid id, Appointment appointment)
        {
            if (appointment.AppointmentDate < DateTime.Today)
            {
                throw new AppointmentDateLapsedAlreadyException("Date Already Lapsed");
            }
            if (appointment.StartTime.ToString(@"hh\:mm") == appointment.EndTime.ToString(@"hh\:mm"))
            {
                throw new InclusiveTimeException("Start Time and End Time should not be equal");
            }
            if (appointment.StartTime > appointment.EndTime)
            {
                throw new InclusiveTimeException("Start time is greater than End Time");
            }

            var resultGuest = guestRepository.Retrieve(appointment.GuestId);

            if (resultGuest == null)
            {
                throw new NonExistingContactException("No Guest Information Available");
            }

            var resultHost = hostRepository.Retrieve(appointment.HostId);

            if ( resultHost == null)
            {
                throw new NonExistingEmployeeException("No Host Information Available");
            }
            
            Appointment result = null;
            var found = appointmentRepository
                .Retrieve(appointment.AppointmentId);


            if (found == null)
            {
                result = appointmentRepository.Create(appointment);
            }
            else
            {
                result = appointmentRepository
                    .Update(appointment.AppointmentId, appointment);
            }
            return result;
        }
        
    }
}