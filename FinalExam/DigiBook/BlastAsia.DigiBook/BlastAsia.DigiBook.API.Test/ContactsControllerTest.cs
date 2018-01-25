using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BlastAsia.DigiBook.Domain.Contacts;
using Moq;
using BlastAsia.DigiBook.API.Controllers;
using BlastAsia.DigiBook.Domain.Models.Contacts;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;

namespace BlastAsia.DigiBook.API.Test
{
    [TestClass]
    public class ContactsControllerTest
    {
        Mock<IContactService> mockContactService;
        Mock<IContactRepository> mockContactRepository;
        ContactsController sut;
        Contact contact;
        JsonPatchDocument patchedContact;

        [TestInitialize]
        public void Initialize()
        {
            mockContactService = new Mock<IContactService>();
            mockContactRepository = new Mock<IContactRepository>();
            sut = new ContactsController(mockContactService.Object, mockContactRepository.Object);

            contact = new Contact {
                ContactId = Guid.NewGuid(),
                FirstName = "Angela Blanche",
                LastName = "Olarte",
                MobilePhone = "09981642039",
                StreetAddress = "#9 Kakawati Street, Pangarap Village",
                CityAddress = "Caloocan City",
                ZipCode = 1427,
                Country = "Philippines",
                EmailAddress = "abbieolarte@gmail.com",
                IsActive = false,
                DateActivated = new Nullable<DateTime>()
            };

            patchedContact = new JsonPatchDocument();
            patchedContact.Replace("FirstName", "Abbie");

            mockContactRepository
               .Setup(c => c.Retrieve(contact.ContactId))
               .Returns(contact);
        }

        [TestCleanup]
        public void Cleanup()
        {

        }

        [TestMethod]
        public void GetContacts_WithEmptyContactId_ReturnOkObjectResult()
        {
            // Arrange
            mockContactRepository
                .Setup(c => c.Retrieve())
                .Returns(new List<Contact>());

            // Act
            var result = sut.GetContacts(null);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            mockContactRepository.Verify(c => c.Retrieve(), Times.Once);
        }

        [TestMethod]
        public void GetContacts_WithValidContactId_ReturnOkObjectResult()
        {
            // Arrange
            mockContactRepository
                .Setup(c => c.Retrieve(Guid.NewGuid()))
                .Returns(contact);

            // Act
            var result = sut.GetContacts(contact.ContactId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            mockContactRepository.Verify(c => c.Retrieve(contact.ContactId), Times.Once);
        }

        [TestMethod]
        public void CreateContact_ContactWithValidData_ReturnCreatedAtActionResult()
        {
            // Act
            var result = sut.CreateContact(contact);

            // Assert
            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
            mockContactService.Verify(c => c.Save(Guid.Empty, contact), Times.Once);
        }

        [TestMethod]
        public void CreateContact_ContactWithNoData_ReturnBadRequestResult()
        {
            // Arrange
            contact = null;

            // Act
            var result = sut.CreateContact(contact);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            mockContactService.Verify(c => c.Save(Guid.Empty, contact), Times.Never);
        }

        [TestMethod]
        public void DeleteContact_WithContactId_ReturnNoContentResult()
        {
            // Act
            var result = sut.DeleteContact(contact.ContactId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
            mockContactRepository.Verify(c => c.Delete(contact.ContactId), Times.Once);
        }
        [TestMethod]
        public void DeleteContact_WithoutContactId_ReturnNoContentResult()
        {
            // Arrange
            mockContactRepository.
                Setup(c => c.Retrieve(contact.ContactId))
                .Returns<Contact>(null);

            // Act
            var result = sut.DeleteContact(contact.ContactId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            mockContactRepository.Verify(c => c.Delete(contact.ContactId), Times.Never);
        }

        [TestMethod]
        public void UpdateContact_WithExistingContactDataAndId_ReturnOkObjectResult()
        {
            // Act
            var result = sut.UpdateContact(contact, contact.ContactId);

            // Assert
            mockContactRepository.Verify(c => c.Retrieve(contact.ContactId), Times.Once);
            mockContactService.Verify(c => c.Save(contact.ContactId, contact), Times.Once);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void UpdateContact_ContactWithoutValue_ReturnNotFoundResult()
        {
            // Arrange
            contact = null;

            // Act
            var result = sut.UpdateContact(contact, Guid.Empty);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            mockContactRepository.Verify(c => c.Retrieve(Guid.Empty), Times.Never);
            mockContactService.Verify(c => c.Save(Guid.Empty, contact), Times.Never);
        }

        [TestMethod]
        public void UpdateContact_WithNoExistingContact_ReturnNotFoundResult()
        {
            // Arrange
            contact.ContactId = Guid.Empty;

            // Act
            var result = sut.UpdateContact(contact, contact.ContactId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            mockContactRepository.Verify(c => c.Retrieve(contact.ContactId), Times.Once);
            mockContactService.Verify(c => c.Save(contact.ContactId, contact), Times.Never);
        }

        [TestMethod]
        public void PatchContact_WithExistingContactDataAndId_ReturnOkObjectResult()
        {
            // Act
            var result = sut.PatchContact(patchedContact, contact.ContactId);

            // Assert
            mockContactService.Verify(c => c.Save(contact.ContactId, contact), Times.Once);
            mockContactRepository.Verify(c => c.Retrieve(contact.ContactId), Times.Once);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void PatchContact_WithoutExistingContactDataAndId_ReturnOkObjectResult()
        {
            // Arrange
            mockContactRepository
              .Setup(c => c.Retrieve(contact.ContactId))
              .Returns<Contact>(null);

            // Act
            var result = sut.PatchContact(patchedContact, contact.ContactId);

            // Assert
            mockContactService.Verify(c => c.Save(contact.ContactId, contact), Times.Never);
            mockContactService.Verify(c => c.Save(contact.ContactId, contact), Times.Never);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void PatchContact_ContactWithEmptyPatchDocument_ReturnBadRequestResult()
        {
            // Arrange
            patchedContact = null;

            // Act
            var result = sut.PatchContact(patchedContact, contact.ContactId);

            // Assert
            mockContactService.Verify(c => c.Save(contact.ContactId, contact), Times.Never);
            mockContactService.Verify(c => c.Save(contact.ContactId, contact), Times.Never);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }
    }
}
