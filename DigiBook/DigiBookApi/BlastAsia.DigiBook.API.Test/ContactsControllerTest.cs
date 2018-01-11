using BlastAsia.DigiBook.API.Controllers;
using BlastAsia.DigiBook.Domain.Contacts;
using BlastAsia.DigiBook.Domain.Models.Contacts;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;

namespace BlastAsia.DigiBook.API.Test
{
    [TestClass]
    public class ContactsControllerTest
    {
        private Contact contact;
        private Mock<IContactRepository> mockContactRepository;
        private Mock<IContactService> mockContactService;
        private ContactsController sut;
        private JsonPatchDocument patchedContact;

        [TestInitialize]
        public void Initialize()
        {
            mockContactRepository = new Mock<IContactRepository>();
            mockContactService = new Mock<IContactService>();
            sut = new ContactsController(mockContactRepository.Object, mockContactService.Object);
            contact = new Contact {
                ContactId = Guid.NewGuid(),
                FirstName = "Chris",
                LastName = "Manuel",
                MobilePhone = "09156879240",
                StreetAddress = "#3 Pananalig St., Brgy. Mabini JRizal",
                CityAddress = "Mandaluyong City",
                ZipCode = 1550,
                Country = "Philippines",
                EmailAddress = "cmanuel@blastasia.com",
                IsActive = false,
                DateActivated = new Nullable<DateTime>()
            };

            patchedContact = new JsonPatchDocument();

            mockContactRepository
                .Setup(x => x.Retrieve())
                .Returns(() => new List<Contact>
                {
                    new Contact()
                });

            mockContactRepository
                .Setup(x => x.Retrieve(contact.ContactId))
                .Returns(contact);
        }

        [TestCleanup]
        public void Cleanup()
        {

        }

        [TestMethod]
        public void GetContacts_WithEmptyContactId_ReturnsOkObjectResult()
        {
            //Arrange

            //Act
            var result = sut.GetContacts(null);

            //Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            mockContactRepository.Verify(x => x.Retrieve(), Times.Once);
        }

        [TestMethod]
        public void GetContacts_WithContactId_ReturnsOkObjectResult()
        {
            //Arrange

            //Act
            var result = sut.GetContacts(contact.ContactId);

            //Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            mockContactRepository.Verify(x => x.Retrieve(contact.ContactId), Times.Once);
        }

        [TestMethod]
        public void CreateContact_WithEmptyContact_ReturnsBadRequestResult()
        {
            //Arrange
            contact = null;

            //Act
            var result = sut.CreateContact(contact);

            //Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            mockContactService.Verify(x => x.Save(Guid.Empty, contact), Times.Never);
        }

        [TestMethod]
        public void CreateContact_WithContact_ReturnsCreatedAtActionResult()
        {
            //Arrange

            //Act
            var result = sut.CreateContact(contact);

            //Assert
            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
            mockContactService.Verify(x => x.Save(Guid.Empty, contact), Times.Once);
        }

