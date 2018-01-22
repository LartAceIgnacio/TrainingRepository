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
        private ContactRepository sut;

        [TestInitialize]
        public void TestInitialize()
        {
            contact = new Contact
            {
                FirstName = "John Karl",
                LastName = "Matencio",
                MobilePhone = "09957206817",
                EmailAddress = "jhnkrl15@gmail.com"
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
        public void TestCleanup()
        {
            dbContext.Dispose();
            dbContext = null;
        }

        [TestMethod]
        [TestProperty("TestType","Integration")]
        public void Create_WithValidData_SavesRecordToTheDatabase()
        {
            //Arrange
            //Act
            var newContact = sut.Create(contact);
            //Asser
            Assert.IsNotNull(newContact);
            Assert.IsTrue(newContact.ContactId != Guid.Empty);
            //Cleanup
            sut.Delete(newContact.ContactId);

        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Delete_WithAnExistingContact_RemovesRecordFromTheDatabase()
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
        public void Retrieve_WithAnExistingContactId_ReturnsRecordFromTheDatabase()
        {
            //Arrange
            var newContact = sut.Create(contact);
            //Act
            var found = sut.Retrieve(newContact.ContactId);
            //Assert
            Assert.IsNotNull(found);
            //Cleanup
            sut.Delete(found.ContactId);
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Update_WithExistingContactId_SavesUpdateInDatabase()
        {
            //Arrange
            var newContact = sut.Create(contact);
            var expectedFirstname = "Maureen"; //John Karl
            var expectedLastname = "Sebastian"; //Matencio
            var expectedEmail = "jmatencio@gmail.com"; //jhnkrl15@mgail.com
            var expectedMobilePhone = "09957206818"; //09957206817

            newContact.FirstName = expectedFirstname;
            newContact.LastName = expectedLastname;
            newContact.EmailAddress = expectedEmail;
            newContact.MobilePhone = expectedMobilePhone;
            //Act
            sut.Update(newContact.ContactId, newContact);
            //Assert
            var updatedContact = sut.Retrieve(newContact.ContactId);

            Assert.AreEqual(expectedFirstname,updatedContact.FirstName);
            Assert.AreEqual(expectedLastname, updatedContact.LastName);
            Assert.AreEqual(expectedEmail, updatedContact.EmailAddress);
            Assert.AreEqual(expectedMobilePhone, updatedContact.MobilePhone);
            //Cleanup
            sut.Delete(updatedContact.ContactId);
        }
    }
}
