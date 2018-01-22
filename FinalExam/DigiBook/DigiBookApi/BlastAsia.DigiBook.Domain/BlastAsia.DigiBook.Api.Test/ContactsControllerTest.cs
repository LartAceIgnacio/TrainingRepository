using BlastAsia.DigiBook.Api.Controllers;
using BlastAsia.DigiBook.Domain.Contacts;
using BlastAsia.DigiBook.Domain.Contacts.Interfaces;
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
        private readonly Guid existingContactId = Guid.NewGuid();
        private readonly Guid nonExistingContactId = Guid.Empty;
        private Mock<IContactRepository> mockContactRepository;
        private Mock<IContactService> contactService;
        private ContactsController sut;

        private Contact contact = new Contact();
        private List<Contact> contactList = new List<Contact>();

        [TestInitialize]
        public void InitializeTest()
        {
            mockContactRepository = new Mock<IContactRepository>();
            contactService = new Mock<IContactService>();

            sut = new ContactsController(contactService.Object, mockContactRepository.Object);

            //GetContacts with id
            mockContactRepository
              .Setup(cr => cr.Retrieve(existingContactId))
              .Returns(contact);

            //GetContacts without id
            mockContactRepository
                .Setup(cr => cr.Retrieve())
                .Returns(contactList);

            //Update with existingId
            mockContactRepository
                .Setup(cr => cr.Retrieve(existingContactId))
                .Returns(contact);

            //Update without existingId
            mockContactRepository
                .Setup(cr => cr.Retrieve(nonExistingContactId))
                .Returns<Contact>(null);

            //CreatContact with null Contact
            mockContactRepository
                .Setup(cr => cr.Create(null))
                .Returns<Contact>(null);


            //CreatContact with valid Contact
            mockContactRepository
                .Setup(cr => cr.Create(contact))
                .Returns(contact);


        }
        [TestMethod]
        public void GetContacts_WithValidId_ShouldReturnOkObjectValue()
        {
            //Act
            var result = sut.GetContacts(existingContactId);

            //Assert
            mockContactRepository
                .Verify(cr => cr.Retrieve(existingContactId), Times.Once);

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void GetContacts_WithoutId_ShouldReturnOkObjectValue()
        {
            //Act
            var result = sut.GetContacts(null);
            
            //Assert
            mockContactRepository
                .Verify(cr => cr.Retrieve(), Times.Once);
           
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void UpdateContact_WithValidId_ShouldReturnOkObjectValue()
        {
           

            //Act
            var result = sut.UpdateContact(contact,existingContactId);


            //Assert 
            mockContactRepository
                .Verify(cr => cr.Retrieve(existingContactId), Times.Once);

            contactService
               .Verify(cr => cr.Save(existingContactId,contact), Times.Once);

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }
        [TestMethod]
        public void UpdateContact_WithNullContact_ShouldReturnBadRequest()
        {
            //Arrange
            contact = null;
            //act
            var result = sut.UpdateContact(contact, nonExistingContactId);

            //Assert
            mockContactRepository
                .Verify(cr => cr.Retrieve(nonExistingContactId), Times.Never);

            contactService
                .Verify(cs => cs.Save(nonExistingContactId, contact), Times.Never);

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }
        [TestMethod]
        public void UpdateContact_WithInvalidId_ShouldReturnBadRequest()
        {
            //Act
            var result = sut.UpdateContact(contact, nonExistingContactId);

            //Assert
            mockContactRepository
                .Verify(cr => cr.Retrieve(nonExistingContactId), Times.Once);

            contactService
                .Verify(cs => cs.Save(nonExistingContactId, contact), Times.Never);

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void CreateContact_WithNullContact_ShouldReturnBadRequest()
        {
            //Arrange
            contact = null;

            //Act
            var result = sut.CreateContact(contact);


            //Assert
            contactService
                .Verify(cs => cs.Save(Guid.Empty, contact), Times.Never);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void CreateContact_WithValidContact_ShouldReturnCreatedAtActionResult()
        {
            //Act
            var result = sut.CreateContact(contact);


            //Assert  
            contactService
                .Verify(cs => cs.Save(Guid.Empty, contact), Times.Once);

            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
        }

        [TestMethod]
        public void DeleteContact_WithValidContactId_ShouldReturnNoContentResult()
        {
            //Act
            var result = sut.DeleteContact(existingContactId);


            //Assert
            mockContactRepository
                .Verify(cr => cr.Retrieve(existingContactId), Times.Once);

            mockContactRepository
                .Verify(cr => cr.Delete(existingContactId), Times.Once);
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }

        [TestMethod]
        public void DeleteContact_WithInvalidContactId_ShouldReturnBadRequest()
        {
            //Act
            var result = sut.DeleteContact(nonExistingContactId);

            //Assert
            mockContactRepository
                .Verify(cr => cr.Retrieve(nonExistingContactId), Times.Once);

            mockContactRepository
                .Verify(cr => cr.Delete(nonExistingContactId), Times.Never);

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));

        }

        [TestMethod]
        public void PatchContact_WithValidContactId_ShouldReturnOkObjectValue()
        {
            //Arrange
            var patchDoc = new JsonPatchDocument();

            //Act
            var result = sut.PatchContact(patchDoc, existingContactId);

            //Assert
            mockContactRepository
                .Verify(cr => cr.Retrieve(existingContactId), Times.Once);

            contactService
                .Verify(cs => cs.Save(existingContactId, contact), Times.Once);

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

        }

        [TestMethod]
        public void PatchContact_WithInvalidContactId_ShouldReturnBadRequest()
        {
            var patchDoc = new JsonPatchDocument();

            //Act
            var result = sut.PatchContact(patchDoc, nonExistingContactId);

            //Assert
            mockContactRepository
                .Verify(cr => cr.Retrieve(nonExistingContactId), Times.Once);

            contactService
                .Verify(cs => cs.Save(nonExistingContactId, contact), Times.Never);

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void PatchContact_WithNullPatchContact_ShouldReturnBadRequest()
        {
            var patchDoc = new JsonPatchDocument();
            patchDoc = null;

            //Act
            var result = sut.PatchContact(patchDoc, existingContactId);

            //Assert
            mockContactRepository
                .Verify(cr => cr.Retrieve(existingContactId), Times.Never);

            contactService
                .Verify(cs => cs.Save(existingContactId, contact), Times.Never);

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }


    }
}
