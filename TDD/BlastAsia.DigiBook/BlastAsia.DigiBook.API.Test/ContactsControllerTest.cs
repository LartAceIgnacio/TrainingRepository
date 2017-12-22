using BlastAsia.DigiBook.API.Controllers;
using BlastAsia.DigiBook.Domain.Contacts;
using BlastAsia.DigiBook.Domain.Models.Contacts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using BlastAsia.DigiBook.API.Utils;
using Microsoft.AspNetCore.JsonPatch;

namespace BlastAsia.DigiBook.API.Test
{
    [TestClass]
    public class ContactsControllerTest
    {
        private Mock<IContactService> mockContactService;
        private Mock<IContactRepository> mockContactRepository;
        ContactsController sut;
        private Guid newContactId;
        private Guid noContactId;
        private Contact contact;
        private JsonPatchDocument patchedContact;

        [TestInitialize]
        public void ContactInitialize()
        {
            contact = new Contact
            {

            };

            mockContactService = new Mock<IContactService>();
            mockContactRepository = new Mock<IContactRepository>();
            sut = new ContactsController(mockContactService.Object, mockContactRepository.Object);
            newContactId = Guid.NewGuid();
            noContactId = Guid.Empty;
            patchedContact = new JsonPatchDocument();

            mockContactRepository
                .Setup(cr => cr.Retrieve(newContactId))
                .Returns(contact);

            mockContactRepository
                .Setup(cr => cr.Retrieve(noContactId))
                .Returns<Contact>(null);

        }

        [TestMethod]
        public void GetContact_WithNoId_ShouldReturnOkObjectValue()
        {
            //Arrange
            mockContactRepository
                .Setup(cr => cr.Retrieve())
                .Returns(new List<Contact>());


            //Act
            var result = sut.GetContacts(null);

            //Assert
            Assert.IsInstanceOfType(result ,typeof(OkObjectResult));
        }

        [TestMethod]
        public void GetContact_WithExistingId_ShouldReturnOkObjectValue()
        {
            //Arrange

            mockContactRepository
                .Setup(cr => cr.Retrieve(newContactId))
                .Returns(contact);


            //Act
            var result = sut.GetContacts(newContactId);

            //Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void CreateContact_WithEmptyContact_ReturnBadRequestResult()
        {
            //Arrange
           Contact contact = null;

            //Act
            var result = sut.CreateContact(null);

            //Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));

            mockContactService
                .Verify(cr => cr.Save(Guid.Empty, contact),Times.Never());
        }

        [TestMethod]
        public void CreateContact_WithValidContact_ReturnCreatedAtActionResult()
        {
;

            //Act
            var result = sut.CreateContact(contact);

            //Assert
            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));

            mockContactService
                .Verify(cr => cr.Save(Guid.Empty, contact), Times.Once());
        }


        [TestMethod]
        public void DeleteContact_WithExistingId_ShouldReturnNoContentResult()
        {
            //Arrange
            mockContactRepository
                .Setup(cr => cr.Retrieve(newContactId))
                .Returns(contact);

            //Act
            var result = sut.DeleteContact(newContactId);


            //Assert
            mockContactRepository
                .Verify(cr => cr.Delete(newContactId), Times.Once());

            Assert.IsInstanceOfType(result,typeof(NoContentResult));


        }

        [TestMethod]
        public void DeleteContact_WithNotExistingId_ShouldReturnNotFoundResult()
        {
            //Arrange
            mockContactRepository
                .Setup(cr => cr.Retrieve(noContactId))
                .Returns<Contact>(null);

            //Act
            var result = sut.DeleteContact(noContactId);


            //Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));

            mockContactRepository
                .Verify(cr => cr.Delete(newContactId), Times.Never());

            


        }

        [TestMethod]
        public void UpdateContact_WithNoExistingContact_ShouldReturnBadRequestResult()
        {
            //Arrange
            contact = null;

            //Act
            var result = sut.UpdateContact(contact, newContactId);

            //Arrange
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));

            mockContactService
                .Verify(cr => cr.Save(newContactId, contact), Times.Never());
        }

        [TestMethod]
        public void UpdateContact_WithNoExistingId_ShouldReturnNotFoundResult()
        {
            //Arrange
            

            //Act
            var result = sut.UpdateContact(contact, noContactId);

            //Arrange
            Assert.IsInstanceOfType(result,typeof(NotFoundResult));

            mockContactService
                .Verify(cr => cr.Save(noContactId,contact),Times.Never());
        }

        [TestMethod]
        public void UpdateContact_WithExistingContactAndId_ShouldReturnOkObjectResult()
        {

            
            //Act
            var result = sut.UpdateContact(contact, newContactId);

            //Arrange
            mockContactService
                .Verify(cr => cr.Save(newContactId, contact), Times.Once());


            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

            
        }

        [TestMethod]
        public void PatchContact_WithNoExistingPatchContact_ShouldReturnBadRequestResult()
        {
            //Arrage
            patchedContact = null;
            //Act
            var result = sut.PatchContact(patchedContact, newContactId);
            //Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));

            mockContactService
                .Verify(cr => cr.Save(newContactId, contact), Times.Never());


        }

        [TestMethod]
        public void PatchContact_WithNoExistingId_ShouldReturnNotFoundResult()
        {
            //Arrage

            //Act
            var result = sut.PatchContact(patchedContact ,noContactId);
            //Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));

            mockContactService
                .Verify(cr => cr.Save(noContactId, contact), Times.Never());
               

        }

        [TestMethod]
        public void PatchContact_WithExistingPactchContactAndId_ShouldReturnOkObjectResult()
        {

            //Act
            var result = sut.PatchContact(patchedContact, newContactId);
            //Assert
            mockContactService
                .Verify(cr => cr.Save(newContactId, contact), Times.Once());

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

            


        }

    }
}
