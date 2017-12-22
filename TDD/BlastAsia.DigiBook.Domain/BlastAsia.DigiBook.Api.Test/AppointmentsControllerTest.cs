using BlastAsia.DigiBook.Api.Controllers;
using BlastAsia.DigiBook.Domain.Appointments;
using BlastAsia.DigiBook.Domain.Models.Appointments;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Api.Test
{
    [TestClass]
    public class AppointmentsControllerTest
    {
        private readonly Guid existingAppointmentId = Guid.NewGuid();
        private readonly Guid nonExistingAppointmentId = Guid.Empty;
        private Mock<IAppointmentRepository> mockAppointmentRepository;
        private Mock<IAppointmentService> appointmentService;
        private AppointmentsController sut;

        private Appointment appointment = new Appointment();
        private List<Appointment> appointmentList = new List<Appointment>();

        [TestInitialize]
        public void InitializeTest()
        {
            mockAppointmentRepository = new Mock<IAppointmentRepository>();
            appointmentService = new Mock<IAppointmentService>();

            sut = new AppointmentsController(appointmentService.Object, mockAppointmentRepository.Object);

            //GetAppointments with id
            mockAppointmentRepository
              .Setup(cr => cr.Retrieve(existingAppointmentId))
              .Returns(appointment);

            //GetAppointments without id
            mockAppointmentRepository
                .Setup(cr => cr.Retrieve())
                .Returns(appointmentList);

            //Update with existingId
            mockAppointmentRepository
                .Setup(cr => cr.Retrieve(existingAppointmentId))
                .Returns(appointment);

            //Update without existingId
            mockAppointmentRepository
                .Setup(cr => cr.Retrieve(nonExistingAppointmentId))
                .Returns<Appointment>(null);

            //CreatAppointment with null Appointment
            mockAppointmentRepository
                .Setup(cr => cr.Create(null))
                .Returns<Appointment>(null);


            //CreatAppointment with valid Appointment
            mockAppointmentRepository
                .Setup(cr => cr.Create(appointment))
                .Returns(appointment);


        }
        [TestMethod]
        public void GetAppointments_WithValidId_ShouldReturnOkObjectValue()
        {
            //Act
            var result = sut.GetAppointments(existingAppointmentId);

            //Assert
            mockAppointmentRepository
                .Verify(cr => cr.Retrieve(existingAppointmentId), Times.Once);

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void GetAppointments_WithoutId_ShouldReturnOkObjectValue()
        {
            //Act
            var result = sut.GetAppointments(null);

            //Assert
            mockAppointmentRepository
                .Verify(cr => cr.Retrieve(), Times.Once);

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void UpdateAppointment_WithValidId_ShouldReturnOkObjectValue()
        {


            //Act
            var result = sut.UpdateAppointment(appointment, existingAppointmentId);


            //Assert 
            mockAppointmentRepository
                .Verify(cr => cr.Retrieve(existingAppointmentId), Times.Once);

            appointmentService
               .Verify(cr => cr.Save(existingAppointmentId, appointment), Times.Once);

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }
        [TestMethod]
        public void UpdateAppointment_WithNullAppointment_ShouldReturnBadRequest()
        {
            //Arrange
            appointment = null;
            //act
            var result = sut.UpdateAppointment(appointment, nonExistingAppointmentId);

            //Assert
            mockAppointmentRepository
                .Verify(cr => cr.Retrieve(nonExistingAppointmentId), Times.Never);

            appointmentService
                .Verify(cs => cs.Save(nonExistingAppointmentId, appointment), Times.Never);

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }
        [TestMethod]
        public void UpdateAppointment_WithInvalidId_ShouldReturnBadRequest()
        {
            //Act
            var result = sut.UpdateAppointment(appointment, nonExistingAppointmentId);

            //Assert
            mockAppointmentRepository
                .Verify(cr => cr.Retrieve(nonExistingAppointmentId), Times.Once);

            appointmentService
                .Verify(cs => cs.Save(nonExistingAppointmentId, appointment), Times.Never);

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void CreateAppointment_WithNullAppointment_ShouldReturnBadRequest()
        {
            //Arrange
            appointment = null;

            //Act
            var result = sut.CreateAppointment(appointment);


            //Assert
            appointmentService
                .Verify(cs => cs.Save(Guid.Empty, appointment), Times.Never);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void CreateAppointment_WithValidAppointment_ShouldReturnCreatedAtActionResult()
        {
            //Act
            var result = sut.CreateAppointment(appointment);


            //Assert  
            appointmentService
                .Verify(cs => cs.Save(Guid.Empty, appointment), Times.Once);

            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
        }

        [TestMethod]
        public void DeleteAppointment_WithValidAppointmentId_ShouldReturnNoContentResult()
        {
            //Act
            var result = sut.DeleteAppointment(existingAppointmentId);


            //Assert
            mockAppointmentRepository
                .Verify(cr => cr.Retrieve(existingAppointmentId), Times.Once);

            mockAppointmentRepository
                .Verify(cr => cr.Delete(existingAppointmentId), Times.Once);
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }

        [TestMethod]
        public void DeleteAppointment_WithInvalidAppointmentId_ShouldReturnBadRequest()
        {
            //Act
            var result = sut.DeleteAppointment(nonExistingAppointmentId);

            //Assert
            mockAppointmentRepository
                .Verify(cr => cr.Retrieve(nonExistingAppointmentId), Times.Once);

            mockAppointmentRepository
                .Verify(cr => cr.Delete(nonExistingAppointmentId), Times.Never);

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));

        }

        [TestMethod]
        public void PatchAppointment_WithValidAppointmentId_ShouldReturnOkObjectValue()
        {
            //Arrange
            var patchDoc = new JsonPatchDocument();

            //Act
            var result = sut.PatchAppointment(patchDoc, existingAppointmentId);

            //Assert
            mockAppointmentRepository
                .Verify(cr => cr.Retrieve(existingAppointmentId), Times.Once);

            appointmentService
                .Verify(cs => cs.Save(existingAppointmentId, appointment), Times.Once);

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

        }

        [TestMethod]
        public void PatchAppointment_WithInvalidAppointmentId_ShouldReturnBadRequest()
        {
            var patchDoc = new JsonPatchDocument();

            //Act
            var result = sut.PatchAppointment(patchDoc, nonExistingAppointmentId);

            //Assert
            mockAppointmentRepository
                .Verify(cr => cr.Retrieve(nonExistingAppointmentId), Times.Once);

            appointmentService
                .Verify(cs => cs.Save(nonExistingAppointmentId, appointment), Times.Never);

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void PatchAppointment_WithNullPatchAppointment_ShouldReturnBadRequest()
        {
            var patchDoc = new JsonPatchDocument();
            patchDoc = null;

            //Act
            var result = sut.PatchAppointment(patchDoc, existingAppointmentId);

            //Assert
            mockAppointmentRepository
                .Verify(cr => cr.Retrieve(existingAppointmentId), Times.Never);

            appointmentService
                .Verify(cs => cs.Save(existingAppointmentId, appointment), Times.Never);

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

    }
}
