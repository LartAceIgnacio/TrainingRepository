using BlastAsia.DigiBook.Domain.Contacts;
using BlastAsia.DigiBook.Domain.Test.Contacts.Contacts;
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
        private Mock<IContactRepository> mockContactRepository;
        ContactService sut;
        private Contact contact;
        private Guid existingContactId = Guid.NewGuid();
        private Guid nonExistingContactId = Guid.Empty;

        [TestInitialize]
        public void initializeTest()
        {
            mockContactRepository = new Mock<IContactRepository>(); // Contact Object

            sut = new ContactService(mockContactRepository.Object); // System Under Test

            contact = new Contact
            {
                FirstName = "Igi",
                LastName = "Abille",
                MobilePhone = "09568717617",
                StreetAddress = "B-10 L7 Narra St. North Ridge Park Subd., Sta. Monica, Nova.",
                CityAddress = "Quezon City",
                ZipCode = 1117,
                Country = "Philippines",
                EmailAddress = "luigiabille@gmail.com",
                IsActive = false,
                DateActivated = new Nullable<DateTime>()
            };
            //mockContactRepository
            //    .Setup(c => c.Create(contact))
            //        .Callback(() =>
            //        {
            //            contact.ContactId = Guid.NewGuid();
            //            contact.DateActivated = DateTime.Now;
            //        })
            //        .Returns(contact);
            // if  existing
            mockContactRepository
                .Setup(c => c.Retrieve(existingContactId))
                    .Returns(contact);
            // if non existing
            mockContactRepository
                .Setup(c => c.Retrieve(nonExistingContactId))
                    .Returns<Contact>(null);
        }
        [TestMethod]
        public void Save_NewContactWithValidData_ShouldCallRepositoryCreate()
        {
            // Arrange


            // Act

            var result = sut.Save(contact);

            // Assert

            mockContactRepository
                .Verify(cr => cr.Retrieve(nonExistingContactId), Times.Once());

            mockContactRepository
                .Verify(cr => cr.Create(contact), Times.Once());

        }
        [TestMethod]
        public void Save_WithValidData_ReturnsNewContactWithContactId()
        {
            // Arrange

            mockContactRepository
                .Setup(c => c.Create(contact))
                .Callback(() => contact.ContactId = Guid.NewGuid()) // Global Unique Identifier
                .Returns(contact);

            //public void SetContactId(Contact contact) //Instead of .Callback()
            //{
            //    contact.ContactId = Guid.NewGuid();
            //    Action<int, int> add = (a,b) => Console.WriteLine(a +b); // Action returns void
            //}

            // Act

            var newContact = sut.Save(contact);

            // Assert

            Assert.IsTrue(
                newContact.ContactId != Guid.Empty);
        }
        [TestMethod]
        public void Save_WithBlankFirstName_ThrowsNameRequiredException()
        {

            // Arrange


            contact.FirstName = "";

            // Act

            // Assert

            
            Assert.ThrowsException<NameRequiredException>(
                () => sut.Save(contact));

            mockContactRepository
                .Verify(c => c.Create(contact), Times.Never()); 
        }
        [TestMethod]
        public void Save_WithBlankLastName_ThrowsNameRequiredException()
        {

            // Arrange

            contact.LastName = "";

            // Act

            // Assert

            Assert.ThrowsException<NameRequiredException>(
                () => sut.Save(contact));

            mockContactRepository
                .Verify(c => c.Create(contact), Times.Never());

        }
        [TestMethod]
        public void Save_WithBlankMobilePhone_ThrowsMobilePhoneRequiredException()
        {

            // Arrange

            contact.MobilePhone = "";

            // Act

            // Assert


            Assert.ThrowsException<MobilePhoneRequiredException>(
                () => sut.Save(contact));

            mockContactRepository
               .Verify(c => c.Create(contact), Times.Never());

        }
        [TestMethod]
        public void Save_WithBlankStreetAddress_ThrowsStreetAddressRequiredException()
        {

            // Arrange

            contact.StreetAddress = "";

            // Act

            // Assert


            Assert.ThrowsException<AddressRequiredException>(
                () => sut.Save(contact));

            mockContactRepository
                .Verify(c => c.Create(contact), Times.Never());
        }
        [TestMethod]
        public void Save_WithBlankCityAddress_ThrowsCityAddressRequiredException()
        {

            // Arrange

            contact.CityAddress = "";

            // Act

            // Assert
            
            Assert.ThrowsException<AddressRequiredException>(
                () => sut.Save(contact));

            mockContactRepository
                .Verify(c => c.Create(contact), Times.Never());
        }
        [TestMethod]
        public void Save_WithNegativeZipCode_ThrowsZipCodeException()
        {

            // Arrange

            contact.ZipCode = -1;

            // Act

            // Assert
            
            Assert.ThrowsException<ZipCodeException>(
                () => sut.Save(contact));

            mockContactRepository
                .Verify(c => c.Create(contact), Times.Never());
        }
        [TestMethod]
        public void Save_WithBlankCountry_ThrowsCountryRequiredException()
        {

            // Arrange

            contact.Country = "";

            // Act

            // Assert

            Assert.ThrowsException<CountryRequiredException>(
                () => sut.Save(contact));

            mockContactRepository
                .Verify(c => c.Create(contact), Times.Never());

        }
        //[TestMethod]
        //public void Save_ValidEmail_ThrowsValidEmailRequiredException()
        //{
        //    // Arrange

        //    contact.EmailAddress = "luigiabillegmail.com";

        //    // Act

        //    // Assert

        //    mockContactRepository
        //        .Verify(c => c.Create(contact), Times.Never());

        //    Assert.ThrowsException<ValidEmailRequiredException>(
        //        () => sut.Create(contact));

        //}
        [TestMethod]
        public void Save_WIthExistingContact_CallsRepositoryUpdate()
        {
            //Arrange
            contact.ContactId = existingContactId;

            //Act
            sut.Save(contact);

            //Assert
            mockContactRepository
                .Verify(c => c.Retrieve(contact.ContactId),
                Times.Once);
            mockContactRepository
                .Verify(c => c.Update(existingContactId, contact),
                Times.Once);
        }

    }
}
