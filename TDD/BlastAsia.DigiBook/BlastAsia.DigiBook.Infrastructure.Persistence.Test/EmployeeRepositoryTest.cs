﻿using BlastAsia.DigiBook.Domain.Models.Employees;
using BlastAsia.DigiBook.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BlastAsia.DigiBook.Infrastructure.Persistence.Test
{
    [TestClass]
    public class EmployeeRepositoryTest
    {
        private Employee _employee = null;
        private string _connectionString;
        private DbContextOptions<DigiBookDbContext> _dbOptions;
        private DigiBookDbContext _dbContext;
        private EmployeeRepository _sut;

        [TestInitialize]
        public void Initialize()
        {
            _employee = new Employee
            {
                 Firstname = "Mattronilo",
                 Lastname = "Mendez",
                 MobilePhone = "09293235700",
                 EmailAddress = "mmendez@blastasia.com",
                 OfficePhone = "1234567",
                 Extension = "02",
                 Photo = new MemoryStream()
            };

            _connectionString = @"Server=(localdb)\MSSQLLocalDB;Database=DigiBookDb;Trusted_Connection=True;";
            _dbOptions = new DbContextOptionsBuilder<DigiBookDbContext>()
                .UseSqlServer(_connectionString)
                .Options;

            _dbContext = new DigiBookDbContext(_dbOptions);

            _sut = new EmployeeRepository(_dbContext);
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
        public void Create_WithValidEmployeeData_SaveRecordInTheDatabase()
        {
            // Arrange

            // Act
            var newEmployee = _sut.Create(_employee);

            //Assert
            Assert.IsNotNull(newEmployee);
            Assert.IsTrue(newEmployee.Id != Guid.Empty);


            // Cleanup
            _sut.Delete(newEmployee.Id);
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Delete_WithExistingEmployee_RemovesDataFromDatabase()
        {
            // Arrange
            var newEmployee = _sut.Create(_employee);
            // Act
            _sut.Delete(newEmployee.Id);

            // Assert
            _employee = _sut.Retrieve(newEmployee.Id);
            Assert.IsNull(_employee);

        }

        [TestMethod]
        public void Retrieve_WithExistingEmployeeId_ReturnsRecordFromDb()
        {
            // Arrange
            var data = _sut.Create(_employee);

            // Act
            var found = _sut.Retrieve(data.Id);

            // Assert
            Assert.IsNotNull(found);
        }

        [TestMethod]
        public void Retrieve_WithoutExistingEmployeeId_ReturnsNoRecordFromDb()
        {

        }


    }
}