        [TestMethod]
        public void CreateContact_WithFieldThatThrowsException_ReturnsBadRequestResult()
        {
            //Arrange
            contact.FirstName = "";
            
            mockContactService
                .Setup(x => x.Save(Guid.Empty, contact))
                .Throws(new Exception());

            //Act
            var result = sut.CreateContact(contact);

            //Assert
            mockContactService.Verify(x => x.Save(Guid.Empty, contact), Times.Once);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public void DeleteContact_WithNonExistingContactId_ReturnsNotFoundResult()
        {
            //Arrange
            contact.ContactId = Guid.Empty;

            //Act
            var result = sut.DeleteContact(contact.ContactId);

            //Assert
            mockContactRepository.Verify(x => x.Delete(contact.ContactId), Times.Never);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void DeleteContact_WithExistingContactId_ReturnsNoContentResult()
        {
            //Arrange


            //Act
            var result = sut.DeleteContact(contact.ContactId);

            //Assert
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
            mockContactRepository.Verify(x => x.Delete(contact.ContactId), Times.Once);

        }

        [TestMethod]
        public void UpdateContact_WithEmptyContact_ReturnsBadRequestResult()
        {
            //Arrange
            contact = null;

            //Act
            var result = sut.UpdateContact(contact, Guid.Empty);

            //Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            mockContactRepository.Verify(x => x.Retrieve(Guid.NewGuid()), Times.Never);
            mockContactService.Verify(x => x.Save(Guid.NewGuid(), contact), Times.Never);
        }

        [TestMethod]
        public void UpdateContact_WithContactButNonExistingContactId_ReturnsNotFoundResult()
        {
            //Arrange
            contact.ContactId = Guid.Empty;

            //Act
            var result = sut.UpdateContact(contact, contact.ContactId);

            //Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            mockContactRepository.Verify(x => x.Retrieve(contact.ContactId), Times.Once);
            mockContactService.Verify(x => x.Save(Guid.NewGuid(), contact), Times.Never);
        }

        [TestMethod]
        public void UpdateContact_WithContactAndExistingContactId_ReturnsOkObjectResult()
        {
            //Arrange


            //Act
            var result = sut.UpdateContact(contact, contact.ContactId);

            //Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            mockContactRepository.Verify(x => x.Retrieve(contact.ContactId), Times.Once);
            mockContactService.Verify(x => x.Save(contact.ContactId, contact), Times.Once);
        }

        [TestMethod]
        public void UpdateContact_WithFieldThatThrowsException_ReturnsBadRequestResult()
        {
            //Arrange
            contact.EmailAddress = "cmanuel.blastasia.com";
            
            mockContactService
                .Setup(x => x.Save(contact.ContactId, contact))
                .Throws(new Exception());


            //Act
            var result = sut.UpdateContact(contact, contact.ContactId);

            //Assert
            mockContactService.Verify(x => x.Save(contact.ContactId, contact), Times.Once);
            mockContactRepository.Verify(x => x.Retrieve(contact.ContactId), Times.Once);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }


        [TestMethod]
        public void PatchContact_WithEmptyPacthedContact_ReturnsBadRequestResult()
        {
            //Arrange
            patchedContact = null;

            //Act
            var result = sut.PatchContact(patchedContact, Guid.Empty);

            //Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            mockContactRepository.Verify(x => x.Retrieve(Guid.NewGuid()), Times.Never);
            mockContactService.Verify(x => x.Save(Guid.NewGuid(), contact), Times.Never);
        }

        [TestMethod]
        public void PatchContact_WithPatchedContactButNonExistingContactId_ReturnsNotFoundResult()
        {
            //Arrange

            //Act
            var result = sut.PatchContact(patchedContact, Guid.Empty);

            //Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            mockContactRepository.Verify(x => x.Retrieve(Guid.Empty), Times.Once);
            mockContactService.Verify(x => x.Save(Guid.NewGuid(), contact), Times.Never);
        }

        [TestMethod]
        public void PatchContact_WithPatchedContactAndExistingContactId_ReturnsOkObjectResult()
        {
            //Arrange


            //Act
            var result = sut.PatchContact(patchedContact, contact.ContactId);

            //Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            mockContactRepository.Verify(x => x.Retrieve(contact.ContactId), Times.Once);
            mockContactService.Verify(x => x.Save(contact.ContactId, contact), Times.Once);
        }

        [TestMethod]
        public void PatchContact_WithFieldThatThrowsException_ReturnsBadRequestResult()
        {
            //Arrange
            patchedContact.Replace("MobilePhone", "");

            mockContactService
                .Setup(x => x.Save(contact.ContactId, contact))
                .Throws(new Exception());


            //Act
            var result = sut.PatchContact(patchedContact, contact.ContactId);

            //Assert
            mockContactService.Verify(x => x.Save(contact.ContactId, contact), Times.Once);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }
    }
}
