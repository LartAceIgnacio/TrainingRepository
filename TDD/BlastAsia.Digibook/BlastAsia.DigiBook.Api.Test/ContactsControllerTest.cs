using BlastAsia.DigiBook.Api.Controllers;
using BlastAsia.DigiBook.Domain.Contacts;
using BlastAsia.DigiBook.Domain.Models.Contacts;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;

namespace BlastAsia.DigiBook.Api.Test
{
    [TestClass]
    public class ContactsControllerTest
    {
        private static Mock<IContactRepository> mockRepo;
        private static Mock<IContactService> mockService;

        private ContactsController sut;
        private Contact contact;
        private JsonPatchDocument patch;


        private readonly Guid existingId = Guid.NewGuid();
        private readonly Guid nonExistingId = Guid.Empty;


        [TestInitialize]
        public void Initialize()
        {

            contact = new Contact
            {
                ContactId = Guid.Empty,
                FirstName = "Emem",
                LastName = "Magadia",
                CityAddress = "Mayuro Rosario",
                Country = "Philippines",
                EmailAddress = "emmanuelmagadia@yahoo.com",
                MobilePhone = "091231231",
                StreetAddress = "245, Kanto",
                ZipCode = 1234
            };

            patch = new JsonPatchDocument();

            mockService = new Mock<IContactService>();
            mockRepo = new Mock<IContactRepository>();


            mockRepo
                .Setup(
                    r => r.Retrieve(existingId)
                )
                .Returns(contact);

            mockRepo
              .Setup(
                  r => r.Retrieve(nonExistingId)
              )
              .Returns<Contact>(null);

            mockService
               .Setup(
                   s => s.Save(existingId, contact)
               )
               .Returns(contact);


            sut = new ContactsController(mockRepo.Object, mockService.Object);
        }
        [TestCleanup]
        public void Cleanup()
        {

        }
        // Get
        [TestMethod]
        public void GetContact_WithNoId_ShouldReturnOkObjectValue()
        {
            // arrange

            mockRepo
                .Setup(
                    r => r.Retrieve()
                )
                .Returns(new List<Contact>());

            // act
            var result = sut.GetContact(null);

            // assert

            mockRepo
               .Verify(
                   r => r.Retrieve(), Times.Once()
               );

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

        }
        [TestMethod]
        public void GetContact_WithId_ShouldReturnOkObjectValue()
        {
            // arrange
            var id = Guid.NewGuid();
            mockRepo
                .Setup(
                    r => r.Retrieve(id)
                )
                .Returns(new Contact());

            // act
            var result = sut.GetContact(id);
            // assert
            mockRepo
               .Verify(
                   r => r.Retrieve(id), Times.Once()
               );

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        // Post
        [TestMethod]
        public void CreateContact_WithNullContact_ShouldReturnBadRequest()
        {
            // arrange
            contact = null;

            // act 
            var result = sut.CreateContact(contact);

            // assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));

            mockService
                .Verify(
                    r => r.Save(Guid.Empty, contact), Times.Never()
                );
        }

        [TestMethod]
        public void CreateContact_WithInvalidContact_ShouldReturnBadRequest()
        {
            // arrange
            contact.FirstName = "";

            mockService
                .Setup(
                    s => s.Save(contact.ContactId, contact)
                )
                .Returns(() => throw new Exception());

            // act
            var result = sut.CreateContact(contact);

            // assert
            mockService
            .Verify(
                r => r.Save(contact.ContactId, contact), Times.Once()
            );

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));

        }

        [TestMethod]
        public void CreateContact_WithvalidContact_ShouldReturnOkObjectRsult()
        {
            // arrange
            mockService
               .Setup(
                   s => s.Save(contact.ContactId, contact)
               )
               .Returns(new Contact());
            // act
            var result = sut.CreateContact(contact);
            // assert
            mockService
                .Verify(
                    s => s.Save(Guid.Empty, contact), Times.Once()
                );

            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
        }

        [TestMethod]
        public void DeleteContact_WithNonExistingContactId_SohouldReturnNotFound()
        {
            // arrange
            // act 
            var result = sut.DeleteContact(nonExistingId);

            // assert

            mockRepo
                .Verify(
                    r => r.Retrieve(nonExistingId), Times.Once()
                );

            mockRepo
              .Verify(
                  r => r.Delete(nonExistingId), Times.Never()
              );

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void DeleteContact_WithExistingContactId_SohouldReturnNoContent()
        {
            // arrange
            // act 
            var result = sut.DeleteContact(existingId);

            // assert

            mockRepo
                .Verify(
                    r => r.Retrieve(existingId), Times.Once()
                );

            mockRepo
             .Verify(
                 r => r.Delete(existingId), Times.Once()
             );

            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }


        [TestMethod]
        public void UpdateContact_WithNullContact_ShouldReturnBadRequest()
        {
            // arrange
            contact = null;
            var id = existingId;

            // act 
            var result = sut.UpdateContact(contact, id);

            // assert
            mockRepo
                .Verify(
                    r => r.Retrieve(id), Times.Never()
                );

            mockService
                .Verify(
                    r => r.Save(id, contact), Times.Never()
                );

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void UpdateContact_WithNonExistingContactid_ShouldReturnNotFoundResult()
        {
            // act
            var result = sut.UpdateContact(contact, nonExistingId);
            // assert 
            mockRepo
               .Verify(
                   r => r.Retrieve(nonExistingId), Times.Once()
               );

            mockService
                .Verify(
                    r => r.Save(nonExistingId, contact), Times.Never()
                );

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void UpdateContact_WithValidData_ShouldReturnOkResult()
        {
            // act
            var result = sut.UpdateContact(contact, existingId);


            // assert 

            mockRepo
               .Verify(
                   r => r.Retrieve(existingId), Times.Once()
            );

            mockService
                .Verify(
                    s => s.Save(existingId, contact), Times.Once()
                );

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void PatchContact_WithNullPatchContact_ShouldReturnBadRequest()
        {
            // arrange
            patch = null;

            var id = existingId;

            // act 
            var result = sut.PatchContact(patch, id);

            // assert
            mockRepo
                .Verify(
                    r => r.Retrieve(id), Times.Never()
                );

            mockService
                .Verify(
                    r => r.Save(id, contact), Times.Never()
                );

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }


        [TestMethod]
        public void PatchContact_WithNonExistingContactid_ShouldReturnNotFoundResult()
        {
            // act
            var result = sut.PatchContact(patch, nonExistingId);
            // assert 
            mockRepo
               .Verify(
                   r => r.Retrieve(nonExistingId), Times.Once()
               );

            mockService
                .Verify(
                    r => r.Save(nonExistingId, contact), Times.Never()
                );

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void PatchContact_WithValidData_ShouldReturnOkResult()
        {
            // act
            var result = sut.PatchContact(patch, existingId);
            // assert 

            mockRepo
               .Verify(
                   r => r.Retrieve(existingId), Times.Once()
            );

            mockService
                .Verify(
                    s => s.Save(existingId, contact), Times.Once()
                );

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

        }
    }
}


