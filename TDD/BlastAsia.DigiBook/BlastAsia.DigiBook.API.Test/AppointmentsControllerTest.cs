using BlastAsia.DigiBook.API.Controllers;
using BlastAsia.DigiBook.Domain.Appointments;
using BlastAsia.DigiBook.Domain.Models.Appointments;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.API.Test
{
    [TestClass]
    public class AppointmentsControllerTest
    {
        private Appointment appointment;
        private Mock<IAppointmentRepository> mockAppointmentRepository;
        private Mock<IAppointmentService> mockAppointmentService;
        private JsonPatchDocument patchedAppointment;

        private AppointmentsController sut;

        private Guid existingAppointmentId = Guid.NewGuid();
        private Guid notExistingAppointmentId = Guid.Empty;

        [TestInitialize]
        public void TestInitialize()
        {
            mockAppointmentRepository = new Mock<IAppointmentRepository>();
            mockAppointmentService = new Mock<IAppointmentService>();
            patchedAppointment = new JsonPatchDocument();

            appointment = new Appointment
            {

                AppointmentId = Guid.NewGuid()
            };

            sut = new AppointmentsController(mockAppointmentService.Object,
               mockAppointmentRepository.Object);

            mockAppointmentRepository
           .Setup(x => x.Retrieve())
           .Returns(() => new List<Appointment>{
               new Appointment()});

            mockAppointmentRepository
            .Setup(x => x.Retrieve(appointment.AppointmentId))
            .Returns(appointment);

            mockAppointmentRepository
                .Setup(cr => cr.Retrieve(existingAppointmentId))
                .Returns(appointment);

            //Setup for Update
            mockAppointmentRepository
            .Setup(cr => cr.Retrieve(notExistingAppointmentId))
            .Returns<Appointment>(null);

            //Setup for Delete
            mockAppointmentRepository
                .Setup(cr => cr.Retrieve(existingAppointmentId))
                .Returns(appointment);
        }
        [TestCleanup()]
        public void TestCleanup()
        {

        }
        [TestMethod]
        public void GetAppointments_WithEmptyAppointmentId_ReturnsOkObjectResult()
        {
            // Act
            var result = sut.GetAppointment(null);

            // Assert              
            mockAppointmentRepository
               .Verify(c => c.Retrieve(), Times.Once());

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void GetAppointment_WithAppointmentId_ReturnsObjectResult()
        {
            // Act
            var result = sut.GetAppointment(notExistingAppointmentId);

            // Assert
            mockAppointmentRepository
               .Verify(c => c.Retrieve(notExistingAppointmentId), Times.Once());

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void CreateAppointment_WithEmptyAppointment_ReturnsBadRequestResult()
        {
            appointment = null;
            var result = sut.CreateAppointment(appointment);

            // Assert
            mockAppointmentService
               .Verify(c => c.Save(Guid.Empty, appointment), Times.Never());

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void CreateAppointment_WithValidAppointment_ReturnsObjectResult()
        {

            mockAppointmentService
              .Setup(cs => cs.Save(Guid.Empty, appointment))
              .Returns(appointment);

            //Act
            var result = sut.CreateAppointment(appointment);

            // Assert         
            mockAppointmentService
             .Verify(c => c.Save(Guid.Empty, appointment), Times.Once());

            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
        }

        [TestMethod]
        public void UpdateAppointment_WithValidAppointment_ReturnsObjectResult()
        {

            var result = sut.UpdateAppointment(appointment, existingAppointmentId);
            // Assert

            mockAppointmentRepository
                .Verify(c => c.Retrieve(existingAppointmentId), Times.Once());

            mockAppointmentService
                .Verify(c => c.Save(existingAppointmentId, appointment), Times.Once());

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void UpdateAppointment_WithEmptyAppointment_ReturnsBadRequestResult()
        {
            appointment = null;
            // Act
            var result = sut.UpdateAppointment(appointment, existingAppointmentId);

            // Assert
            mockAppointmentRepository
                .Verify(c => c.Update(existingAppointmentId, appointment), Times.Never());

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }
        [TestMethod]
        public void UpdateAppointment_WithEmptyAppointmentId_ReturnsNotFoundResult()
        {
            var result = sut.UpdateAppointment(appointment, notExistingAppointmentId);

            // Assert
            mockAppointmentRepository
                .Verify(c => c.Update(notExistingAppointmentId, appointment), Times.Never());

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }
        [TestMethod]
        public void DeleteAppointment_WithAppointmentId_ReturnsNoContentResult()
        {
            // Act
            var result = sut.DeleteAppointment(existingAppointmentId);

            //Assert          
            mockAppointmentRepository
                .Verify(c => c.Delete(existingAppointmentId), Times.Once());

            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }

        [TestMethod]
        public void DeleteAppointment_WithEmptyAppointmentId_ReturnsNotFound()
        {
            //Act
            var result = sut.DeleteAppointment(notExistingAppointmentId);

            // Assert 
            mockAppointmentRepository
                .Verify(c => c.Delete(notExistingAppointmentId),
                Times.Never());

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void PatchAppointment_WithValidPatchedAppointment_ReturnsObjectResult()
        {
            var result = sut.PatchAppointment(patchedAppointment, existingAppointmentId);
            // Assert

            mockAppointmentRepository
                .Verify(c => c.Retrieve(existingAppointmentId), Times.Once());

            mockAppointmentService
                .Verify(c => c.Save(existingAppointmentId, appointment), Times.Once());

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

        }
        [TestMethod]
        public void PatchAppointment_WithEmptyPatchedAppointment_ReturnsBadRequestResult()
        {
            patchedAppointment = null;
            // Act
            var result = sut.PatchAppointment(patchedAppointment, existingAppointmentId);

            // Assert
            mockAppointmentRepository
               .Verify(c => c.Retrieve(notExistingAppointmentId), Times.Never());

            mockAppointmentService
                .Verify(c => c.Save(notExistingAppointmentId, appointment), Times.Never());

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void PatchAppointment_WithInvalidAppointmentId_ReturnsNotFound()
        {
            var result = sut.PatchAppointment(patchedAppointment, notExistingAppointmentId);

            //Assert
            mockAppointmentRepository
                .Verify(c => c.Retrieve(notExistingAppointmentId), Times.Once());

            mockAppointmentService
                 .Verify(c => c.Save(notExistingAppointmentId, appointment), Times.Never());

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }
    }
}
