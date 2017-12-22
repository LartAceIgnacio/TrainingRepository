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
        private Mock<IContactService> _mockContactService;
        private Mock<IContactRepository> _mockContactRepository;
        private ContactsController _sut;
        private Contact contact;

        [TestInitialize]
        public void Initialize()
        {
            _mockContactService = new Mock<IContactService>();
            _mockContactRepository = new Mock<IContactRepository>();
            _sut = new ContactsController(_mockContactService.Object, _mockContactRepository.Object);

            contact = new Contact() {
                ContactId = Guid.NewGuid(),
                Firstname = "Matt",
                Lastname = "Mendez",
                MobilePhone = "09293235700",
                StreetAddress = "318 Saint Michael St., Brgy. Holy Spirit",
                CityAddress = "Quezon City",
                ZipCode = 1127,
                EmailAddress = "mmendez@blastasia.com",
                IsActive = false,
                DateActivated = new Nullable<DateTime>()
            };

            _mockContactRepository.Setup(c => c.Retrieve(contact.ContactId))
                .Returns(contact);

            _mockContactRepository.Setup(x => x.Retrieve())
                .Returns(() => new List<Contact>
                {
                    new Contact()
                });


            //_mockContactRepository.Setup(c => c.Create(null))
            //    .Returns<Contact>(null);

        }

        [TestCleanup]
        public void Cleanup()
        {

        }

        [TestMethod]
        [TestProperty("API Test", "ContactsController")]
        public void GetContacts_NoRetrieveParameterGetAllContacts_ReturnsOkResult()
        {

            // Act
            var result = _sut.GetContacts(null);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            _mockContactRepository.Verify(c => c.Retrieve(), Times.Once);
        }

        [TestMethod]
        [TestProperty("API Test", "ContactsController")]
        public void GetContacts_WithValidId_ReturnsOkResult()
        {

            var result = _sut.GetContacts(contact.ContactId);

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            _mockContactRepository.Verify(c => c.Retrieve(contact.ContactId), Times.Once);
        }

        [TestMethod]
        [TestProperty("API Test", "ContactsController")]
        public void PostContact_WithNoContactInformation_ReturnsBadRequest()
        {
            // Arrange

            // act
            var result = _sut.PostContact(null);

            //assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            _mockContactService.Verify(service => service.Save(Guid.Empty, contact), Times.Never);
        }

        [TestMethod]
        [TestProperty("API Test", "ContactsController")]
        public void PostContact_WithValidInformation_ReturnsCreateActionResult()
        {
            // Act
            var result = _sut.PostContact(contact);

            // Assert
            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
            _mockContactService.Verify(service => service.Save(Guid.Empty, contact));//Empty Guid coz' we're creating new data.

        }

        [TestMethod]
        [TestProperty("API Test", "ContactsController")]
        public void DeleteContact_WithValidId_ReturnsNoContentResult()
        {
            // Act

            var result = _sut.DeleteContact(contact.ContactId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
            _mockContactRepository.Verify(repo => repo.Delete(contact.ContactId), Times.Once);
        }

        [TestMethod]
        [TestProperty("API Test", "ContactsController")]
        public void DeleteContact_WithInvalidId_ReturnsNotFoundResult()
        {
            // Act

            var result = _sut.DeleteContact(Guid.Empty);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            _mockContactRepository.Verify(repo => repo.Delete(Guid.Empty), Times.Never);
        }

        [TestMethod]
        [TestProperty("API Test", "ContactsController")]
        public void UpdateContact_WithValidData_ReturnsOkResult()
        {
            // Act
            var result = _sut.UpdateContact(contact,contact.ContactId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkResult));
            _mockContactService.Verify(service => service.Save(contact.ContactId, contact), Times.Once);

        }

        [TestMethod]
        [TestProperty("API Test", "ContactsController")]
        public void UpdateContact_WithInvalid_ReturnsBadRequest()
        {
            // Act
            contact.Firstname = null;

            _mockContactService.Setup(service => service.Save(contact.ContactId, contact))
                .Throws<Exception>();

            // Act
            var result = _sut.UpdateContact(contact, contact.ContactId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            _mockContactRepository.Verify(repo => repo.Retrieve(contact.ContactId), Times.Once);
            _mockContactService.Verify(service => service.Save(contact.ContactId, contact), Times.Once);
        }

        [TestMethod]
        [TestProperty("API Test", "ContactsController")]
        public void Patch_WithNullPatchContact_ReturnsBadRequest()
        {
            // Arrange

            // Act
            var result = _sut.PatchContact(null, contact.ContactId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            _mockContactRepository.Verify(repo => repo.Retrieve(contact.ContactId), Times.Never);
            _mockContactService.Verify(service => service.Save(contact.ContactId, contact), Times.Never);
        }

        [TestMethod]
        [TestProperty("API Test", "ContactsController")]
        public void Patch_WithoutRetrievedContact_ReturnsNotFounnd()
        {
            // Arrange
            var patchedDoc = new JsonPatchDocument();
            var guid = Guid.Empty;

            _mockContactService.Setup(service => service.Save(guid, contact))
                .Returns<Contact>(null);

            // Act
            var result = _sut.PatchContact(patchedDoc, guid);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            _mockContactRepository.Verify(repo => repo.Retrieve(guid), Times.Once);
            _mockContactService.Verify(service => service.Save(guid, contact), Times.Never);
        }
    }
}
