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
        private Mock<IContactRepository> mockContactRepo;
        private Mock<IContactService> mockContactService;
        private ContactsController sut;
        private Contact contact;
        private Guid emptyContactId;
        private Guid existingContactId;
        private JsonPatchDocument patchedContact;


        [TestInitialize]
        public void InitializeTest()
        {
            mockContactRepo = new Mock<IContactRepository>();
            mockContactService = new Mock<IContactService>();
            sut = new ContactsController(mockContactService.Object, mockContactRepo.Object);
            contact = new Contact();
            existingContactId = Guid.NewGuid();
            emptyContactId = Guid.Empty;
            patchedContact = new JsonPatchDocument();

            mockContactRepo
                .Setup(cr => cr.Retrieve(existingContactId))
                .Returns(contact);
            mockContactRepo
                .Setup(cr => cr.Retrieve(emptyContactId))
                .Returns<Contact>(null);
        }

        [TestCleanup]
        public void CleanupTest()
        {

        }

        [TestMethod]
        public void GetContacts_WithEmptyContactId_ReturnsOkObjectValue()
        {
            // Arrange
            mockContactRepo
                .Setup(cr => cr.Retrieve())
                .Returns(new List<Contact>());

            // Act
            var result = sut.GetContacts(null);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void GetContacts_WithExistingContactId_ReturnsOkObjectValue()
        {
            // Act
            var result = sut.GetContacts(existingContactId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void CreateContact_WithEmptyContact_ReturnsBadRequest()
        {
            // Act
            var result = sut.CreateContact(null);

            // Assert
            mockContactService
                .Verify(cs => cs.Save(Guid.Empty, contact), Times.Never());
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            
        }

        [TestMethod]
        public void CreateContact_WithValidContact_ReturnsNewContactWithContactId()
        {
            // Arrange
            mockContactService
                .Setup(cs => cs.Save(Guid.Empty, contact))
                .Returns(contact);

            // Act
            var result = sut.CreateContact(contact);

            // Assert
            mockContactService
                .Verify(cs => cs.Save(Guid.Empty, contact), Times.Once);
            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
        }

        [TestMethod]
        public void DeleteContact_WithEmptyContactId_ReturnsNotFound()
        {
            // Act
            var result = sut.DeleteContact(emptyContactId);

            // Assert
            mockContactRepo
                .Verify(cr => cr.Delete(emptyContactId), Times.Never);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void DeleteContact_WithExistingContactId_ReturnsNoContent()
        {
            // Act
            var result = sut.DeleteContact(existingContactId);

            // Assert
            mockContactRepo
                .Verify(cr => cr.Delete(existingContactId), Times.Once);
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }

        [TestMethod]
        public void UpdateContact_WithEmptyContact_ReturnsBadRequest()
        {
            // Act
            var result = sut.UpdateContact(null, existingContactId);

            // Assert
            mockContactService
                .Verify(cs => cs.Save(existingContactId, null), Times.Never);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void UpdateContact_WithEmptyContactId_ReturnsNotFound()
        {
            // Act
            var result = sut.UpdateContact(contact, emptyContactId);

            // Assert
            mockContactService
                .Verify(cs => cs.Save(emptyContactId, contact), Times.Never);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void UpdateContact_WithExistingContactIdAndContact_ReturnsOkObjectValue()
        {
            // Act
            var result = sut.UpdateContact(contact, existingContactId);

            // Assert
            mockContactService
                .Verify(cs => cs.Save(existingContactId, contact), Times.Once);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void PatchContact_WithEmptyPatchedContact_ReturnsBadRequest()
        {
            // Arrange 
            patchedContact = null;

            // Act
            var result = sut.PatchContact(patchedContact, existingContactId);

            // Assert
            mockContactRepo
                .Verify(cr => cr.Retrieve(existingContactId), Times.Never);
            mockContactService
                .Verify(cs => cs.Save(existingContactId, contact), Times.Never);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));

        }

        [TestMethod]
        public void PatchContact_WithEmptyContactId_ReturnsNotFound()
        {
            // Act
            var result = sut.PatchContact(patchedContact, emptyContactId);

            // Assert
            mockContactService
                .Verify(cs => cs.Save(emptyContactId, contact), Times.Never);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void PatchContact_WithExistingContactId_ReturnsOkObjectValue()
        {
            // Act
            var result = sut.PatchContact(patchedContact, existingContactId);

            // Assert
            mockContactService
                .Verify(cs => cs.Save(existingContactId, contact), Times.Once);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }
    }
}
