
using BlastAsia.DigiBook.Domain.Contacts;
using BlastAsia.DigiBook.Domain.Models.Contacts;
using BlastAsia.DigiBook.Domain.Contacts.ContactExceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Test.ContactsTest
{
    [TestClass]
    
    public class ContactServiceTest
    {
        private Mock<IContactRepository> mockContactRepository;
        private Contact contact;
        private ContactService sut;
        private Guid existingContactId; //= Guid.NewGuid();
        private Guid nonExistingContactId;// = Guid.Empty;

        [TestInitialize]
        public void Initialize()
        {
            existingContactId = Guid.NewGuid();
            nonExistingContactId = Guid.Empty;

            mockContactRepository = new Mock<IContactRepository>();
            sut = new ContactService(mockContactRepository.Object);
            contact = new Contact
            {

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

            mockContactRepository.Setup(c => c.Retrieve(existingContactId))
                .Returns(contact);

            mockContactRepository.Setup(c => c.Retrieve(nonExistingContactId))
                .Returns<Contact>(null);

            //_mockContactRepository.Setup(c => c.Retrieve(It.IsAny<Guid>()));

        }

        [TestCleanup]
        public void Cleanup(){}

        [TestMethod]
        public void Save_NewContactWithValidData_ShouldCallRepositoryCreate()
        {

            // Act
            var result = sut.Save(contact.ContactId, contact);

            //Assert
            mockContactRepository.Verify(c => c.Retrieve(nonExistingContactId), Times.Once);
            mockContactRepository.Verify(o => o.Create(contact), Times.Once);
        }

        [TestMethod]
        public void Save_WithExistingContact_CallsRepositoryUpdate()
        {
            // Arrange
            contact.ContactId = existingContactId;

            // Act
            sut.Save(existingContactId, contact);


            // Assert
            mockContactRepository.Verify(c => c.Retrieve(contact.ContactId), Times.Once);
            mockContactRepository.Verify(c => c.Update(existingContactId, contact), Times.Once);

        }



        [TestMethod]
        public void Save_WithValidData_ReturnsNewContactWithContactId()
        {
            // Arrange
            mockContactRepository.Setup(c => c.Create(contact))
                .Callback(() => contact.ContactId = Guid.NewGuid())
                .Returns(contact);

            // Act
            var newContact = sut.Save(contact.ContactId, contact);

            // Assert
            Assert.IsTrue(newContact.ContactId != Guid.Empty);
        }

        [TestMethod]
        public void Save_WithBlankFirstname_ThrowsNameRequiredException()
        {
            // Arrange
            contact.Firstname = string.Empty;

            // Act
            // Assert
            Assert.ThrowsException<NameRequiredException>(() => sut.Save(contact.ContactId, contact));

            mockContactRepository.Verify(c => c.Create(contact), Times.Never);
        }

        [TestMethod]
        public void Save_WithBlankLastname_ThrowsNameRequiredException()
        {
            // Arrange
            contact.Lastname = string.Empty;

            // Act
            // Assert

            Assert.ThrowsException<NameRequiredException>(
                    () => sut.Save(contact.ContactId,contact));
            mockContactRepository.Verify(c => c.Create(contact), Times.Never);
        }

        [TestMethod]
        public void Save_WithBlankAddress_ThrowsAddressRequiredException()
        {
            // Arrange
            contact.StreetAddress = string.Empty;

            // Act
            // Assert

            Assert.ThrowsException<AddressRequiredException>(
                () => sut.Save(contact.ContactId, contact));

            mockContactRepository.Verify(c => c.Create(contact), Times.Never);
        }

        [TestMethod]
        public void Save_WithBlankMobileNumber_ThrowsContactNumberRequiredException()
        {
            // Arrange
            contact.MobilePhone = string.Empty;

            // Act
            // Assert

            Assert.ThrowsException<ContactNumberRequiredException>(
                () => sut.Save(contact.ContactId, contact));

            mockContactRepository.Verify(c => c.Create(contact), Times.Never);
        }

        [TestMethod]
        public void Save_MininumNumberOfContactNumber_ThrowsContatctNumberException()
        {
            // Arrange
            contact.MobilePhone = "0929332";

            // Act
            // Assert

            Assert.ThrowsException<ContactNumberMinimumLength>(
                () => sut.Save(contact.ContactId, contact));

            mockContactRepository.Verify(c => c.Create(contact), Times.Never);
        }

        [TestMethod]
        public void Save_WithBlankCityAddress_ThrowsCityAddressRequiredException()
        {
            // Arrange
            contact.CityAddress = string.Empty;

            // Act
            // Assert
            Assert.ThrowsException<CityAddressRequiredException>(
                () => sut.Save(contact.ContactId, contact));

            mockContactRepository.Verify(c => c.Create(contact), Times.Never);
        }

        [TestMethod]
        public void Save_WithNegativeZip_ThrowInvalidZipCodeException()
        {
            // Arrange
            contact.ZipCode = -1;

            // Act
            // Assert
            Assert.ThrowsException<InvalidZipCodeException>(
                ()=> sut.Save(contact.ContactId, (contact)));

            mockContactRepository.Verify(c => c.Create(contact), Times.Never);

        }

        [TestMethod]
        public void Save_WithBlankZip_ThrowInvalidZipCodeException()
        {
            // Arrange
            contact.ZipCode = null;

            // Act
            // Assert
            Assert.ThrowsException<InvalidZipCodeException>(
                () => sut.Save(contact.ContactId, contact));

            mockContactRepository.Verify(c => c.Create(contact), Times.Never);

        }

        [DataTestMethod]
        [DataRow("mmendezblastasia.com")]
        [DataRow("mmendezblastasia.com")]
        [DataRow("mmendez.blastasia.com")]        
        public void Save_InvalidEmailFormat_ThrowsInvalidEmailFormatException(string email)
        {
            contact.EmailAddress = email;
            // ACT
            // ASSERT
            Assert.ThrowsException<InvalidEmailFormatException>(
                    () => sut.Save(contact.ContactId, contact));

            mockContactRepository.Verify(c => c.Create(contact), Times.Never);
        }
    }
}
