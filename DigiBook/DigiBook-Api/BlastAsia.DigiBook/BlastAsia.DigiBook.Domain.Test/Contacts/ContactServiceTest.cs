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
        private Guid existingContactId;
        private Guid nonExistingContactId;

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

            existingContactId = Guid.NewGuid();
            nonExistingContactId = Guid.Empty;

            mockContactRepository
                .Setup(c => c.Retrieve(nonExistingContactId))
                .Returns<Contact>(null);

            mockContactRepository
                .Setup(c => c.Retrieve(existingContactId))
                .Returns(contact);
            
        }



        [TestMethod]
        public void Save_NewContactWithValidData_ShouldCallRepositoryCreate()
        {

            //Act

            var result = sut.Save(contact.ContactId, contact);

            //Assert
            mockContactRepository
                .Verify(c => c.Retrieve(nonExistingContactId), Times.Once());
                    

            mockContactRepository
                .Verify(c => c.Create(contact), Times.Once());


        }

        [TestMethod]
        public void Save_WithExistingContact_CallsRepositoryUpdate()
        {
            //Arrange
            contact.ContactId = existingContactId;

            //Act
            sut.Save(contact.ContactId, contact);

            //Assert
            mockContactRepository
                .Verify(c => c.Retrieve(existingContactId), Times.Once());

            mockContactRepository
                .Verify(c => c.Update(existingContactId, contact), Times.Once());
        }

        [TestMethod]
        public void Save_WithValidData_ReturnsNewContactWithContactId()
        {
            //Arrange
            

            mockContactRepository
                .Setup(c => c.Create(contact))
                .Callback(() => contact.ContactId = Guid.NewGuid())
                .Returns(contact);

            //Act
            var newContact = sut.Save(contact.ContactId, contact);

            //Assert

         
            Assert.IsTrue(newContact.ContactId != Guid.Empty);

        }

        [TestMethod]
        public void Save_WithBlankFirstName_ThrowsNameRquiredException()
        {
            //Arrange
            contact.FirstName = "";

            //Assert
            

            Assert.ThrowsException<NameRequiredException>(
                () => sut.Save(contact.ContactId, contact)
                );

            mockContactRepository
                .Verify(c => c.Create(contact), Times.Never());
        }

        [TestMethod]
        public void Save_WithBlanklastName_ThrowsNameRquiredException()
        {
            //Arrange
            contact.LastName = "";

            //Assert


            Assert.ThrowsException<NameRequiredException>(
                () => sut.Save(contact.ContactId, contact)
                );

            mockContactRepository
                .Verify(c => c.Create(contact), Times.Never());
        }

        [TestMethod]
        public void Save_WithBlankMobileNumber_ThrowsMobileNumberRquiredException()
        {
            //Arrange
            contact.MobilePhone = "";


            //Assert


            Assert.ThrowsException<MobileNumberRquiredException>(
                () => sut.Save(contact.ContactId, contact)
                );

            mockContactRepository
                .Verify(c => c.Create(contact), Times.Never());
        }

        [TestMethod]
        public void Save_WithBlankStreetAddress_ThrowsStreetAddressRquiredException()
        {
            //Arrange
            contact.StreetAddress = "";

            //Assert


            Assert.ThrowsException<StreetAddressRquiredException>(
                () => sut.Save(contact.ContactId, contact)
                );

            mockContactRepository
                .Verify(c => c.Create(contact), Times.Never());
        }

        [TestMethod]
        public void Save_WithBlankCityAddress_ThrowsCityAddressRquiredException()
        {
            //Arrange
            contact.CityAddress = "";

            //Assert


            Assert.ThrowsException<CityAddressRquiredException>(
                () => sut.Save(contact.ContactId, contact)
                );

            mockContactRepository
                .Verify(c => c.Create(contact), Times.Never());
        }

        [TestMethod]
        public void Save_WithNegativeZipCode_ThrowsZipCodeNegativeException()
        {
            //Arrange
            contact.ZipCode = -1800;

            //Assert


            Assert.ThrowsException<ZipCodeNegativeException>(
                () => sut.Save(contact.ContactId, contact)
                );

            mockContactRepository
                .Verify(c => c.Create(contact), Times.Never());
        }

        [TestMethod]
        public void Save_WithBlankCountry_ThrowsCountryRequiredException()
        {
            //Arrange
            contact.Country = "";

            //Assert


            Assert.ThrowsException<CountryRequiredException>(
                () => sut.Save(contact.ContactId,contact)
                );

            mockContactRepository
                .Verify(c => c.Create(contact), Times.Never());
        }
    }
}
