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
        public void InitializeTest()
        {
            contact = new Contact
            {
                FirstName = "Jasmin",
                LastName = "Magdaleno",
                MobilePhone = "09057002880",
                StreetAddress = "L2 B15 Utex St., Litex Vill., San Jose",
                CityAddress = "Rodriguez, Rizal",
                ZipCode = 1860,
                Country = "Philippines",
                EmailAddress = "jasminmagdaleno@blastasia.com",
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
            //Act
            var result = sut.Create(contact);

            //Assert
            mockContactRepository
                .Verify(c => c.Create(contact), Times.Once());
        }

        [TestMethod]
        public void Create_WithValidData_ReturnsNewContactWithContactID()
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

            //Assert
            Assert.ThrowsException<NameRequiredException>(
                () => sut.Create(contact));

            mockContactRepository
                .Verify(c => c.Create(contact), Times.Never());
        }

        [TestMethod]
        public void Create_WithBlankLastName_ThrowsNameRequiredException()
        {
            //Arrange
            contact.LastName = "";

            //Assert
            Assert.ThrowsException<NameRequiredException>(
                () => sut.Create(contact));

            mockContactRepository
                .Verify(c => c.Create(contact), Times.Never());
        }

        [TestMethod]
        public void Create_WithBlankMobileNumber_ThrowsMobileNumberRequiredException()
        {
            //Arrange
            contact.MobilePhone = "";

            //Assert
            Assert.ThrowsException<MobileNumberRequiredException>(
                () => sut.Create(contact));

            mockContactRepository
                .Verify(c => c.Create(contact), Times.Never());
        }

        [TestMethod]
        public void Create_WithBlankStreetAddress_ThrowsAddressRequiredException()
        {
            //Arrange
            contact.StreetAddress = "";

            //Assert
            Assert.ThrowsException<AddressRequiredException>(
                () => sut.Create(contact));

            mockContactRepository
                .Verify(c => c.Create(contact), Times.Never());
        }

        [TestMethod]
        public void Create_WithBlankCityAddress_ThrowsAddressRequiredException()
        {
            //Arrange
            contact.CityAddress = "";

            //Assert
            Assert.ThrowsException<AddressRequiredException>(
                () => sut.Create(contact));

            mockContactRepository
                .Verify(c => c.Create(contact), Times.Never());
        }

        [TestMethod]
        public void Create_WithBlankZipCode_ThrowsAddressRequiredException()
        {
            //Arrange
            contact.ZipCode = 0;

            //Assert
            Assert.ThrowsException<AddressRequiredException>(
                () => sut.Create(contact));

            mockContactRepository
                .Verify(c => c.Create(contact), Times.Never());
        }

        [TestMethod]
        public void Create_WithNegativeZipCode_ThrowsValidZipCodeRequiredException()
        {
            //Arrange
            contact.ZipCode = -1;

            //Assert
            Assert.ThrowsException<ValidZipCodeRequiredException>(
                () => sut.Create(contact));

            mockContactRepository
                .Verify(c => c.Create(contact), Times.Never());
        }

        [TestMethod]
        public void Create_WithBlankCountry_ThrowsAddressRequiredException()
        {
            //Arrange
            contact.Country = "";

            //Assert
            Assert.ThrowsException<AddressRequiredException>(
                () => sut.Create(contact));

            mockContactRepository
                .Verify(c => c.Create(contact), Times.Never());
        }

        [TestMethod]
        public void Create_WithInvalidEmailAddress_ThrowsEmailAddressRequiredException()
        {
            //Arrange
            contact.EmailAddress = "jmagdalenoblastasiacom";

            //Assert
            Assert.ThrowsException<EmailAddressRequiredException>(
                    () => sut.Create(contact));
            mockContactRepository
                .Verify(c => c.Create(contact), Times.Never());
        }
    }
}
