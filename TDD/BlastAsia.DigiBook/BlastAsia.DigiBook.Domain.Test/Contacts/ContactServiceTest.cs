using BlastAsia.DigiBook.Domain.Contacts;
using BlastAsia.DigiBook.Domain.Models.Contacts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Test.NewFolder
{
    [TestClass]
    public class ContactServiceTest
    {
        private Mock<IContactRepository> mockContactRepository;
        private ContactService sut;
        private Contact contact;

        [TestInitialize]
        public void InitializeTest()
        {
            contact = new Contact
            {
                FirstName = "Duane", // required
                LastName = "De Guzman", // required
                MobilePhone = "09158959384", // required 
                StreetAddress = "264 Quezon st Cuyab", // required
                CityAddress = "San Pedro, Laguna", // required
                ZipCode = 4023, // required
                Country = "Philippines", // required
                EmailAddress = "dlfdeguzman@outlook.com",
                IsActive = false,
                DateActivated = new Nullable<DateTime>()

            };
            mockContactRepository = new Mock<IContactRepository>();
            sut = new ContactService(mockContactRepository.Object);    
        }

        [TestCleanup]
        public void CleanupTest()
        {

        }

        [TestMethod]
        public void Create_WithValidData_ShouldCallRepositoryCreate()
        {
            //Act
            var result = sut.Create(contact);
            //Assert
            mockContactRepository
                .Verify(c => c.Create(contact), Times.Once());
        }

        [TestMethod]
        public void Create_WithValidData_ReturnsNewContactWithContactId()
        {
            mockContactRepository
                .Setup(c => c.Create(contact))
                .Callback(() => contact.ContactId = Guid.NewGuid())
                .Returns(contact);
            // Act
            var newContact = sut.Create(contact);
            // Assert
            Assert.IsTrue(newContact.ContactId != Guid.Empty);
        }

        [TestMethod]
        public void Create_WithBlankFirstName_ThrowsNameRequiredException()
        {
            // Arrange
            contact.FirstName = "";
            // Assert
            mockContactRepository
                .Verify(c => c.Create(contact), Times.Never());
            Assert.ThrowsException<NameRequiredException>(
                () => sut.Create(contact));
        }

        [TestMethod]
        public void Create_WithBlankLastName_ThrowsNameRequiredException()
        {
            // Arrange
            contact.LastName = "";
            // Assert
            mockContactRepository
                .Verify(c => c.Create(contact), Times.Never());
            Assert.ThrowsException<NameRequiredException>(
                () => sut.Create(contact));
        }

        [TestMethod]
        public void Create_WithBlankMobilePhone_ThrowsMobilePhoneRequiredException()
        {
            // Arrange
            contact.MobilePhone = "";
            // Assert
            mockContactRepository
                .Verify(c => c.Create(contact), Times.Never());
            Assert.ThrowsException<MobilePhoneRequiredException>(
                () => sut.Create(contact));
        }

        [TestMethod]
        public void Create_WithBlankStreetAddress_ThrowsAddressRequiredException()
        {
            // Arrange
            contact.StreetAddress = "";
            // Assert
            mockContactRepository
                .Verify(c => c.Create(contact), Times.Never());
            Assert.ThrowsException<AddressRequiredException>(
                () => sut.Create(contact));
        }

        [TestMethod]
        public void Create_WithBlankCityAddress_ThrowsAddressRequiredException()
        {
            // Arrange
            contact.CityAddress = "";
            // Assert
            mockContactRepository
                .Verify(c => c.Create(contact), Times.Never());
            Assert.ThrowsException<AddressRequiredException>(
                () => sut.Create(contact));
        }

        [TestMethod]
        public void Create_WithNegativeZipCode_ThrowsAddressRequiredException()
        {
            // Arrange
            contact.ZipCode = -4023;
            // Assert
            mockContactRepository
                .Verify(c => c.Create(contact), Times.Never());
            Assert.ThrowsException<AddressRequiredException>(
                () => sut.Create(contact));
        }

        [TestMethod]
        public void Create_WithBlankCountry_ThrowsAddressRequiredException()
        {
            // Arrange
            contact.Country = "";
            // Assert
            mockContactRepository
                .Verify(c => c.Create(contact), Times.Never());
            Assert.ThrowsException<AddressRequiredException>(
                () => sut.Create(contact));
        }
    }
}
