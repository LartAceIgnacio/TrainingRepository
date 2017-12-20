using BlastAsia.Digibook.Domain.Appointments;
using BlastAsia.Digibook.Domain.Contacts;
using BlastAsia.Digibook.Domain.Employees;
using BlastAsia.Digibook.Domain.Models.Appointments;
using BlastAsia.Digibook.Domain.Models.Contacts;
using BlastAsia.Digibook.Domain.Models.Employees;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BlastAsia.Digibook.Domain.Test.Appointments
{
    [TestClass]
    public class AppointmentServiceTest
    {
        private Mock<IAppointmentRepository> mockAppointmentRepository;
        private Mock<IContactRepository> mockContactRepository;
        private Mock<IEmployeeRepository> mockEmployeeRepository;
        private AppointmentService sut;
        private Appointment appointment;
        private Employee employee;
        private Contact contact;
        private Guid guestId = Guid.NewGuid();
        private Guid hostId = Guid.NewGuid();
        private Guid existingAppointmentId = Guid.NewGuid();
        private Guid nonExistingAppointmentId = Guid.Empty;
        private DateTime appointmentDate = new DateTime(2017, 12, 20);
        private DateTime startTime = new DateTime();
        private DateTime endTime = new DateTime();

        [TestInitialize]
        public void InitializeTest()
        {
            startTime = appointmentDate.AddHours(9);
            endTime = appointmentDate.AddHours(10);
            mockAppointmentRepository = new Mock<IAppointmentRepository>();
            mockContactRepository = new Mock<IContactRepository>();
            mockEmployeeRepository = new Mock<IEmployeeRepository>();
            sut = new AppointmentService(mockAppointmentRepository.Object,mockContactRepository.Object,mockEmployeeRepository.Object);

            appointment = new Appointment
            {
                AppointmentDate = appointmentDate,
                GuestId = guestId,
                HostId = hostId,
                StartTime = startTime,
                EndTime = endTime,
                IsCanceled = false,
                IsDone = false,
                Notes = "test"
            };

            contact = new Contact
            {
                FirstName = "Alex",
                LastName = "Cano",
                MobilePhone = "09173723594",
                StreetAddress = "16 J. Cruz St., Parang",
                CityAddress = "Marikina City",
                ZipCode = 1809,
                Country = "Philippines",
                EmailAddress = "gcano@blastasia.com",
                IsActive = false,
                DateActive = DateTime.Now
            };

            employee = new Employee
            {
                FirstName = "Glenn Alexander",
                LastName = "Cano",
                MobilePhone = "09173723594",
                EmailAddress = "gcano@blastasia.com",
                Photo = new MemoryStream(),
                OfficePhone = "9144456",
                Extension = "421"
            };

            mockAppointmentRepository
                .Setup(ar => ar.Create(appointment))
                .Callback(() => appointment.AppointmentId = Guid.NewGuid());

            mockAppointmentRepository
                .Setup(ar => ar.Retrieve(existingAppointmentId))
                .Returns(appointment);

            mockAppointmentRepository
                .Setup(ar => ar.Retrieve(nonExistingAppointmentId))
                .Returns<Appointment>(null);
        }

        [TestMethod]
        public void Set_NewAppointment_ShouldCreateAppointment()
        {
            employee.EmployeeId = existingAppointmentId;
            contact.ContactId = existingAppointmentId;
            appointment.HostId = employee.EmployeeId;
            appointment.GuestId = contact.ContactId;

            mockContactRepository
                .Setup(ar => ar.Retrieve(existingAppointmentId))
                .Returns(contact);

            mockEmployeeRepository
                .Setup(ar => ar.Retrieve(existingAppointmentId))
                .Returns(employee);

            var result = sut.Set(appointment);

            mockAppointmentRepository
                .Verify(c => c.Retrieve(nonExistingAppointmentId), Times.Once);

            mockAppointmentRepository
                .Verify(c => c.Create(appointment), Times.Once);
        }

        [TestMethod]
        public void Set_ExistingAppointment_ShouldUpdateAppointment()
        {
            employee.EmployeeId = existingAppointmentId;
            contact.ContactId = existingAppointmentId;
            appointment.HostId = employee.EmployeeId;
            appointment.GuestId = contact.ContactId;

            mockContactRepository
                .Setup(ar => ar.Retrieve(existingAppointmentId))
                .Returns(contact);

            mockEmployeeRepository
                .Setup(ar => ar.Retrieve(existingAppointmentId))
                .Returns(employee);

            appointment.AppointmentId = existingAppointmentId;
            var result = sut.Set(appointment);

            mockAppointmentRepository
                .Verify(c => c.Retrieve(appointment.AppointmentId), Times.Once);

            mockAppointmentRepository
                .Verify(c => c.Update(appointment.AppointmentId,appointment), Times.Once);
        }

        [TestMethod]
        public void Set_NewAppointment_ReturnsNewAppointmentWithId()
        {
            employee.EmployeeId = existingAppointmentId;
            contact.ContactId = existingAppointmentId;
            appointment.HostId = employee.EmployeeId;
            appointment.GuestId = contact.ContactId;

            mockContactRepository
                .Setup(ar => ar.Retrieve(existingAppointmentId))
                .Returns(contact);

            mockEmployeeRepository
                .Setup(ar => ar.Retrieve(existingAppointmentId))
                .Returns(employee);

            mockAppointmentRepository
                .Setup(a => a.Create(appointment))
                .Callback(() => appointment.AppointmentId = Guid.NewGuid())
                .Returns(appointment);

            sut = new AppointmentService(mockAppointmentRepository.Object, mockContactRepository.Object, mockEmployeeRepository.Object);

            var newAppointment = sut.Set(appointment);

            Assert.IsTrue(newAppointment.AppointmentId != Guid.Empty);
        }

        [TestMethod]
        public void Set_WithAppointmentDateLessThanToday_ThrowsInvalidAppointmentDateException()
        {
            appointment.AppointmentDate = DateTime.Now.AddDays(-1);

            Assert.ThrowsException<InvalidAppointmentDateException>(
                ()=>sut.Set(appointment));
            mockAppointmentRepository
                .Verify(a => a.Create(appointment), Times.Never);
        }

        [TestMethod]
        public void Set_WithNoGuestId_ThrowsUserDoesNotExistsException()
        {
            appointment.GuestId = nonExistingAppointmentId;

            Assert.ThrowsException<UserDoesNotExistsException>
                (() => sut.Set(appointment));
            mockAppointmentRepository
                .Verify(a => a.Create(appointment), Times.Never);
        }

        [TestMethod]
        public void Set_WithNoHostId_ThrowsUserDoesNotExistsException()
        {
            appointment.HostId = nonExistingAppointmentId;

            Assert.ThrowsException<UserDoesNotExistsException>
                (() => sut.Set(appointment));
            mockAppointmentRepository
                .Verify(a => a.Create(appointment), Times.Never);
        }

        [TestMethod]
        public void Set_WithStartTimeGreaterThanEndTime_ThrowsInvalidTimeRangeException()
        {
            appointment.StartTime = appointment.StartTime.AddHours(2);

            Assert.ThrowsException<InvalidTimeRangeException>
                (()=>sut.Set(appointment));
            mockAppointmentRepository
                .Verify(a => a.Create(appointment), Times.Never);
        }

        [TestMethod]
        public void Set_WithEndTimeGreaterThanEndTime_ThrowsInvalidTimeRangeException()
        {
            appointment.EndTime = appointment.EndTime.AddHours(-2);

            Assert.ThrowsException<InvalidTimeRangeException>
                (() => sut.Set(appointment));
            mockAppointmentRepository
                .Verify(a => a.Create(appointment), Times.Never);
        }

        [TestMethod]
        public void Set_WithSameStartAndEndTime_ThrowsInvalidTimeRangeException()
        {
            appointment.EndTime = appointment.StartTime;

            Assert.ThrowsException<InvalidTimeRangeException>
                (() => sut.Set(appointment));
            mockAppointmentRepository
                .Verify(a => a.Create(appointment), Times.Never);
        }

        [TestMethod]
        public void Set_WithAppointmentCanceled_ThrowsAppointmentCanceledException()
        {
            appointment.IsCanceled = true;

            Assert.ThrowsException<AppointmentCanceledException>
                (() => sut.Set(appointment));
            mockAppointmentRepository
                .Verify(a => a.Create(appointment), Times.Never);
        }

        [TestMethod]
        public void Set_WithAppointmentDone_ThrowsAppointmentDoneException()
        {
            appointment.IsDone = true;

            Assert.ThrowsException<AppointmentDoneException>
                (() => sut.Set(appointment));
            mockAppointmentRepository
                .Verify(a => a.Create(appointment), Times.Never);
        }

        [TestMethod]
        public void Set_WithNonExistingGuest_ThrowsUserDoesNotExistsException()
        {
            mockContactRepository
                .Setup(c => c.Retrieve(appointment.GuestId))
                .Returns<Contact>(null);
            mockAppointmentRepository
                .Verify(a => a.Create(appointment), Times.Never);
        }

        [TestMethod]
        public void Set_WithNonExistingHost_ThrowsUserDoesNotExistsException()
        {
            mockEmployeeRepository
                .Setup(e => e.Retrieve(appointment.HostId))
                .Returns<Employee>(null);
            mockAppointmentRepository
                .Verify(a => a.Create(appointment), Times.Never);
        }
    }
}
