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
        private Contact contact;
        private DbContextOptions<DigiBookDbContext> dbOptions = null;
        private DigiBookDbContext dbContext = null;
        private String connectionString = null;
        private ContactRepository sut;

        [TestInitialize]
        public void TestInitialize()
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
                DateActivated = DateTime.Today
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
        public void CleanUp()
        {
            dbContext.Dispose();
            dbContext = null;
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Create_WithValidData_SavesRecordInDatabase()
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
        public void Delete_WithAnExistingContact_RemovesRecordFromDatabase()
        {
            //Arrange

            var newContact = sut.Create(contact);

            //Act
            sut.Delete(contact.ContactId);

            //Assert
            contact = sut.Retrieve(newContact.ContactId);
            Assert.IsNull(contact);
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Retrieve_WithExistingContactId_ReturnsRecordFromDb()
        {
            //Arrange
            var newContact = sut.Create(contact);

            //Act
            var found = sut.Retrieve(newContact.ContactId);

            //Assert
            Assert.IsNotNull(found);

            //CleanUp
            sut.Delete(found.ContactId);
        }

        [TestMethod]
        [TestProperty("TestType","Integration")]
        public void Update_WithValidData_SavesUpdatesInDb()
        {
            //Arrange
            var newContact = sut.Create(contact);
            var expectedFirstName = "Sherlock"; //from Jasmin
            var expectedLastName = "Holmes"; //from Magdaleno
            var expectedMobilePhone = "09057002000"; //from 09057002991
            var expectedStreetAddress = "221B"; //from L2 B15 Utex St., Litex Vill., San Jose
            var expectedCityAddress = "London"; //from Rodriguez, Rizal
            var expectedZipCode = 4567; //from 1860
            var expectedCountry = "UK"; //from Philippines
            var expectedEmailAddress = "sHolmes@blastasia.com"; //from jasminmagdaleno@blastasia.com
            var expectedIsActive = true; //from false
            var expectedDateActivated = DateTime.Today.AddDays(-1); //from Date.Now

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

            //Act
            sut.Update(newContact.ContactId, newContact);

            //Assert
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

            //CleanUp
            sut.Delete(updatedContact.ContactId);
        }
    }
}
