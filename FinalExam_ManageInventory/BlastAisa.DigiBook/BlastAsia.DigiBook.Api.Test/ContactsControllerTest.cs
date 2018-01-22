using BlastAsia.DigiBook.API.Controllers;
using BlastAsia.DigiBook.Domain.Contacts;
using BlastAsia.DigiBook.Domain.Models.Contacts;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;

namespace BlastAsia.DigiBook.Api.Test
{
    [TestClass]
    public class ContactsControllerTest
    {
        private Mock<IContactRepository> mockContactRepository;
        private Mock<IContactService> mockContactService;
        private ContactsController sut;
        private Contact contact;
        private JsonPatchDocument patch;

        [TestInitialize]
        public void TestInitialize()
        {
            mockContactRepository = new Mock<IContactRepository>();
            mockContactService = new Mock<IContactService>();
            sut = new ContactsController(mockContactService.Object,
                mockContactRepository.Object);

            patch = new JsonPatchDocument();

            contact = new Contact
            {
                ContactId = Guid.NewGuid(),
                FirstName = "John Karl",
                LastName = "Matencio",
                MobilePhone = "09957206817",
                StreetAddress = "7th St. Metrogate, Dasmariñas",
                CityAddress = "Cavite",
                ZipCode = 1604,
                Country = "Philippines",
                EmailAddress = "jhnkrl15@gmail.com",
                IsActive = false,
                DateActivated = new Nullable<DateTime>()
            };

            mockContactRepository
               .Setup(c => c.Retreive())
               .Returns(new List<Contact>());


            mockContactRepository
                .Setup(c => c.Retrieve(contact.ContactId))
                .Returns(contact);

            mockContactRepository
                .Setup(c => c.Create(contact))
                .Returns(contact);

            mockContactRepository
                .Setup(c => c.Retrieve(Guid.Empty))
                .Returns<Contact>(null);

        }

        [TestCleanup]
        public void TestCleanup()
        {

        }

        [TestMethod]
        public void GetContacts_WithEmptyContactId_ReturnsOkObjectValue()
        {
            //Arrange
            //Act
            var result = sut.GetContacts(null);
            //Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            mockContactRepository
                .Verify(c => c.Retreive(), Times.Once);
        }

        [TestMethod]
        public void GetContacts_WithExistingContactId_ReturnsOkObjectValue()
        {
            //Arrange
            //Act
            var result = sut.GetContacts(contact.ContactId);
            //Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            mockContactRepository
                .Verify(c => c.Retrieve(contact.ContactId), Times.Once);
        }

        [TestMethod]
        public void DeleteContact_WithEmptyContactId_ReturnsNotFound()
        {
            //Arrange
            contact.ContactId = Guid.Empty;
            //Act
            var result = sut.DeleteContact(contact.ContactId);
            //Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            mockContactRepository
                .Verify(c => c.Delete(contact.ContactId), Times.Never);
        }

        [TestMethod]
        public void DeleteContact_WithExistingContactId_ReturnsNoContent()
        {
            //Arrange
            //Act
            var result = sut.DeleteContact(contact.ContactId);
            //Assert
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
            mockContactRepository
                .Verify(c => c.Delete(contact.ContactId), Times.Once);
        }

        [TestMethod]
        public void CreateContact_WithEmptyContact_ReturnsBadRequest()
        {
            //Arrange          
            contact = null;
            //Act
            var result = sut.CreateContact(contact);
            //Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            mockContactService
                .Verify(c => c.Save(Guid.Empty, contact), Times.Never);
        }
        
        [TestMethod]
        public void CreateContact_WithExistingContact_ReturnsCreatedAtActionResult()
        {
            //Arrange
            //Act
            var result = sut.CreateContact(contact);
            //Assert
            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
            mockContactService
                .Verify(c => c.Save(Guid.Empty, contact), Times.Once);
        }

        [TestMethod]
        public void UpdateContact_WithEmptyContact_ReturnsBadRequest()
        {
            //Arrange
            contact = null;
            //Act
            var result = sut.UpdateContact(contact, Guid.Empty);
            //Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            mockContactService
                .Verify(c => c.Save(Guid.Empty, contact), Times.Never);
        }

        [TestMethod]
        public void UpdateContact_WithEmptyContactId_ReturnsNotFound()
        {
            //Arrange
            contact.ContactId = Guid.Empty;
            //Act
            var result = sut.UpdateContact(contact, contact.ContactId);
            //Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            mockContactRepository
                .Verify(c => c.Retrieve(contact.ContactId), Times.Once);           
            mockContactService
                .Verify(c => c.Save(Guid.NewGuid(), contact), Times.Never);
        }

        [TestMethod]
        public void UpdateContact_WithValidData_ReturnOkObjectResult()
        {
            //Arrange
            //Act
            var result = sut.UpdateContact(contact, contact.ContactId);
            //Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            mockContactRepository
                .Verify(c => c.Retrieve(contact.ContactId), Times.Once);
            mockContactService
                .Verify(c => c.Save(contact.ContactId, contact), Times.Once);
        }

        [TestMethod]
        public void PatchContact_WithEmptyPatchedContact_ReturnsBadRequest()
        {
            //Arrange
            patch = null;
            //Act
            var result = sut.PatchContact(patch, contact.ContactId);
            //Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            mockContactService
                .Verify(c => c.Save(contact.ContactId, contact), Times.Never);
        }

        [TestMethod]
        public void PatchContact_WithEmptyContactId_ReturnsNotFound()
        {
            //Arrange
            contact.ContactId = Guid.Empty;
            //Act
            var result = sut.PatchContact(patch, contact.ContactId);
            //Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            mockContactRepository
                .Verify(c => c.Retrieve(contact.ContactId), Times.Once);
            mockContactService
                .Verify(c => c.Save(contact.ContactId, contact), Times.Never);
        }

        [TestMethod]
        public void PatchContact_WithValidData_ReturnsOkObjectValue()
        {
            //Arrange
            //Act
            var result = sut.PatchContact(patch, contact.ContactId);
            //Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            mockContactRepository
                .Verify(c => c.Retrieve(contact.ContactId), Times.Once);
            mockContactService
                .Verify(c => c.Save(contact.ContactId, contact), Times.Once);
        }
    }
}
