using BlastAsia.DigiBook.Domain.Models.Contacts;
using BlastAsia.DigiBook.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Infrastructure.Persistence.Test
{   [TestClass]
    public class ContactRepositoryTest
    {
        private Contact contact = null;
        private DbContextOptions<DigiBookDbContext> dbOptions = null;
        private DigiBookDbContext dbContext = null;
        private String connectionString = null;
        private ContactRepository sut = null;

        [TestInitialize]
        public void InitializeTest()
        {

            contact = new Contact
            {
                FirstName = "Duane",
                LastName = "De Guzman",
                MobilePhone = "09158959384",
                StreetAddress = "264 Quezon st Cuyab",
                CityAddress = "San Pedro, Laguna",
                ZipCode = 4023, 
                Country = "Philippines",
                EmailAddress = "dlfdeguzman@outlook.com",
                IsActive = false,
            };
            connectionString
                = @"Data Source=.;Database=DigiBookDb;Integrated Security=true;";
            dbOptions = new DbContextOptionsBuilder<DigiBookDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            dbContext = new DigiBookDbContext(dbOptions);
            dbContext.Database.EnsureCreated();

            sut = new ContactRepository(dbContext);
        }

        [TestCleanup]
        public void CleanupTest()
        {
            dbContext.Dispose();
            dbContext = null;
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Create_WithValidData_SavesRecordInTheDatabase() 
        {
            // Act
            var newContact = sut.Create(contact);
            
            // Assert
            Assert.IsNotNull(newContact);
            Assert.IsTrue(newContact.ContactId != Guid.Empty);

            // Cleanup
            sut.Delete(newContact.ContactId);
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Delete_WithAnExistingContact_RemovesRecordFromDatabase()
        {
            // Arrange
            var newContact = sut.Create(contact);

            // Act
            sut.Delete(newContact.ContactId);

            // Assert
            contact = sut.Retrieve(newContact.ContactId);
            Assert.IsNull(contact);
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Retrieve_WithExistingContactId_ReturnsRecordFromDb()
        {
            // Arrange
            var newContact = sut.Create(contact);

            // Act
            var found = sut.Retrieve(newContact.ContactId);

            // Assert
            Assert.IsNotNull(found);

            // Cleanup
            sut.Delete(found.ContactId);
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Update_WithValidData_SavesUpdatesInDb()
        {
            // Arrange
            var newContact = sut.Create(contact);
            var expectedFirstName = "Lester"; // from Duane
            var expectedLastName = "Francisco"; // from De Guzman
            var expectedMobilePhone = "09123456789"; // from 09158959384
            var expectedStreetAddress = "244 Quezon st Cuyab"; // from 264 Quezon st Cuyab
            var expectedCityAddress = "Binan, Laguna"; // from San Pedro, Laguna
            var expectedZipCode = 4000; // from 4023
            var expectedCountry = "US"; // from Philippines
            var expectedEmail = "duanedeguzman@gmail.com"; // from deguzmanduane@gmail.com
            var expectedIsActive = true; // from false

            newContact.FirstName = expectedFirstName;
            newContact.LastName = expectedLastName;
            newContact.MobilePhone = expectedMobilePhone;
            newContact.StreetAddress = expectedStreetAddress;
            newContact.CityAddress = expectedCityAddress;
            newContact.ZipCode = expectedZipCode;
            newContact.Country = expectedCountry;
            newContact.EmailAddress = expectedEmail;
            newContact.IsActive = expectedIsActive;

            // Act
            sut.Update(newContact.ContactId, newContact);

            // Assert
            var updatedContact = sut.Retrieve(newContact.ContactId);
            Assert.AreEqual(expectedFirstName, updatedContact.FirstName);
            Assert.AreEqual(expectedLastName, updatedContact.LastName);
            Assert.AreEqual(expectedMobilePhone, updatedContact.MobilePhone);
            Assert.AreEqual(expectedStreetAddress, updatedContact.StreetAddress);
            Assert.AreEqual(expectedCityAddress, updatedContact.CityAddress);
            Assert.AreEqual(expectedZipCode, updatedContact.ZipCode);
            Assert.AreEqual(expectedCountry, updatedContact.Country);
            Assert.AreEqual(expectedEmail, updatedContact.EmailAddress);
            Assert.AreEqual(expectedIsActive, updatedContact.IsActive);

            // Cleanup
            sut.Delete(updatedContact.ContactId);
        }
    }

}
