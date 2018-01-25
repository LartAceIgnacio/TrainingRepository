using BlastAsia.DigiBook.Api.Controllers;
using BlastAsia.DigiBook.Domain.Contacts;
using BlastAsia.DigiBook.Domain.Models.Contacts;
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
    public class ContactsControllerTest
    {
        private Mock<IContactRepository> mockContactRepository;
        private Mock<IContactService> mockContactService;
        private ContactsController sut;
        private Contact contact;
        private Object result;
        private JsonPatchDocument patchedContact;
        private Guid existingContactId;
        private Guid nonExistingContactId;

        [TestInitialize]
        public void Initialize()
        {
            mockContactRepository = new Mock<IContactRepository>();
            mockContactService = new Mock<IContactService>();
            sut = new ContactsController(
                mockContactRepository.Object
                , mockContactService.Object);

            contact = new Contact();
            patchedContact = new JsonPatchDocument();

            existingContactId = Guid.NewGuid();
            nonExistingContactId = Guid.Empty;

            mockContactRepository
                .Setup(c => c.Retrieve(existingContactId))
                .Returns(contact);

            mockContactRepository
               .Setup(c => c.Retrieve(nonExistingContactId))
               .Returns<Contact>(null);
        }

        [TestMethod]
        public void GetContacts_WithEmptyContactId_ReturnsOkObjectResult()
        {
            //Arrange

            //Act

            result = sut.GetContacts(null);

            //Assert

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

            mockContactRepository
                .Verify(c => c.Retrieve(), Times.Once);

        }

        [TestMethod]
        public void GetContacts_WithExistingContactId_ReturnsOkObjectResutl()
        {
            //Arrange

            var existingContactId = Guid.NewGuid();

            //Act

            result = sut.GetContacts(existingContactId);

            //Assert

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

            mockContactRepository
                .Verify(c => c.Retrieve(existingContactId), Times.Once);

        }

        [TestMethod]
        public void CreateContact_WithValidContactData_ReturnsCreatedAtActionResult()
        {
            //Arrange

            contact = new Contact();

            //Act

            result = sut.CreateContact(contact);

            //Assert

            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));

            mockContactService
                .Verify(c => c.Save(Guid.Empty, contact), Times.Once);
        }
        [TestMethod]
        public void CreateContact_WithNullContactData_ReturnsBadRequestResult()
        {
            //Arrange

            contact = null;

            //Act

            result = sut.CreateContact(contact);

            //Assert

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));

            mockContactService
                .Verify(c => c.Save(Guid.Empty, contact), Times.Never);
        }

        [TestMethod]
        public void DeleteContact_WithExistingContactId_ReturnsOkResult()
        {
            //Arrange

            contact.ContactId = existingContactId;

            //Act

            result = sut.DeleteContact(contact.ContactId);

            //Assert

            Assert.IsInstanceOfType(result, typeof(OkResult));

            mockContactRepository
                .Verify(c => c.Delete(contact.ContactId), Times.Once);
        }

        [TestMethod]
        public void DeleteContact_WithNonExistingContactId_ReturnsNotFoundResult()
        {
            //Arrange

            contact.ContactId = nonExistingContactId;

            //Act

            result = sut.DeleteContact(nonExistingContactId);

            //Assert

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));

            mockContactRepository
                .Verify(c => c.Delete(contact.ContactId), Times.Never);
        }

        [TestMethod]
        public void UpdateContact_WithExistingContactId_ReturnsOkObjectResult()
        {
            //Arrange

            contact.ContactId = existingContactId;

            //Act

            result = sut.UpdateContact(contact, contact.ContactId);

            //Assert

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

            mockContactService
                .Verify(cs => cs.Save(contact.ContactId, contact), Times.Once);
            mockContactRepository
                .Verify(cr => cr.Retrieve(contact.ContactId), Times.Once);
            
        }

        [TestMethod]
        public void UpdateContact_WithNonExistingContactId_ReturnsBadRequestResult()
        {
            //Arrange

            contact.ContactId = nonExistingContactId;

            //Act

            result = sut.UpdateContact(contact, contact.ContactId);

            //Assert

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));

            mockContactRepository
                .Verify(cr => cr.Retrieve(contact.ContactId), Times.Once);
            mockContactService
                .Verify(cs => cs.Save(contact.ContactId, contact), Times.Never);
        }

        [TestMethod]
        public void PatchContact_WithValidPatchContactData_ReturnsOkObjectResult()
        {
            //Arrange

            contact.ContactId = existingContactId;

            //Act

            result = sut.PatchContact(patchedContact, contact.ContactId);

            //Assert

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

            mockContactService
                .Verify(cs => cs.Save(contact.ContactId, contact), Times.Once);

        }

        [TestMethod]
        public void PatchContact_WithNotValidPatchContactData_ReturnsBadRequestResult()
        {
            //Arrange

            patchedContact = null;

            //Act

            result = sut.PatchContact(patchedContact, contact.ContactId);

            //Assert

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));

            mockContactService
                .Verify(cs => cs.Save(contact.ContactId, contact), Times.Never);
        }

        [TestMethod]
        public void PatchContact_WithNonExistingContatctId_ReturnsNotFoundResult()
        {
            //Arrange

            contact.ContactId = nonExistingContactId;

            //Act

            result = sut.PatchContact(patchedContact, contact.ContactId);

            //Assert

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));

            mockContactService
                .Verify(cs => cs.Save(contact.ContactId, contact), Times.Never);
        }
    }
}