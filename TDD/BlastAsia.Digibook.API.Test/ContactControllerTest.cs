using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BlastAsia.Digibook.Domain.Contacts;
using Moq;
using BlastAsia.Digibook.API.Controllers;
using BlastAsia.Digibook.Domain.Models.Contacts;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using BlastAsia.Digibook.Domain.Employees;

namespace BlastAsia.Digibook.API.Test
{
    [TestClass]
    public class ContactControllerTest
    {
        private Mock<IContactRepository> mockContactRepository;
        private Mock<IContactService> mockContactService;
        private ContactsController sut;
        private Contact contact;
        private JsonPatchDocument jsonPatchDocument;
        Guid existingId = Guid.NewGuid();

        [TestInitialize]
        public void InitializeData()
        {
            mockContactRepository = new Mock<IContactRepository>();
            mockContactService = new Mock<IContactService>();
            contact = new Contact();
            jsonPatchDocument = new JsonPatchDocument();
            sut = new ContactsController(mockContactService.Object,mockContactRepository.Object);
        }

        [TestMethod]
        public void GetContact_WithEmptyContactId_ReturnsOkObjectResult()
        {
            mockContactRepository
                .Setup(
                    r => r.Retrieve()
                )
                .Returns(new List<Contact>());

            var result = sut.GetContact(null);

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void GetContact_WithContactId_ReturnsOkObjectResult()
        {
            mockContactRepository
                .Setup(
                    r => r.Retrieve(existingId)
                )
                .Returns(new Contact{ });

            var result = sut.GetContact(null);

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void CreateContact_WithValidContactData_ShouldReturnCreatedAtActionResult()
        {
            //Arrange
            contact.ContactId = Guid.Empty;

            mockContactService
                .Setup(c => c.Save(contact.ContactId, contact))
                .Returns(contact);

            //Act
            var result = sut.CreateContact(contact);

            //Assert
            mockContactService.Verify(c => c.Save(contact.ContactId, contact), Times.Once());
            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
        }

        [TestMethod]
        public void CreateContact_WithNullData_ShouldReturnBadRequestResult()
        {
            contact = null;

            var result = sut.CreateContact(contact);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            mockContactService
                .Verify(
                    r => r.Save(Guid.Empty, contact), Times.Never()
                );
        }

        [TestMethod]
        public void CreateContact_WithInvalidData_ShouldReturnBadRequestResult()
        {
            contact.FirstName = "";

            mockContactService
                .Setup(cs => cs.Save(Guid.Empty, contact))
                .Throws(new InvalidNameFormatException("First name cannot be blank."));

            var result = sut.CreateContact(contact);

            mockContactService
                .Verify(
                    r => r.Save(Guid.Empty, contact), Times.Once()
                );
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public void DeleteContact_WithValidId_ShouldReturnNoContentResult()
        {
            var result = sut.DeleteContact(existingId);
            Assert.IsInstanceOfType(result, typeof(NoContentResult));

            mockContactService
                .Verify(
                    cs => cs.Save(Guid.Empty,contact),Times.Never()
                );
        }

        [TestMethod]
        public void UpdateContact_WithValidContact_ShouldReturnOkObjectResult()
        {
            mockContactRepository
                .Setup(c => c.Retrieve(existingId))
                .Returns(contact);

            var result = sut.UpdateContact(contact, existingId);

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            mockContactService
                .Verify(
                    cs => cs.Save(existingId, contact), Times.Once()
                );
        }

        [TestMethod]
        public void UpdateContact_WithNullContact_ShouldReturnBadRequestResult()
        {
            contact = null;

            var result = sut.UpdateContact(contact, existingId);

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            mockContactService
                .Verify(
                    cs => cs.Save(existingId, contact), Times.Never()
                );
        }

        [TestMethod]
        public void UpdateContact_WithNonExistingContact_ShouldReturnNotFoundResult()
        {
            var result = sut.UpdateContact(contact, Guid.Empty);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            mockContactService
                .Verify(
                    cs => cs.Save(Guid.Empty, contact), Times.Never()
                );
        }

        [TestMethod]
        public void PatchContact_WithValidJsonPatch_ShouldReturnOkObjectResult()
        {
            mockContactRepository
                .Setup(
                    r => r.Retrieve(existingId)
                )
                .Returns(contact);

            var result = sut.PatchContact(jsonPatchDocument, existingId);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            mockContactService
                .Verify(
                    cs => cs.Save(existingId, contact), Times.Once()
                );
        }

        [TestMethod]
        public void PatchContact_WithNullJsonPatch_ShouldReturnBadRequestResult()
        {
            jsonPatchDocument = null;
            var result = sut.PatchContact(jsonPatchDocument, Guid.NewGuid());
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            mockContactService
                .Verify(
                    cs => cs.Save(existingId, contact), Times.Never()
                );
        }

        [TestMethod]
        public void PatchContact_WithNonExistingContact_ShouldReturnNotFoundResult()
        {
            var result = sut.PatchContact(jsonPatchDocument, Guid.Empty);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            mockContactService
                .Verify(
                    cs => cs.Save(existingId, contact), Times.Never()
                );
        }
    }
}
