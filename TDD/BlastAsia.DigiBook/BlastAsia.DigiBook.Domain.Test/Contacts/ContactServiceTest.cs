
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
                FirstName = "Eugene",
                LastName = "Ravina",
                MobilePhone = "09277109530",
                StreetAddress = "Blk2 Lot 5 Salvador Camp",
                CityAddress = "Montalban",
                ZipCode = 1800,
                Country = "Philippines",
                EmailAddress = "eravina@blastasia.com",
                IsActive = false,
                DateActivated = new Nullable<DateTime>()
            };

            mockContactRepository = new Mock<IContactRepository>();
            sut = new ContactService(mockContactRepository.Object);

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

            //Assert
            Assert.IsTrue(newContact.ContactId != Guid.Empty);

        }

        [TestMethod]
        public void Create_WithBlankFirstName_ThrowsNameRequiredException()
        {
            // Arrange 
            contact.FirstName = "";

            //Asert
            Assert.ThrowsException<NameRequiredException>(
                () => sut.Create(contact));

            mockContactRepository
              .Verify(c => c.Create(contact), Times.Never());
        }
        [TestMethod]
        public void Create_WithBlankLastName_ThrowsNameRequiredException()
        {
            // Arrange 
            contact.LastName = "";

            //Asert
            Assert.ThrowsException<NameRequiredException>(
                () => sut.Create(contact));

            mockContactRepository
              .Verify(c => c.Create(contact), Times.Never());
        }

        [TestMethod]
        public void Create_WithBlankMobilePhone_ThrowsMobileRequiredException()
        {
            contact.MobilePhone = "";

            //Asert
            Assert.ThrowsException<MobilePhoneRequiredException>(
                () => sut.Create(contact));

            mockContactRepository
              .Verify(c => c.Create(contact), Times.Never());
        }

        [TestMethod]
        public void Create_WithBlankStreetAddress_ThrowsStreetAddressRequiredException()
        {
            contact.StreetAddress = "";

            //Asert
            Assert.ThrowsException<StrongAddressRequiredException>(
                () => sut.Create(contact));

            mockContactRepository
              .Verify(c => c.Create(contact), Times.Never());
        }

        [TestMethod]
        public void Create_WithBlankCityAddress_ThrowsAddressRequiredException()
        {
            contact.CityAddress = "";

            //Asert
            Assert.ThrowsException<StrongAddressRequiredException>(
                () => sut.Create(contact));

            mockContactRepository
              .Verify(c => c.Create(contact), Times.Never());
        }

        [TestMethod]
        public void Create_WithZipCode_ThrowsZipCodeRequiredException()
        {
            contact.ZipCode = -1800;

            //Asert
            Assert.ThrowsException<ZipCodeRequiredException>(
                () => sut.Create(contact));

            mockContactRepository
              .Verify(c => c.Create(contact), Times.Never());

        }

        [TestMethod]
        public void Create_WithCountry_ThrowsCountryRequiredException()
        {
            contact.Country = "";

            //Asert
            Assert.ThrowsException<StrongAddressRequiredException>(
                () => sut.Create(contact));

            mockContactRepository
              .Verify(c => c.Create(contact), Times.Never());
        }


    }
}
