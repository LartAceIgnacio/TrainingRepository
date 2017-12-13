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
            sut = new ContactService(mockContactRepository.Object);
        }

        [TestCleanup]
        public void CleanUp()
        {

        }

        [TestMethod]
        public void Create_WithValidData_ShouldCallRepositoryCreate()
        {
            // Act
            var result = sut.Create(contact);

            // Assert
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
            contact = new Contact
            {
                FirstName = "",
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
            
            // Assert
            Assert.ThrowsException<NameRequiredException>(
                () => sut.Create(contact));
            mockContactRepository
                .Verify(c => c.Create(contact), Times.Never());
        }

        [TestMethod]
        public void Create_WithBlankLastName_ThrowsNameRequiredException()
        {
            // Arrange
            contact = new Contact
            {
                FirstName = "Angela Blanche",
                LastName = "",
                MobilePhone = "09981642039",
                StreetAddress = "#9 Kakawati Street, Pangarap Village",
                CityAddress = "Caloocan City",
                ZipCode = 1427,
                Country = "Philippines",
                EmailAddress = "abbieolarte@gmail.com",
                IsActive = false,
                DateActivated = new Nullable<DateTime>()
            };

            // Assert
            Assert.ThrowsException<NameRequiredException>(
               () => sut.Create(contact));
            mockContactRepository
                .Verify(c => c.Create(contact), Times.Never());
        }

        [TestMethod]
        public void Create_WithBlankPhoneNumber_ThrowsMobileNumberRequiredException()
        {
            contact = new Contact
            {
                FirstName = "Angela Blanche",
                LastName = "Olarte",
                MobilePhone = "",
                StreetAddress = "#9 Kakawati Street, Pangarap Village",
                CityAddress = "Caloocan City",
                ZipCode = 1427,
                Country = "Philippines",
                EmailAddress = "abbieolarte@gmail.com",
                IsActive = false,
                DateActivated = new Nullable<DateTime>()
            };

            Assert.ThrowsException<MobileNumberRequiredException>(
               () => sut.Create(contact));
            mockContactRepository
                .Verify(c => c.Create(contact), Times.Never());
        }

        [TestMethod]
        public void Create_WithBlankStreetAddress_ThrowsAddressRequiredException()
        {
            contact = new Contact
            {
                FirstName = "Angela Blanche",
                LastName = "Olarte",
                MobilePhone = "09981672039",
                StreetAddress = "",
                CityAddress = "Caloocan City",
                ZipCode = 1427,
                Country = "Philippines",
                EmailAddress = "abbieolarte@gmail.com",
                IsActive = false,
                DateActivated = new Nullable<DateTime>()
            };

            Assert.ThrowsException<AddressRequiredException>(
               () => sut.Create(contact));
            mockContactRepository
                .Verify(c => c.Create(contact), Times.Never());
        }

        [TestMethod]
        public void Create_WithBlankCityAddress_ThrowsAddressRequiredException()
        {
            contact = new Contact
            {
                FirstName = "Angela Blanche",
                LastName = "Olarte",
                MobilePhone = "09981642039",
                StreetAddress = "#9 Kakawati Street, Pangarap Village",
                CityAddress = "",
                ZipCode = 1427,
                Country = "Philippines",
                EmailAddress = "abbieolarte@gmail.com",
                IsActive = false,
                DateActivated = new Nullable<DateTime>()
            };

            Assert.ThrowsException<AddressRequiredException>(
               () => sut.Create(contact));
            mockContactRepository
                .Verify(c => c.Create(contact), Times.Never());
        }

        [TestMethod]
        public void Create_WithNegativeValue_ThrowsPositiveZipCodeRequiredException()
        {
            contact = new Contact
            {
                FirstName = "Angela Blanche",
                LastName = "Olarte",
                MobilePhone = "09981642039",
                StreetAddress = "#9 Kakawati Street, Pangarap Village",
                CityAddress = "Caloocan City",
                ZipCode = -1427,
                Country = "Philippines",
                EmailAddress = "abbieolarte@gmail.com",
                IsActive = false,
                DateActivated = new Nullable<DateTime>()
            };

            Assert.ThrowsException<PositiveZipCodeRequiredException>(
              () => sut.Create(contact));
            mockContactRepository
                .Verify(c => c.Create(contact), Times.Never());
        }

        [TestMethod]
        public void Create_WithBlankCountry_ThrowsCountryRequiredException()
        {
            contact = new Contact
            {
                FirstName = "Angela Blanche",
                LastName = "Olarte",
                MobilePhone = "09981642039",
                StreetAddress = "#9 Kakawati Street, Pangarap Village",
                CityAddress = "Caloocan City",
                ZipCode = 0,
                Country = "",
                EmailAddress = "abbieolarte@gmail.com",
                IsActive = false,
                DateActivated = new Nullable<DateTime>()
            };

            Assert.ThrowsException<CountryRequiredException>(
              () => sut.Create(contact));
            mockContactRepository
                .Verify(c => c.Create(contact), Times.Never());
        }
    }
}
