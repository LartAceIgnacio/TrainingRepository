using BlastAsia.DigiBook.Domain.Models.Contacts;
using BlastAsia.DigiBook.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Infrastructure.Persistence.Test
{
    [TestClass]
    public class ContactRepositoryTest
    {
        public Contact contact = null;
        public string connectionString = null;
        public DbContextOptions<DigiBookDbContext> dbOptions = null;
        public DigiBookDbContext dbContext = null;
        public ContactRepository sut = null;

        [TestInitialize]
        public void Initialize()
        {
            contact = new Contact {
                FirstName = "Chris",
                LastName = "Manuel",
                MobilePhone = "09156879240",
                StreetAddress = "#3 Pananalig St., Brgy. Mabini JRizal",
                CityAddress = "Mandaluyong City",
                ZipCode = 1550,
                Country = "Philippines",
                EmailAddress = "cmanuel@blastasia.com",
                IsActive = false,
                DateActivated = new Nullable<DateTime>()
            };

            connectionString =
                @"Data Source=.;Database=DigiBookDb;Integrated Security=true;";

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
            //Arrange

            //Act
            var newContact = sut.Create(contact);

            //Assert
            Assert.IsNotNull(newContact);
            Assert.IsTrue(newContact.ContactId != Guid.Empty);

            //CleanUp
            sut.Delete(newContact.ContactId);
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Delete_WithExistingContact_RemovesRecordFromDatabase()
        {
            //Arrange
            var newContact = sut.Create(contact);

            //Act
            sut.Delete(newContact.ContactId);

            //Assert
            contact = sut.Retrieve(newContact.ContactId);
            Assert.IsNull(contact);
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Retrieve_WithExistingContactId_ReturnsRecordFromDatabase()
        {
            //Arrange
            var newContact = sut.Create(contact);

            //Act
            var found = sut.Retrieve(newContact.ContactId);

            //Assert
            Assert.IsNotNull(found);

            //Cleanup
            sut.Delete(newContact.ContactId);
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Update_WithValidData_SavesUpdatesInDatabase()
        {
            //Arrange
            var newContact = sut.Create(contact);
            var expectedFirstName = "Topeng";
            var expectedLastName = "Leunam";
            var expectedMobileNo = "09263135367";
            var expectedStreetAddress = "Blk 50 Lot 18 NorthVill 5";
            var expectedCityAddress = "Bocaue, Bulacan";
            var expectedZipCode = 3018;
            var expectedCountry = "PH";
            var expectedEmailAddress = "cmanuel@blastasia.com";
            var expectedIsActive = true;
            var expectedDateActivated = DateTime.Today;

            newContact.FirstName = expectedFirstName;
            newContact.LastName = expectedLastName;
            newContact.MobilePhone = expectedMobileNo;
            newContact.StreetAddress = expectedStreetAddress;
            newContact.CityAddress = expectedCityAddress;
            newContact.ZipCode = expectedZipCode;
            newContact.Country = expectedCountry;
            newContact.EmailAddress = expectedEmailAddress;
            newContact.IsActive = expectedIsActive;
            newContact.DateActivated = expectedDateActivated;
            
            //Act
            sut.Update(newContact.ContactId, newContact);

            //Assert
            var updatedContact = sut.Retrieve(newContact.ContactId);
            Assert.AreEqual(updatedContact.FirstName, newContact.FirstName);
            Assert.AreEqual(updatedContact.LastName, newContact.LastName);
            Assert.AreEqual(updatedContact.MobilePhone, newContact.MobilePhone);
            Assert.AreEqual(updatedContact.StreetAddress, newContact.StreetAddress);
            Assert.AreEqual(updatedContact.CityAddress, newContact.CityAddress);
            Assert.AreEqual(updatedContact.ZipCode, newContact.ZipCode);
            Assert.AreEqual(updatedContact.Country, newContact.Country);
            Assert.AreEqual(updatedContact.EmailAddress, newContact.EmailAddress);
            Assert.AreEqual(updatedContact.IsActive, newContact.IsActive);
            Assert.AreEqual(updatedContact.DateActivated, newContact.DateActivated);
            
            //Cleanup
            sut.Delete(newContact.ContactId);
        }
    }
}
