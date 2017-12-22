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
        private Contact _contact = null;
        private string _connectionString;
        private DbContextOptions<DigiBookDbContext> _dbOptions;
        private DigiBookDbContext _dbContext;
        private ContactRepository _sut;

        [TestInitialize]
        public void Initialize()
        {
            _contact = new Contact
            {
                Firstname = "Matt",
                Lastname = "Mendez",
                MobilePhone = "09293235700",
                StreetAddress = "318 Saint Michael St., Brgy. Holy Spirit",
                CityAddress = "Quezon City",
                ZipCode = 1127,
                EmailAddress = "mmendez@blastasia.com",
                IsActive = false,
                DateActivated = new Nullable<DateTime>()
            };

            _connectionString = @"Server=(localdb)\MSSQLLocalDB;Database=DigiBookDb;Trusted_Connection=True;";
            _dbOptions = new DbContextOptionsBuilder<DigiBookDbContext>()
                .UseSqlServer(_connectionString)
                .Options;

            _dbContext = new DigiBookDbContext(_dbOptions);

            _sut = new ContactRepository(_dbContext);
            _dbContext.Database.EnsureCreated();
        }

        [TestCleanup]
        public void CleanUp()
        {
            _dbContext.Dispose();
            _dbContext = null;
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Create_WithValidContactData_SavesRecordInTheDatabase ()
        {
            // Arrange
            
            
            // Act
            var newContact = _sut.Create(_contact);

            // Assert
            Assert.IsNotNull(newContact);
            Assert.IsTrue(newContact.ContactId != Guid.Empty);

            //Cleanup
            _sut.Delete(newContact.ContactId);
            
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Delete_WithExistingContact_RemovesDataFromDatabase()
        {
            var newContact = _sut.Create(_contact);

            _sut.Delete(_contact.ContactId);

            // Assert
            _contact = _sut.Retrieve(newContact.ContactId);
            Assert.IsNull(_contact);
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Retrieve_WithExistingContactId_ReturnsRecordFromDb()
        {
            // Arrange
            var newcontact = _sut.Create(_contact);

            // Act
            var found = _sut.Retrieve(_contact.ContactId);

            // Assert
            Assert.IsNotNull(found);

            //CleanUp
            _sut.Delete(found.ContactId);


        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Update_WithExistingContactId_ShouldUpdateRecordFromDb()
        {

            var newcontact = _sut.Create(_contact);
            var expectedFirstName = "James"; //Matt
            var expectedLastName = "Montemagno"; //Mendez
            var expectedEmail = "jmontemagno@xamarin.com"; //mmendez@blastasia.com

            newcontact.Firstname = expectedFirstName;
            newcontact.Lastname = expectedLastName;
            newcontact.EmailAddress = expectedEmail;

            // Act
            _sut.Update(newcontact.ContactId, _contact);

            //Assert
            var updatedContact = _sut.Retrieve(newcontact.ContactId);
            Assert.AreEqual(expectedFirstName, updatedContact.Firstname);
            Assert.AreEqual(expectedLastName, updatedContact.Lastname);
            Assert.AreEqual(expectedEmail, updatedContact.EmailAddress);

            // CleanUp
            _sut.Delete(updatedContact.ContactId);

        }
    }
}
