
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

            mockContactRepository
               .Setup(c => c.Create(contact))
               .Callback(() => contact.ContactId = Guid.NewGuid())
               .Returns(contact);

            mockContactRepository
                .Setup(c => c.Retrieve(existingContactId))
                .Returns(contact);

            mockContactRepository
              .Setup(c => c.Retrieve(nonExistingContactId))
              .Returns<Contact>(null);
        }


        [TestMethod]
        public void Save_NewContactWithValidData_ShouldCallRepositoryCreate()
        {
            // Act
            var result = sut.Save(contact.ContactId,contact);

            // Assert
            mockContactRepository
                .Verify(c => c.Retrieve(nonExistingContactId), Times.Once);

            mockContactRepository
                .Verify(c => c.Create(contact), Times.Once());
        }

        [TestMethod]
        public void Save_WithValidData_ReturnsNewContactWithContactId()
        {
            mockContactRepository
               .Setup(c => c.Create(contact))
               .Callback(() => contact.ContactId = Guid.NewGuid())
               .Returns(contact);

            mockContactRepository
                .Setup(c => c.Retrieve(existingContactId))
                .Returns(contact);

            mockContactRepository
              .Setup(c => c.Retrieve(nonExistingContactId))
              .Returns<Contact>(null);


            // Act

            var newContact = sut.Save(contact.ContactId, contact);

            //Assert
            Assert.IsTrue(newContact.ContactId != Guid.Empty);

        }

        [TestMethod]
        public void Save_WithBlankFirstName_ThrowsNameRequiredException()
        {
            // Arrange 
            contact.FirstName = "";

            //Asert
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

            //Asert
            Assert.ThrowsException<NameRequiredException>(
                () => sut.Save(contact.ContactId, contact));

            mockContactRepository
              .Verify(c => c.Create(contact), Times.Never());
        }

        [TestMethod]
        public void Save_WithBlankMobilePhone_ThrowsMobileRequiredException()
        {
            contact.MobilePhone = "";

            //Asert
            Assert.ThrowsException<MobilePhoneRequiredException>(
                () => sut.Save(contact.ContactId, contact));

            mockContactRepository
              .Verify(c => c.Create(contact), Times.Never());
        }

        [TestMethod]
        public void Save_WithBlankStreetAddress_ThrowsStreetAddressRequiredException()
        {
            contact.StreetAddress = "";

            //Asert
            Assert.ThrowsException<StrongAddressRequiredException>(
                () => sut.Save(contact.ContactId, contact));

            mockContactRepository
              .Verify(c => c.Create(contact), Times.Never());
        }

        [TestMethod]
        public void Create_WithBlankCityAddress_ThrowsAddressRequiredException()
        {
            contact.CityAddress = "";

            //Asert
            Assert.ThrowsException<StrongAddressRequiredException>(
                () => sut.Save(contact.ContactId, contact));

            mockContactRepository
              .Verify(c => c.Create(contact), Times.Never());
        }

        [TestMethod]
        public void Create_WithZipCode_ThrowsZipCodeRequiredException()
        {
            contact.ZipCode = -1800;

            //Asert
            Assert.ThrowsException<ZipCodeRequiredException>(
                () => sut.Save(contact.ContactId, contact));

            mockContactRepository
              .Verify(c => c.Create(contact), Times.Never());

        }

        [TestMethod]
        public void Save_WithCountry_ThrowsCountryRequiredException()
        {
            contact.Country = "";

            //Asert
            Assert.ThrowsException<StrongAddressRequiredException>(
                () => sut.Save(contact.ContactId, contact));

            mockContactRepository
              .Verify(c => c.Create(contact), Times.Never());
        }

        [TestMethod]
        public void Save_WithExistingContact_CallsRepositoryUpdate()
        {
            // Arrange 
            contact.ContactId = existingContactId;

            //Act 
            sut.Save(contact.ContactId, contact);

            //Assert
            mockContactRepository
                .Verify(c => c.Retrieve(existingContactId), Times.Once);

            mockContactRepository
               .Verify(c => c.Update(existingContactId,contact), Times.Once);
        }
      
     


    }
}
