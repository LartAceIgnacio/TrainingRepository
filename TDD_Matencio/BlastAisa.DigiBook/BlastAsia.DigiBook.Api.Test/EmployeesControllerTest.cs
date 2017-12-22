using BlastAsia.DigiBook.Api.Controllers;
using BlastAsia.DigiBook.Domain.Employees;
using BlastAsia.DigiBook.Domain.Models.Employees;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BlastAsia.DigiBook.Api.Test
{
    [TestClass]
    public class EmployeesControllerTest
    {
        private Mock<IEmployeeRepository> mockEmployeeRepository;
        private Mock<IEmpolyeeService> mockEmployeeService;
        private EmployeesController sut;
        private Employee employee;
        private JsonPatchDocument patch;

        [TestInitialize]
        public void TestInitialize()
        {
            mockEmployeeRepository = new Mock<IEmployeeRepository>();
            mockEmployeeService = new Mock<IEmpolyeeService>();
            sut = new EmployeesController(mockEmployeeService.Object,
                mockEmployeeRepository.Object);

            patch = new JsonPatchDocument();

            employee = new Employee
            {
                employeeId = Guid.NewGuid(),
                firstName = "John Karl",
                lastName = "Matencio",
                mobilePhone = "09957206817",
                emailAddress = "jhnkrl15@gmail.com",
                officePhone = "4848766",
                extension = "1001"
            };

            mockEmployeeRepository
               .Setup(c => c.Retreive())
               .Returns(new List<Employee>());


            mockEmployeeRepository
                .Setup(c => c.Retrieve(employee.employeeId))
                .Returns(employee);

            mockEmployeeRepository
                .Setup(c => c.Create(employee))
                .Returns(employee);

            mockEmployeeRepository
                .Setup(e => e.Retrieve(Guid.Empty))
                .Returns<Employee>(null);

        }

        [TestCleanup]
        public void TestCleanup()
        {

        }

        [TestMethod]
        public void GetEmployees_WithEmptyEmployeeId_ReturnsOkObjectValue()
        {
            //Arrange
            //Act
            var result = sut.GetEmployees(null);
            //Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            mockEmployeeRepository
                .Verify(c => c.Retreive(), Times.Once);
        }

        [TestMethod]
        public void GetEmployees_WithExistingEmployeeId_ReturnsOkObjectValue()
        {
            //Arrange
            //Act
            var result = sut.GetEmployees(employee.employeeId);
            //Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            mockEmployeeRepository
                .Verify(c => c.Retrieve(employee.employeeId), Times.Once);
        }

        [TestMethod]
        public void DeleteEmployee_WithEmptyEmployeeId_ReturnsNotFound()
        {
            //Arrange
            employee.employeeId = Guid.Empty;
            //Act
            var result = sut.DeleteEmployee(employee.employeeId);
            //Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            mockEmployeeRepository
                .Verify(c => c.Delete(employee.employeeId), Times.Never);
        }

        [TestMethod]
        public void DeleteEmployee_WithExistingEmployeeId_ReturnsNoContent()
        {
            //Arrange
            //Act
            var result = sut.DeleteEmployee(employee.employeeId);
            //Assert
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
            mockEmployeeRepository
                .Verify(c => c.Delete(employee.employeeId), Times.Once);
        }

        [TestMethod]
        public void CreateEmployee_WithEmptyEmployee_ReturnsBadRequest()
        {
            //Arrange          
            employee = null;
            //Act
            var result = sut.CreateEmployee(employee);
            //Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            mockEmployeeService
                .Verify(c => c.Save(Guid.Empty, employee), Times.Never);
        }

        [TestMethod]
        public void CreateEmployee_WithExistingEmployee_ReturnsCreatedAtActionResult()
        {
            //Arrange
            //Act
            var result = sut.CreateEmployee(employee);
            //Assert
            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
            mockEmployeeService
                .Verify(c => c.Save(Guid.Empty, employee), Times.Once);
        }

        [TestMethod]
        public void UpdateEmployee_WithEmptyEmployee_ReturnsBadRequest()
        {
            //Arrange
            employee = null;
            //Act
            var result = sut.UpdateEmployee(employee, Guid.Empty);
            //Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            mockEmployeeService
                .Verify(c => c.Save(Guid.Empty, employee), Times.Never);
        }

        [TestMethod]
        public void UpdateEmployee_WithEmptyEmployeeId_ReturnsNotFound()
        {
            //Arrange
            employee.employeeId = Guid.Empty;
            //Act
            var result = sut.UpdateEmployee(employee, employee.employeeId);
            //Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            mockEmployeeRepository
                .Verify(c => c.Retrieve(employee.employeeId), Times.Once);
            mockEmployeeService
                .Verify(c => c.Save(Guid.NewGuid(), employee), Times.Never);
        }

        [TestMethod]
        public void UpdateEmployee_WithValidData_ReturnOkObjectResult()
        {
            //Arrange
            //Act
            var result = sut.UpdateEmployee(employee, employee.employeeId);
            //Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            mockEmployeeRepository
                .Verify(c => c.Retrieve(employee.employeeId), Times.Once);
            mockEmployeeService
                .Verify(c => c.Save(employee.employeeId, employee), Times.Once);
        }

        [TestMethod]
        public void PatchEmployee_WithEmptyPatchedEmployee_ReturnsBadRequest()
        {
            //Arrange
            patch = null;
            //Act
            var result = sut.PatchEmployee(patch, employee.employeeId);
            //Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            mockEmployeeService
                .Verify(c => c.Save(employee.employeeId, employee), Times.Never);
        }

        [TestMethod]
        public void PatchEmployee_WithEmptyEmployeeId_ReturnsNotFound()
        {
            //Arrange
            employee.employeeId = Guid.Empty;
            //Act
            var result = sut.PatchEmployee(patch, employee.employeeId);
            //Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            mockEmployeeRepository
                .Verify(c => c.Retrieve(employee.employeeId), Times.Once);
            mockEmployeeService
                .Verify(c => c.Save(employee.employeeId, employee), Times.Never);
        }

        [TestMethod]
        public void PatchEmployee_WithValidData_ReturnsOkObjectValue()
        {
            //Arrange
            //Act
            var result = sut.PatchEmployee(patch, employee.employeeId);
            //Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            mockEmployeeRepository
                .Verify(c => c.Retrieve(employee.employeeId), Times.Once);
            mockEmployeeService
                .Verify(c => c.Save(employee.employeeId, employee), Times.Once);
        }
    }
}
