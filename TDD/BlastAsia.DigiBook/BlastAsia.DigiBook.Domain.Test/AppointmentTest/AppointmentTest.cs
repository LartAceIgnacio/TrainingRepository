using BlastAsia.DigiBook.Domain.Appointments.Adapters;
using BlastAsia.DigiBook.Domain.Appointments.Services;
using BlastAsia.DigiBook.Domain.Contacts;
using BlastAsia.DigiBook.Domain.Employees;
using BlastAsia.DigiBook.Domain.Models.Appointments;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Test.AppointmentTest
{
    public class AppointmentTest
    {
        private Appointment _appointment;
        private AppointmentService _sut;

        private IContactRepository _contactRepository;
        private IEmployeeRepository _employeeRepository;
        private IAppointmentRepository _appointmentRepo;
        private IDateTimeWrapper _dateTimeWrapper;

        private Mock<IContactRepository> _mockContactRepo;
        private Mock<IEmployeeRepository> _mockEmployeeRepo;


        private Guid _appointmentId;
        private DateTime _appointmentDate;

        [TestInitialize]
        public void Initialize()
        {
            _appointmentId = Guid.NewGuid();
            _appointmentDate = _dateTimeWrapper.GetDate();

            _appointment = new Appointment
            {
                AppointmentId = _appointmentId,
                AppointmentDate = _appointmentDate,
                GuestId = Guid.NewGuid(),
                HostId = Guid.NewGuid(),
                StartTime = _dateTimeWrapper.GetTime,
                EndTime = _dateTimeWrapper.GetTime,
                IsCancelled = false,
                IsDone = false,
                Notes = ""
            };

        }

       
    }
}
