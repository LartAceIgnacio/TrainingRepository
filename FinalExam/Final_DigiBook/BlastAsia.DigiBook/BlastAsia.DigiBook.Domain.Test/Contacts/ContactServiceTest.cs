using BlastAsia.DigiBook.Domain.Contacts;
using BlastAsia.DigiBook.Domain.Models.Contacts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Test.Contacts
{
    [TestClass]
    public class ContactServiceTest
    {
        private Contact contact;
        private Mock<IContactRepository> mockContactRepository;
        private ContactService sut;
        private Guid existingContactId = Guid.NewGuid();
        private Guid nonExistingContactId = Guid.Empty;

        [TestInitialize]
        public void InitializeTest()
        {
            mockContactRepository = new Mock<IContactRepository>();
            sut = new ContactService(mockContactRepository.Object);
            contact = new Contact
            {
                FirstName = "Ryan",
                LastName = "Oribello",
                MobilePhone = "09264709989",
                StreetAddress = "43 Nueva Vizcaya St. Bago-Bantay Quezon City",
                CityAddress = "Quezon City",
                ZipCode = 1105,
                Country = "Philippines",
                EmailAddress = "oribelloryan@gmail.com",
                IsActive = false,
                DateActivated = new Nullable<DateTime>()
            };

         
        }
        
        [TestCleanup]
        public void CleanUp()
        {

        }

        [TestMethod]
        public void Save_NewContactWithValidData_ShouldCallRepositorySave()
        {
            // Arrange

            // Act
            sut.Save(contact.ContactId, contact);
            // Assert
            mockContactRepository
                .Verify(c => c.Retrieve(nonExistingContactId), Times.Once);

            mockContactRepository
                .Verify(c => c.Create(contact), Times.Once());
        }

        [TestMethod]
        public void Save_WithExistingContact_CallsRepositoryUpdate()
        {

            mockContactRepository
            .Setup(c => c.Retrieve(existingContactId))
                .Returns(contact);
            // if non existing
            mockContactRepository
                .Setup(c => c.Retrieve(nonExistingContactId))
                    .Returns<Contact>(null);

            contact.ContactId = existingContactId;

            sut.Save(contact.ContactId, contact);

            mockContactRepository
                .Verify(c => c.Retrieve(existingContactId), Times.Once);
            mockContactRepository
                .Verify(c => c.Update(existingContactId, contact), Times.Once);
        }

        [TestMethod]
        public void Create_WithValidData_ReturnNewContactWithContactId()
        {
            // Arrange
            mockContactRepository
                .Setup(c => c.Create(contact))
                .Callback(() => contact.ContactId = Guid.NewGuid())
                .Returns(contact);

            // Act
            var newContact = sut.Save(contact.ContactId, contact);

            // Asert
            //Assert.IsInstanceOfType(newContact.Co, typeof(Guid));
            Assert.IsTrue(newContact.ContactId != Guid.Empty);
        }

        [TestMethod]
        public void Create_WithBlankFirstName_ThrowsNameRequiredException()
        {
            contact.FirstName = "";

            //Act    
            Assert.ThrowsException<NameRequiredException>(
                () => sut.Save(contact.ContactId, contact));

            mockContactRepository
                 .Verify(c => c.Create(contact), Times.Never());
        }

        [TestMethod]
        public void Create_WithBlankLastname_ThrowsNameRequiredException()
        {
            contact.LastName = "";
            //Act    
            Assert.ThrowsException<NameRequiredException>(
                () => sut.Save(contact.ContactId, contact));

            mockContactRepository
                 .Verify(c => c.Create(contact), Times.Never());
        }

        [TestMethod]
        public void Create_WithBlankPhoneNumber_ThrowsPhoneNUmberRequiredException()
        {
            contact.MobilePhone = "";
            
            //Act    
            Assert.ThrowsException<MobilePhoneRequiredException>(
                () => sut.Save(contact.ContactId, contact));

            mockContactRepository
                 .Verify(c => c.Create(contact), Times.Never());
        }

        [TestMethod]
        public void Create_WithBlankStreetAddress_ThrowsAddressRequiredException()
        {
            contact.StreetAddress = "";
            //Act    
            Assert.ThrowsException<AddressRequiredException>(
                () => sut.Save(contact.ContactId, contact));

            mockContactRepository
                 .Verify(c => c.Create(contact), Times.Never());
        }

        [TestMethod]
        public void Create_WithBlankCityAddress_ThrowsAddressRequiredException()
        {
            contact.CityAddress = "";
            Assert.ThrowsException<AddressRequiredException>(
                () => sut.Save(contact.ContactId, contact));
            mockContactRepository
                .Verify(c => c.Create(contact), Times.Never());
        }

        [TestMethod]
        public void Create_WithNegativeNumberZipCode_ThrowsNonNegativeZipCodeException()
        {
            contact.ZipCode = -1;
            Assert.ThrowsException<NonNegativeZipCodeException>(
                () => sut.Save(contact.ContactId, contact));
            mockContactRepository
               .Verify(c => c.Create(contact), Times.Never());
        }

        [TestMethod]
        public void Create_WithBlankCountry_ThrowsAddressRequiredException()
        {
            contact.Country = "";
            Assert.ThrowsException<AddressRequiredException>(
                () => sut.Save(contact.ContactId, contact));
            mockContactRepository
                .Verify(c => c.Create(contact), Times.Never());
        }

        [TestMethod]
        public void Create_EmailNotRequired_ShouldCallRepositoryCreate()
        {
            contact.EmailAddress = "";
            sut.Save(contact.ContactId, contact);

            mockContactRepository
                .Verify(c => c.Create(contact), Times.Once());
        }

        [DataTestMethod]
        [DataRow("oribelloryanyahoo.com")]
        [DataRow("oribelloryan@yahoo")]
        [DataRow("oribelloryanyahoo.com")]
        [DataRow("oribello_ryanyahoo.com")]
        public void Create_EmailInvalidFormat_ThrowsEmailInvalidFormatException(string email)
        {
            contact.EmailAddress = email;

            mockContactRepository
                .Setup(c => c.Create(contact))
                .Callback(() => contact.ContactId = Guid.NewGuid())
                .Returns(contact);

            Assert.ThrowsException<EmailInvalidFormatException>(
                () => sut.Save(contact.ContactId, contact));
            mockContactRepository
                .Verify(c => c.Create(contact), Times.Never());
        }
   
    }
}