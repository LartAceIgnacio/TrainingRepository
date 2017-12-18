using BlastAsia.DigiBook.Domain.Models.Contacts;
using BlastAsia.DigiBook.Infrastructure.Persistence.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BlastAsia.DigiBook.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace BlastAsia.Digibook.Infrastructure.Persistence.Test
{
    [TestClass]
    public class ContactRepositoryTest
    {

        private Contact contact = null;
        private DbContextOptions<DigiBookDbContext> dbOptions = null;
        private DigiBookDbContext dbContext = null;
        private String connectionString = null;
        ContactRepository sut;

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
                DateActivated = DateTime.Today
            };

            connectionString =
                @"Server=.;Database=DigiBookDb;Integrated Security=true;";
            dbOptions = new DbContextOptionsBuilder<DigiBookDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            dbContext = new DigiBookDbContext(dbOptions);
            dbContext.Database.EnsureCreated();

            sut = new ContactRepository(dbContext);
        }

        [TestCleanup]
        public void Cleanup()
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
            sut.Delete(contact.ContactId);
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
            sut.Delete(found.ContactId);
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Update_WithValidData_SavesUpdatesInDb()
        {
            // Arrange
            var newContact = sut.Create(contact);
            var expectedFirstName = "Abbie";
            var expectedLastName = "Veluz";
            var expectedMobilePhone = "02981642039";
            var expectedStreetAddress = "#8 Kakawati Street, Pangarap Village";
            var expectedCityAddress = "Quezon City";
            var expectedZipCode = 1400;
            var expectedCountry = "Japan";
            var expectedEmailAddress = "abbieolarte@yahoo.com";
            var expectedIsActive = true;
            var expectedDateActivated = DateTime.Today;

            newContact.FirstName = expectedFirstName;
            newContact.LastName = expectedLastName;
            newContact.MobilePhone = expectedMobilePhone;
            newContact.StreetAddress = expectedStreetAddress;
            newContact.CityAddress = expectedCityAddress;
            newContact.ZipCode = expectedZipCode;
            newContact.Country = expectedCountry;
            newContact.EmailAddress = expectedEmailAddress;
            newContact.IsActive = expectedIsActive;
            newContact.DateActivated = expectedDateActivated;

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
            Assert.AreEqual(expectedEmailAddress, updatedContact.EmailAddress);
            Assert.AreEqual(expectedIsActive, updatedContact.IsActive);
            Assert.AreEqual(expectedDateActivated, updatedContact.DateActivated);

            //Cleanup
            sut.Delete(updatedContact.ContactId);
        }

    }
}
