using BlastAsia.DigiBook.Domain.Contacts;
using BlastAsia.DigiBook.Domain.Models.Contacts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
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
        public void Initialize()
        {
            contact = new Contact
            {
                FirstName = "Angela Blanche",
                LastName = "Olarte",
                MobilePhone = "09981642039",
                StreetAddress = "#9 Kakawati Street, Pangarap Village",
                CityAddress = "Caloocan City",
                ZipCode = 1427,
                Country = "Philippines",
                EmailAddress = "abbieolarte@gmail.com",
                IsActive = false,
                DateActivated = new Nullable<DateTime>()
            };

            mockContactRepository = new Mock<IContactRepository>();

            mockContactRepository
                .Setup(c => c.Retrieve(existingContactId))
                .Returns(contact);

            mockContactRepository
                .Setup(c => c.Retrieve(nonExistingContactId))
                .Returns<Contact>(null);

            sut = new ContactService(mockContactRepository.Object);
        }

        [TestCleanup]
        public void CleanUp()
        {

        }

        [TestMethod]
        public void Save_NewContactWithValidData_ShouldCallRepositoryCreate()
        {
            // Act
            var result = sut.Save(contact.ContactId,contact);

            // Assert
            mockContactRepository
                .Verify(c => c.Retrieve(nonExistingContactId), Times.Once());
            
            mockContactRepository
                .Verify(c => c.Create(contact), Times.Once());
        }

        [TestMethod]
        public void Save_WithExistingContact_CallsRepositoryUpdate()
        {
            // Arrange
            contact.ContactId = existingContactId;

            // Act
            sut.Save(contact.ContactId,contact);

            // Assert
            mockContactRepository
                .Verify(c => c.Retrieve(existingContactId), Times.Once);
            mockContactRepository
                .Verify(c => c.Update(existingContactId, contact), Times.Once);
        }

        [TestMethod]
        public void Save_WithValidData_ReturnsNewContactWithContactId()
        {
            mockContactRepository
                .Setup(c => c.Create(contact))
                .Callback(() => contact.ContactId = Guid.NewGuid())
                .Returns(contact);

            // Act
            var newContact = sut.Save(contact.ContactId,contact);

            // Assert
            Assert.IsTrue(newContact.ContactId != Guid.Empty);
        }

        [TestMethod]
        public void Save_WithBlankFirstName_ThrowsNameRequiredException()
        {
            // Arrange
            contact.FirstName = "";

            // Assert
            Assert.ThrowsException<NameRequiredException>(
                () => sut.Save(contact.ContactId, contact));
            mockContactRepository
                .Verify(c => c.Create(contact), Times.Never());
        }

        [TestMethod]
        public void Save_WithBlankLastName_ThrowsNameRequiredException()
        {
            // Arrange
            contact.LastName = "";

            // Assert
            Assert.ThrowsException<NameRequiredException>(
               () => sut.Save(contact.ContactId, contact));
            mockContactRepository
                .Verify(c => c.Create(contact), Times.Never());
        }

        [TestMethod]
        public void Save_WithBlankPhoneNumber_ThrowsMobileNumberRequiredException()
        {
            contact.MobilePhone = "";

            Assert.ThrowsException<MobileNumberRequiredException>(
               () => sut.Save(contact.ContactId, contact));
            mockContactRepository
                .Verify(c => c.Create(contact), Times.Never());
        }

        [TestMethod]
        public void Save_WithBlankStreetAddress_ThrowsAddressRequiredException()
        {
            contact.StreetAddress = "";

            Assert.ThrowsException<AddressRequiredException>(
               () => sut.Save(contact.ContactId, contact));
            mockContactRepository
                .Verify(c => c.Create(contact), Times.Never());
        }

        [TestMethod]
        public void Save_WithBlankCityAddress_ThrowsAddressRequiredException()
        {
            contact.CityAddress = "";

            Assert.ThrowsException<AddressRequiredException>(
               () => sut.Save(contact.ContactId, contact));
            mockContactRepository
                .Verify(c => c.Create(contact), Times.Never());
        }

        [TestMethod]
        public void Save_ZipWithNegativeValue_ThrowsPositiveZipCodeRequiredException()
        {
            contact.ZipCode = -1427;

            Assert.ThrowsException<PositiveZipCodeRequiredException>(
              () => sut.Save(contact.ContactId, contact));
            mockContactRepository
                .Verify(c => c.Create(contact), Times.Never());
        }

        [TestMethod]
        public void Save_WithBlankCountry_ThrowsCountryRequiredException()
        {
            contact.Country = "";

            Assert.ThrowsException<CountryRequiredException>(
              () => sut.Save(contact.ContactId,contact));
            mockContactRepository
                .Verify(c => c.Create(contact), Times.Never());
        }

    }
}
