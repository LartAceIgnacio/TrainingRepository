using BlastAsia.Digibook.Domain.Models.Contacts;
using BlastAsia.Digibook.Infrastracture.Persistence;
using BlastAsia.Digibook.Infrastracture.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.Digibook.Infrastructure.Persistence.Test
{
    [TestClass]
    public class ContactRepositoryTest
    {
        private Contact contact = null;
        private DbContextOptions<DigiBookDbContext> dbOptions = null;
        private DigiBookDbContext dbContext = null;
        private string connectionString = null;
        private ContactRepository sut = null;

        [TestInitialize]
        public void Initialize_Data()
        {
            contact = new Contact
            {
                FirstName = "Alex",
                LastName = "Cano",
                MobilePhone = "09173723594",
                StreetAddress = "16 J. Cruz St., Parang",
                CityAddress = "Marikina City",
                ZipCode = 1809,
                Country = "Philippines",
                EmailAddress = "gcano@blastasia.com",
                IsActive = false,
                DateActive = new Nullable<DateTime>()
            };

            connectionString = @"Data Source=.;Database=DigiBookDb;Integrated Security=true;";
            dbOptions = new DbContextOptionsBuilder<DigiBookDbContext>()
                                    .UseSqlServer(connectionString)
                                    .Options;

            dbContext = new DigiBookDbContext(dbOptions);
            dbContext.Database.EnsureCreated();
            sut = new ContactRepository(dbContext);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            dbContext.Dispose();
            dbContext = null;
        }

        [TestMethod]
        [TestProperty("TestType","Integration")]
        public void Create_WithValidData_SavesRecordInTheDatabase()
        {
            var newContact = sut.Create(contact);

            Assert.IsNotNull(newContact);
            Assert.IsTrue(newContact.ContactId != Guid.Empty);

            sut.Delete(newContact.ContactId);
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Delete_WithAnExistingContact_RemoveRecordFromDatabase()
        {
            var newContact = sut.Create(contact);

            sut.Delete(newContact.ContactId);
            contact = sut.Retrieve(newContact.ContactId);
            Assert.IsNull(contact);
        }

        [TestMethod]
        [TestProperty("TestType","Integration")]
        public void Retrieve_WithExistingContactId_ReturnsRecordFromDb()
        {
            var newContact = sut.Create(contact);

            var found = sut.Retrieve(newContact.ContactId);

            Assert.IsNotNull(found);

            sut.Delete(found.ContactId);
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Update_WithValidData_SavesUpdatesInDb()
        {
            var newContact = sut.Create(contact);

            var expectedFirstName = "Linus";
            var expectedLastName = "Torvalds";
            var expectedEmail = "ltorvalds@linux.com";
            var expectedMobilePhone = "09123456789";
            var expectedStreetAddress = "13 Upper Burgos St.";
            var expectedCityAddress = "Baguio City";
            var expectedZipCode = 333;
            var expectedCountry = "Philippines";
            var expectedIsActive = false;
            var expectedDateActive = DateTime.Today;

        //other

            newContact.FirstName = expectedFirstName;
            newContact.LastName = expectedLastName;
            newContact.EmailAddress = expectedEmail;
            newContact.MobilePhone = expectedMobilePhone;
            newContact.StreetAddress = expectedStreetAddress;
            newContact.CityAddress = expectedCityAddress;
            newContact.ZipCode = expectedZipCode;
            newContact.Country = expectedCountry;
            newContact.IsActive = expectedIsActive;
            newContact.DateActive = expectedDateActive;

            sut.Update(newContact.ContactId,newContact);

            var updatedContact = sut.Retrieve(newContact.ContactId);
            Assert.AreEqual(expectedFirstName,newContact.FirstName);
            Assert.AreEqual(expectedLastName, newContact.LastName);
            Assert.AreEqual(expectedEmail, newContact.EmailAddress);
            Assert.AreEqual(expectedMobilePhone, newContact.MobilePhone);
            Assert.AreEqual(expectedStreetAddress, newContact.StreetAddress);
            Assert.AreEqual(expectedCityAddress, newContact.CityAddress);
            Assert.AreEqual(expectedZipCode, newContact.ZipCode);
            Assert.AreEqual(expectedCountry, newContact.Country);
            Assert.AreEqual(expectedIsActive, newContact.IsActive);
            Assert.AreEqual(expectedDateActive, newContact.DateActive);

            sut.Delete(updatedContact.ContactId);

        }
    }
}
