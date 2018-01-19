using BlastAsia.DigiBook.Api.Controllers;
using BlastAsia.DigiBook.Domain.Employees;
using BlastAsia.DigiBook.Domain.Models.Employees;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Api.Test
{
    [TestClass]
    public class EmployeesControllerTest
    {
        private Mock<IEmployeeRepository> mockEmployeeRepository;
        private Mock<IEmployeeService> mockEmployeeService;
        private EmployeesController sut;
        private Employee employee;
        private Object result;
        private JsonPatchDocument patchedEmployee;
        private Guid existingEmployeeId;
        private Guid nonExistingEmployeeId;

        [TestInitialize]
        public void Initialize()
        {
            mockEmployeeRepository = new Mock<IEmployeeRepository>();
            mockEmployeeService = new Mock<IEmployeeService>();
            sut = new EmployeesController(mockEmployeeRepository.Object, mockEmployeeService.Object);

            employee = new Employee();
            patchedEmployee = new JsonPatchDocument();

            existingEmployeeId = Guid.NewGuid();
            nonExistingEmployeeId = Guid.Empty;

            mockEmployeeRepository
                .Setup(cr => cr.Retrieve(existingEmployeeId))
                .Returns(employee);

            mockEmployeeRepository
               .Setup(cr => cr.Retrieve(nonExistingEmployeeId))
               .Returns<Employee>(null);
        }

        [TestMethod]
        public void GetEmployees_WithEmptyEmployeeId_ReturnsOkObjectResult()
        {
            //Arrange
            //Act
            result = sut.GetEmployees(null);

            //Assert
            mockEmployeeRepository
                .Verify(c => c.Retrieve(), Times.Once);

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            //Assert.ReferenceEquals

        }

        [TestMethod]
        public void GetEmployees_WithExistingEmployeeId_ReturnsOkObjectResutl()
        {
            //Arrange
            var existingEmployeeId = Guid.NewGuid();
            //Act
            result = sut.GetEmployees(existingEmployeeId);
            //Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

            mockEmployeeRepository
                .Verify(c => c.Retrieve(existingEmployeeId), Times.Once);

        }

        [TestMethod]
        public void CreateEmployee_WithValidEmployeeData_ReturnsOkObjectResult()
        {
            //Arrange
            employee = new Employee();
            //Act
            result = sut.CreateEmployee(employee);
            //Assert
            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
            mockEmployeeService
                .Verify(c => c.Save(Guid.Empty, employee), Times.Once);
        }
        [TestMethod]
        public void CreateEmployee_WithNullEmployeeData_ReturnsBadRequestObjectResult()
        {
            //Arrange
            employee = null;
            //Act
            result = sut.CreateEmployee(employee);
            //Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            mockEmployeeService
                .Verify(c => c.Save(Guid.Empty, employee), Times.Never);
        }

        [TestMethod]
        public void DeleteEmployee_WithExistingEmployeeId_ReturnsOkResult()
        {
            //Arrange
            employee.EmployeeId = existingEmployeeId;
            //Setup EmployeeRepository

            //Act
            result = sut.DeleteEmployee(employee.EmployeeId);

            //Assert
            Assert.IsInstanceOfType(result, typeof(OkResult));
            mockEmployeeRepository
                .Verify(c => c.Delete(employee.EmployeeId), Times.Once);
        }

        [TestMethod]
        public void DeleteEmployee_WithNonExistingEmployeeId_ReturnsNotFoundResult()
        {
            //Arrange
            employee.EmployeeId = nonExistingEmployeeId;

            //Act
            result = sut.DeleteEmployee(nonExistingEmployeeId);

            //Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            mockEmployeeRepository
                .Verify(c => c.Delete(employee.EmployeeId), Times.Never);
        }

        [TestMethod]
        public void UpdateEmployee_WithExistingEmployeeId_ReturnsOkResult()
        {
            //Arrange
            employee.EmployeeId = existingEmployeeId;

            //Act
            result = sut.UpdateEmployee(employee, employee.EmployeeId);

            //Assert
            mockEmployeeService
                .Verify(cs => cs.Save(employee.EmployeeId, employee), Times.Once);
            mockEmployeeRepository
                .Verify(cr => cr.Retrieve(employee.EmployeeId), Times.Once);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void UpdateEmployee_WithNonExistingEmployeeId_ReturnsBadRequestObject()
        {
            //Arrange
            employee.EmployeeId = nonExistingEmployeeId;
           
            //Act
            result = sut.UpdateEmployee(employee, employee.EmployeeId);

            //Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            mockEmployeeRepository
                .Verify(cr => cr.Retrieve(employee.EmployeeId), Times.Once);
            mockEmployeeService
                .Verify(cs => cs.Save(employee.EmployeeId, employee), Times.Never);
        }

        [TestMethod]
        public void PatchEmployee_WithValidPatchEmployeeData_ReturnsOkObjectResult()
        {
            //Arrange
            employee.EmployeeId = existingEmployeeId;

            //Act
            result = sut.PatchEmployee(patchedEmployee, employee.EmployeeId);

            //Assert

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            mockEmployeeService
                .Verify(cs => cs.Save(employee.EmployeeId, employee), Times.Once);

        }

        [TestMethod]
        public void PatchEmployee_WithNotValidPatchEmployeeData_ReturnsBadRequestResult()
        {
            //Arrange
            patchedEmployee = null;
            //Act
            result = sut.PatchEmployee(patchedEmployee, employee.EmployeeId);

            //Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            mockEmployeeService
                .Verify(cs => cs.Save(employee.EmployeeId, employee), Times.Never);
        }

        [TestMethod]
        public void PatchEmployee_WithNonExistingEmployeeId_ReturnsNotFoundResult()
        {
            //Arrange
            employee.EmployeeId = nonExistingEmployeeId;

            //Act
            result = sut.PatchEmployee(patchedEmployee, employee.EmployeeId);

            //Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            mockEmployeeService
                .Verify(cs => cs.Save(employee.EmployeeId, employee), Times.Never);
        }
    
    }
}
