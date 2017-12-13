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
        public void CodeInitialize()
        {
            contact = new Contact
            {
                FirstName = "Gelo",
                LastName = "Celis",
                MobilePhone = "09266026333",
                StreetAddress = "1325 San Diego St. Sampaloc Manila",
                CityAddress = "Manila City",
                ZipCode = 1800,
                Country = "Philippines",
                EmailAddress = "anjacelis@outlook.com",
                IsActive = false,
                DateActivated = new Nullable<DateTime>()
            };

            mockContactRepository = new Mock<IContactRepository>();
            sut = new ContactService(mockContactRepository.Object);
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
        public void Create_WithBlankFirstName_ThrowsNameRquiredException()
        {
            //Arrange
            contact.FirstName = "";

            //Assert
            

            Assert.ThrowsException<NameRequiredException>(
                () => sut.Create(contact)
                );

            mockContactRepository
                .Verify(c => c.Create(contact), Times.Never());
        }

        [TestMethod]
        public void Create_WithBlanklastName_ThrowsNameRquiredException()
        {
            //Arrange
            contact.LastName = "";

            //Assert


            Assert.ThrowsException<NameRequiredException>(
                () => sut.Create(contact)
                );

            mockContactRepository
                .Verify(c => c.Create(contact), Times.Never());
        }

        [TestMethod]
        public void Create_WithBlankMobileNumber_ThrowsMobileNumberRquiredException()
        {
            //Arrange
            contact.MobilePhone = "";


            //Assert


            Assert.ThrowsException<MobileNumberRquiredException>(
                () => sut.Create(contact)
                );

            mockContactRepository
                .Verify(c => c.Create(contact), Times.Never());
        }

        [TestMethod]
        public void Create_WithBlankStreetAddress_ThrowsStreetAddressRquiredException()
        {
            //Arrange
            contact.StreetAddress = "";

            //Assert


            Assert.ThrowsException<StreetAddressRquiredException>(
                () => sut.Create(contact)
                );

            mockContactRepository
                .Verify(c => c.Create(contact), Times.Never());
        }

        [TestMethod]
        public void Create_WithBlankCityAddress_ThrowsCityAddressRquiredException()
        {
            //Arrange
            contact.CityAddress = "";

            //Assert


            Assert.ThrowsException<CityAddressRquiredException>(
                () => sut.Create(contact)
                );

            mockContactRepository
                .Verify(c => c.Create(contact), Times.Never());
        }

        [TestMethod]
        public void Create_WithNegativeZipCode_ThrowsZipCodeNegativeException()
        {
            //Arrange
            contact.ZipCode = -1800;

            //Assert


            Assert.ThrowsException<ZipCodeNegativeException>(
                () => sut.Create(contact)
                );

            mockContactRepository
                .Verify(c => c.Create(contact), Times.Never());
        }

        [TestMethod]
        public void Create_WithBlankCountry_ThrowsCountryRquiredException()
        {
            //Arrange
            contact.Country = "";

            //Assert


            Assert.ThrowsException<CountryRquiredException>(
                () => sut.Create(contact)
                );

            mockContactRepository
                .Verify(c => c.Create(contact), Times.Never());
        }
    }
}
