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
        private Mock<IContactRepository> mockContactRepository;
        private Mock<IContactService> mockContactService;
        private JsonPatchDocument patchedContact;

        private ContactsController sut;

        private Guid existingContactId = Guid.NewGuid();
        private Guid notExistingContactId = Guid.Empty;

        [TestInitialize]
        public void Initialize()
        {
            
            mockContactRepository = new Mock<IContactRepository>();
            mockContactService = new Mock<IContactService>();
            patchedContact = new JsonPatchDocument();

            contact = new Contact
            {

                ContactId = Guid.NewGuid()
            };

            sut = new ContactsController(mockContactService.Object,
               mockContactRepository.Object);

            mockContactRepository
           .Setup(x => x.Retrieve())
           .Returns( () => new List<Contact>{
               new Contact()});

            mockContactRepository
            .Setup(x => x.Retrieve(contact.ContactId))
            .Returns(contact);

            mockContactRepository
                .Setup(cr => cr.Retrieve(existingContactId))
                .Returns(contact);

            //Setup for Update
            mockContactRepository
            .Setup(cr => cr.Retrieve(notExistingContactId))
            .Returns<Contact>(null);

            //Setup for Delete
            mockContactRepository
                .Setup(cr => cr.Retrieve(existingContactId))
                .Returns(contact);
        }
        [TestMethod]
        public void GetContacts_WithEmptyContactId_ReturnsOkObjectResult()
        {
            // Act
            var result = sut.GetContacts(null);

            // Assert      
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

            mockContactRepository
               .Verify(c => c.Retrieve(), Times.Once());
        }

        [TestMethod]
        public void GetContact_WithContactId_ReturnsObjectResult()
        {
            // Act
            var result = sut.GetContacts(notExistingContactId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

            mockContactRepository
               .Verify(c => c.Retrieve(notExistingContactId), Times.Once());
        }

        [TestMethod]
        public void CreateContact_WithEmptyContact_ReturnsBadRequestResult()
        {
            contact = null;
            var result = sut.CreateContact(contact);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));

            mockContactService
               .Verify(c => c.Save(Guid.Empty, contact), Times.Never());
        }

        [TestMethod]
        public void CreateContact_WithValidContact_ReturnsObjectResult()
        {
           
            mockContactService
              .Setup(cs => cs.Save(Guid.Empty, contact))
              .Returns(contact);

            //Act
            var result = sut.CreateContact(contact);

            // Assert         
            mockContactService
             .Verify(c => c.Save(Guid.Empty, contact), Times.Once());

            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
        }

        [TestMethod]
        public void UpdateContact_WithValidContact_ReturnsObjectResult()
        {
            
            var result = sut.UpdateContact(contact, existingContactId);
            // Assert
            
            mockContactRepository
                .Verify(c => c.Retrieve(existingContactId), Times.Once());

            mockContactService
                .Verify(c => c.Save(existingContactId, contact), Times.Once());

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void UpdateContact_WithEmptyContact_ReturnsBadRequestResult()
        {
            contact = null;
            // Act
            var result = sut.UpdateContact(contact,existingContactId);

            // Assert
            mockContactRepository
                .Verify(c => c.Update(existingContactId, contact), Times.Never());

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void UpdateContact_WithEmptyContactId_ReturnsNotFoundResult()
        {
            var result = sut.UpdateContact(contact, notExistingContactId);

            // Assert
            mockContactRepository
                .Verify(c => c.Update(notExistingContactId, contact), Times.Never());

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void DeleteContact_WithContactId_ReturnsNoContentResult()
        {   
            // Act
            var result = sut.DeleteContact(existingContactId);

            //Assert          
            mockContactRepository
                .Verify(c => c.Delete(existingContactId),Times.Once());

            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }

        [TestMethod]
        public void DeleteContact_WithEmptyContactId_ReturnsNotFound()
        {
            //Act
            var result = sut.DeleteContact(notExistingContactId);

            // Assert 
            mockContactRepository
                .Verify(c => c.Delete(notExistingContactId),
                Times.Never());
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void PatchContact_WithValidPatchedContact_ReturnsObjectResult()
        {
            var result = sut.PatchContact(patchedContact, existingContactId);
            // Assert

            mockContactRepository
                .Verify(c => c.Retrieve(existingContactId), Times.Once());

            mockContactService
                .Verify(c => c.Save(existingContactId, contact), Times.Once());

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

        }
        [TestMethod]
        public void PatchContact_WithEmptyPatchedContact_ReturnsBadRequestResult()
        {
            patchedContact = null;
            // Act
            var result = sut.PatchContact(patchedContact, existingContactId);

            // Assert
            mockContactRepository
               .Verify(c => c.Retrieve(notExistingContactId), Times.Never());

            mockContactService
                .Verify(c => c.Save(notExistingContactId, contact), Times.Never());

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void PatchContact_WithInvalidContactId_ReturnsNotFound()
        {
            var result = sut.PatchContact(patchedContact,notExistingContactId);

            //Assert
            mockContactRepository
                .Verify(c => c.Retrieve(notExistingContactId), Times.Once());

            mockContactService
                 .Verify(c => c.Save(notExistingContactId, contact), Times.Never());

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }
        
    }
}
