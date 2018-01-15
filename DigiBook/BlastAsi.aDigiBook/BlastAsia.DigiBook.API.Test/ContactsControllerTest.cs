using BlastAsia.DigiBook.API.Controllers;
using BlastAsia.DigiBook.Domain.Contacts;
using BlastAsia.DigiBook.Domain.Models.Contacts;
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
    public class ContactsControllerTest
    {
        private Contact contact;
        private Mock<IContactService> mockContactService;
        private Mock<IContactRepository> mockContactRepository;
        private ContactsController sut;
        private readonly Guid existingContactId = Guid.NewGuid();
        private readonly Guid nonExistingContactId = Guid.Empty;
        private JsonPatchDocument patchedContact;

        [TestInitialize]
        public void Initialize()
        {
            contact = new Contact
            {
                ContactId = existingContactId,
                FirstName = "Jasmin",
                LastName = "Magdaleno",
                MobilePhone = "09057002880",
                StreetAddress = "L2 B15 Utex St., Litex Vill., San Jose",
                CityAddress = "Rodriguez, Rizal",
                ZipCode = 1860,
                Country = "Philippines",
                EmailAddress = "jasminmagdaleno@blastasia.com",
                IsActive = false,
                DateActivated = new Nullable<DateTime>(),
            };

            mockContactService = new Mock<IContactService>();
            mockContactRepository = new Mock<IContactRepository>();
            sut = new ContactsController(mockContactService.Object, mockContactRepository.Object);

            patchedContact = new JsonPatchDocument();
            patchedContact.Replace("Country", "London");

            mockContactRepository
                .Setup(c => c.Retrieve(existingContactId))
                .Returns(contact);

            mockContactService
                .Setup(c => c.Save(existingContactId, contact))
                .Returns(contact);
        }

        [TestCleanup]
        public void CleanUp()
        {

        }

        [TestMethod]
        public void GetContacts_WithEmptyContactId_ReturnsOkObjectResult()
        {
            //Arrange
            mockContactRepository
                .Setup(c => c.Retrieve())
                .Returns(() => new List<Contact>{
                       new Contact()
                       });
            //Act
            var result = sut.GetContacts(null);

            //Assert
            mockContactRepository.Verify(c => c.Retrieve(), Times.Once());

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void GetContacts_WithExistingContactId_ReturnsOkObjectResult()
        {
            //Arrange

            //Act
            var result = sut.GetContacts(contact.ContactId);

            //Assert
            mockContactRepository.Verify(c => c.Retrieve(contact.ContactId), Times.Once());

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void CreateContact_WithValidContactData_ReturnsCreatedAtActionResult()
        {
            //Arrange
            contact.ContactId = nonExistingContactId;

            mockContactService
                .Setup(c => c.Save(contact.ContactId, contact))
                .Returns(contact);

            //Act
            var result = sut.CreateContact(contact);

            //Assert
            mockContactService.Verify(c => c.Save(contact.ContactId,contact), Times.Once());
            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
        }

        [TestMethod]
        public void CreateContact_WithInvalidContactData_ReturnsBadRequestObjectResult()
        {
            //Arrange
            contact.EmailAddress = "jasminmagdalenoblastasiacom";
            contact.ContactId = nonExistingContactId;

            mockContactService
                .Setup(c => c.Save(contact.ContactId, contact))
                .Throws(new EmailAddressRequiredException(""));

            //Act
            var result = sut.CreateContact(contact);

            //Assert
            mockContactService.Verify(c => c.Save(contact.ContactId, contact), Times.Once());
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public void CreateContact_WithEmptyContactData_ReturnsBadRequestResult()
        {
            //Arrange
            contact = null;

            //Act
            var result = sut.CreateContact(contact);

            //Assert
            mockContactService.Verify(c => c.Save(Guid.NewGuid(), contact), Times.Never);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void DeleteContact_WithExistingContactId_ReturnsNoContentResult()
        {
            //Arrange

            //Act
            var result = sut.DeleteContact(contact.ContactId);

            //Assert
            mockContactRepository.Verify(c => c.Retrieve(contact.ContactId), Times.Once);
            mockContactRepository.Verify(c => c.Delete(contact.ContactId), Times.Once);
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }

        [TestMethod]
        public void DeleteContact_WithNonExistingContactId_ReturnsNotFoundResult()
        {
            //Arrange
            contact.ContactId = nonExistingContactId;

            //Act
            var result = sut.DeleteContact(contact.ContactId);

            //Assert
            mockContactRepository.Verify(c => c.Retrieve(contact.ContactId), Times.Once);
            mockContactRepository.Verify(c => c.Delete(contact.ContactId), Times.Never);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void UpdateContact_WithExistingContactIdAndData_ReturnsOkObjectResult()
        {
            //Arrange

            //Act
            var result = sut.UpdateContact(contact, contact.ContactId);

            //Assert
            mockContactRepository.Verify(c=>c.Retrieve(contact.ContactId), Times.Once);
            mockContactService.Verify(c=>c.Save(contact.ContactId,contact), Times.Once);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void UpdateContact_WithEmptyContactData_ReturnsBadRequestResult()
        {
            //Arrange
            contact = null;

            //Act
            var result = sut.UpdateContact(contact, nonExistingContactId);

            //Assert
            mockContactRepository.Verify(c => c.Retrieve(nonExistingContactId), Times.Never);
            mockContactService.Verify(c => c.Save(nonExistingContactId, contact), Times.Never);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void UpdateContact_WithInvalidContactData_ReturnsBadRequestObjectResult()
        {
            //Arrange
            contact.EmailAddress = "jasminmagdalenoblastasiacom";

            mockContactService
                .Setup(c => c.Save(contact.ContactId, contact))
                .Throws(new EmailAddressRequiredException(""));

            //Act
            var result = sut.UpdateContact(contact, contact.ContactId);

            //Assert
            mockContactRepository.Verify(c => c.Retrieve(contact.ContactId), Times.Once);
            mockContactService.Verify(c => c.Save(contact.ContactId, contact), Times.Once);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public void UpdateContact_WithNonExistingId_ReturnsNotFound()
        {
            //Arrange
            contact.ContactId = nonExistingContactId;

            //Act
            var result = sut.UpdateContact(contact, contact.ContactId);

            //Assert
            mockContactRepository.Verify(c => c.Retrieve(contact.ContactId), Times.Once);
            mockContactService.Verify(c => c.Save(contact.ContactId, contact), Times.Never);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void PatchContact_WithExistingIdAndValidData_ReturnOkObjectResult()
        {
            //Arrange

            //Act
            var result = sut.PatchContact(patchedContact, contact.ContactId);

            //Assert
            mockContactRepository.Verify(c => c.Retrieve(contact.ContactId), Times.Once);
            mockContactService.Verify(c => c.Save(contact.ContactId, contact), Times.Once);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void PatchContact_WithEmptyContactData_ReturnsBadRequestResult()
        {
            //Arrange
            patchedContact = null;

            //Act
            var result = sut.PatchContact(patchedContact, nonExistingContactId);

            //Assert
            mockContactRepository.Verify(c => c.Retrieve(nonExistingContactId), Times.Never);
            mockContactService.Verify(c => c.Save(nonExistingContactId, contact), Times.Never);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void PatchContact_WithInvalidContactData_ReturnsBadRequestObjectResult()
        {
            //Arrange
            patchedContact.Replace("EmailAddress", "jasminmagdalenoblastasiacom");

            mockContactService
                .Setup(c => c.Save(contact.ContactId, contact))
                .Throws(new EmailAddressRequiredException(""));

            //Act
            var result = sut.PatchContact(patchedContact, contact.ContactId);

            //Assert
            mockContactRepository.Verify(c => c.Retrieve(contact.ContactId), Times.Once);
            mockContactService.Verify(c => c.Save(contact.ContactId, contact), Times.Once);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public void PatchContact_WithNonExistingId_ReturnsNotFound()
        {
            //Arrange
            contact.ContactId = nonExistingContactId;

            //Act
            var result = sut.PatchContact(patchedContact, contact.ContactId);

            //Assert
            mockContactRepository.Verify(c => c.Retrieve(contact.ContactId), Times.Once);
            mockContactService.Verify(c => c.Save(contact.ContactId, contact), Times.Never);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }
    }
}