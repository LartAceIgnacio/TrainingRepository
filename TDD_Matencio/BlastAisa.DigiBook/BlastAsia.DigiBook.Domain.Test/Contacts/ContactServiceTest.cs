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
        ContactService sut;

        [TestInitialize]
        public void TestInitialize()
        {
            contact = new Contact
            {
                FirstName = "John Karl",
                LastName = "Matencio",
                MobilePhone = "09957206817",
                StreetAddress = "7th St. Metrogate, Dasmariñas",
                CityAddress = "Cavite",
                ZipCode = 1604,
                Country = "Philippines",
                EmailAddress = "jhnkrl15@gmail.com",
                isActive = false,
                DateActivated = new Nullable<DateTime>()
            };

            mockContactRepository = new Mock<IContactRepository>();
            sut = new ContactService(mockContactRepository.Object);
        }

        [TestCleanup]
        public void CleanupTeset()
        {

        }

        [TestMethod]
        public void Create_WithValidData_ShouldCallRepositoryCreate()
        {
            //Arrange
            //Act
            var result = sut.Create(contact);
            //Assert
            mockContactRepository
                .Verify(c => c.Create(contact), Times.Once());
        }

        [TestMethod]
        public void  Create_WithValidData_ReturnsNewContactWithContactID()
        {
            //Arrange
            mockContactRepository
                .Setup(c => c.Create(contact))
                .Callback(() => contact.ContactId = Guid.NewGuid())
                .Returns(contact);
            //Act
            var newContact = sut.Create(contact);
            //Assert
            Assert.IsTrue(newContact.ContactId != Guid.Empty);
        }

        [TestMethod]
        public void Create_WithBlankFirstName_ThrowsNameRequiredException()
        {
            //Arrange
            contact.FirstName = "";
            //Act
            //Assert
            Assert.ThrowsException<NameRequiredException>(
                () => sut.Create(contact));
            mockContactRepository.
              Verify(c => c.Create(contact), Times.Never);
        }

        [TestMethod]
        public void Create_WithBlankLastName_ThrowsNameRequiredException()
        {
            //Arrange
            contact.LastName = "";
            //Act
            //Assert
            Assert.ThrowsException<NameRequiredException>(
                () => sut.Create(contact));
            mockContactRepository.
              Verify(c => c.Create(contact), Times.Never);
        }

        [TestMethod]
        public void Create_WithBlankMobilePhone_ThrowsNameRequiredException()
        {
            //Arrange
            contact.MobilePhone = "";
            //Act
            //Assert
            Assert.ThrowsException<MobilePhoneRequiredException>(
                () => sut.Create(contact));
            mockContactRepository.
              Verify(c => c.Create(contact), Times.Never);
        }

        [TestMethod]
        public void Create_WithBlankStreetAddress_ThrowsNameRequiredException()
        {
            //Arrange
            contact.StreetAddress = "";
            //Act

            //Assert
            Assert.ThrowsException<AddressRequiredException>(
                () => sut.Create(contact));
            mockContactRepository.
              Verify(c => c.Create(contact), Times.Never);
        }

        [TestMethod]
        public void Create_WithBlankSCityAddress_ThrowsNameRequiredException()
        {
            //Arrange
            contact.CityAddress = "";
            //Act
            //Assert
            Assert.ThrowsException<AddressRequiredException>(
                () => sut.Create(contact));
            mockContactRepository.
              Verify(c => c.Create(contact), Times.Never);
        }

        [TestMethod]
        public void Create_WithNegativeZipcode_ThrowsNameRequiredException()
        {
            //Arrange
            contact.ZipCode = -1;
            //Act
            //Assert
            Assert.ThrowsException<ZipcodeShouldBePositiveException>(
                () => sut.Create(contact));
            mockContactRepository.
              Verify(c => c.Create(contact), Times.Never);
        }

        [TestMethod]
        public void Create_WithBlankCountry_ThrowsNameRequiredException()
        {
            //Arrange
            contact.Country = "";
            //Act
            //Assert
            Assert.ThrowsException<CountryRequiredException>(
                () => sut.Create(contact));
            mockContactRepository.
              Verify(c => c.Create(contact), Times.Never);
        }
    }
}
