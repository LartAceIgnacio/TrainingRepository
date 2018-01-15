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

        private Contact contact = null;
        private DbContextOptions<DigiBookDbContext> dbOptions = null;
        private DigiBookDbContext dbContext = null;
        private String connectionString = null;
        private ContactRepository sut = null;


        [TestInitialize]
        public void TestInitialize()
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
        public void TestCleanUp()
        {
            dbContext.Dispose();
            dbContext = null;
        }


        [TestMethod]
        [TestProperty("TestType","Integration")]
        public void Create_WithValidData_SavesRecordsInTheDatabase()
        {

            

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
        public void Delete_WithAnExistingContact_RemovesRecordFromData()
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
        [TestProperty("TestType","Integration")]
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
        [TestProperty("TestType", "Integration")]
        public void Update_WithValidData_SavesUpdatesInDb()
        {
            //Arrange
            var newContact = sut.Create(contact);
            var expectedFirstName = "Angelou"; //from Gelo
            var expectedLastName = "Acosta"; //from celis
            var expectedMobileNUmber = "09275663255"; //from 09266026333         
            var expectedStreetAddress = "122 Sam Mateo St. Cebu"; //from 1325 San Diego St. Sampaloc Manila
            var expectedCityAddress = "Cebu City"; //from Manila City
            var expectedZipCode = 1811; //from 1800
            var expectedCountry = "USA"; //from Philipppines
            var expectedEmail = "anjacelis21@gmail.com"; //anjacelis@outlook.com
            var expectedIsActive = true; //from false
            var expectedDateActivated = DateTime.Today; //from null



            newContact.FirstName = expectedFirstName;
            newContact.LastName = expectedLastName;
            newContact.MobilePhone = expectedMobileNUmber;
            newContact.StreetAddress = expectedStreetAddress;
            newContact.CityAddress = expectedCityAddress;
            newContact.ZipCode = expectedZipCode;
            newContact.Country = expectedCountry;
            newContact.EmailAddress = expectedEmail;
            newContact.IsActive = expectedIsActive;
            newContact.DateActivated = expectedDateActivated;
            
            //Act

            sut.Update(newContact.ContactId,newContact);

            //Assert
            var updatedContact = sut.Retrieve(newContact.ContactId);
            Assert.AreEqual(expectedFirstName, updatedContact.FirstName);
            Assert.AreEqual(expectedLastName, updatedContact.LastName);
            Assert.AreEqual(expectedMobileNUmber, updatedContact.MobilePhone);
            Assert.AreEqual(expectedStreetAddress, updatedContact.StreetAddress);
            Assert.AreEqual(expectedCityAddress, updatedContact.CityAddress);
            Assert.AreEqual(expectedZipCode, updatedContact.ZipCode);
            Assert.AreEqual(expectedCountry, updatedContact.Country);
            Assert.AreEqual(expectedEmail, updatedContact.EmailAddress);
            Assert.AreEqual(expectedIsActive, updatedContact.IsActive);
            Assert.AreEqual(expectedDateActivated, updatedContact.DateActivated);

            //CleanUp
            sut.Delete(updatedContact.ContactId);
        }
    }
}
