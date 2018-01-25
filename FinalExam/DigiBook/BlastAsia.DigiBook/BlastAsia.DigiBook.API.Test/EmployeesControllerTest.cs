using BlastAsia.DigiBook.API.Controllers;
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

namespace BlastAsia.DigiBook.API.Test
{
    [TestClass]
    public class EmployeesControllerTest
    {
        private Mock<IEmployeeService> mockEmployeeService;
        private Mock<IEmployeeRepository> mockEmployeeRepository;
        private EmployeesController sut;
        private Employee employee;
        private JsonPatchDocument patchedEmployee;

        [TestInitialize]
        public void Initialize()
        {
            mockEmployeeService = new Mock<IEmployeeService>();
            mockEmployeeRepository = new Mock<IEmployeeRepository>();
            sut = new EmployeesController(mockEmployeeService.Object, mockEmployeeRepository.Object);
            employee = new Employee {
                EmployeeId = Guid.NewGuid(),
                FirstName = "Angela Blanche",
                LastName = "Olarte",
                MobilePhone = "09981642039",
                EmailAddress = "abbieolarte@gmail.com",
                Photo = new MemoryStream(),
                OfficePhone = "555222",
                Extension = "105"
            };

            patchedEmployee = new JsonPatchDocument();
            patchedEmployee.Replace("FirstName", "Abbie");

            mockEmployeeRepository
               .Setup(e => e.Retrieve(employee.EmployeeId))
               .Returns(employee);
        }

        [TestCleanup]
        public void CleanUp()
        {

        }

        [TestMethod]
        public void GetEmployees_WithEmptyEmployeeId_ReturnsOkObjectResult()
        {
            // Arrange
            mockEmployeeRepository
                .Setup(e => e.Retrieve())
                .Returns(new List<Employee>());

            // Act
            var result = sut.GetEmployees(null);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            mockEmployeeRepository.Verify(e => e.Retrieve(), Times.Once);
        }

        [TestMethod]
        public void GetEmployees_WithEmployeeId_ReturnsOkObjectResult()
        {
            // Arrange
            var id = Guid.NewGuid();
            mockEmployeeRepository
                .Setup(e => e.Retrieve(Guid.NewGuid()))
                .Returns(employee);

            // Act
            var result = sut.GetEmployees(id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            mockEmployeeRepository.Verify(e => e.Retrieve(id), Times.Once);
        }

        [TestMethod]
        public void CreateEmployee_EmployeeWithValidData_ReturnCreatedAtActionResult()
        {
            // Act
            var result = sut.CreateEmployee(employee);

            // Assert
            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
            mockEmployeeService.Verify(e => e.Save(Guid.Empty, employee), Times.Once);
        }

        [TestMethod]
        public void CreateEmployee_EmployeeWithEmptyData_ReturnBadRequestResult()
        {
            // Arrange
            employee = null;

            // Act
            var result = sut.CreateEmployee(employee);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            mockEmployeeService.Verify(e => e.Save(Guid.Empty, employee), Times.Never);
        }

        [TestMethod]
        public void DeleteEmployee_EmployeeDeleted_ReturnNoContentResult()
        {
            // Act
            var result = sut.DeleteEmployee(employee.EmployeeId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
            mockEmployeeRepository.Verify(e => e.Delete(employee.EmployeeId), Times.Once);
        }

        [TestMethod]
        public void DeleteEmployee_WithoutEmployeeId_ReturnNotFoundResult()
        {
            // Arrange
            mockEmployeeRepository.
                Setup(e => e.Retrieve(employee.EmployeeId))
                .Returns<Employee>(null);

            // Act
            var result = sut.DeleteEmployee(employee.EmployeeId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            mockEmployeeRepository.Verify(e => e.Delete(employee.EmployeeId), Times.Never);
        }

        [TestMethod]
        public void UpdateEmployee_WithExistingEmployeeDataAndId_ReturnOkObjectResult()
        {
            // Act
            var result = sut.UpdateEmployee(employee, employee.EmployeeId);

            // Assert
            mockEmployeeRepository.Verify(e => e.Retrieve(employee.EmployeeId), Times.Once);
            mockEmployeeService.Verify(e => e.Save(employee.EmployeeId, employee), Times.Once);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void UpdateEmployee_EmployeeWithoutValue_ReturnBadRequestResult()
        {
            // Arrange
            employee = null;

            // Act
            var result = sut.UpdateEmployee(employee, Guid.Empty);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            mockEmployeeRepository.Verify(e => e.Retrieve(Guid.Empty), Times.Never);
            mockEmployeeService.Verify(e => e.Save(Guid.Empty, employee), Times.Never);
        }

        [TestMethod]
        public void UpdateEmployee_WithNoExistingEmployee_ReturnNotFoundResult()
        {
            // Arrange
            employee.EmployeeId= Guid.Empty;

            // Act
            var result = sut.UpdateEmployee(employee, employee.EmployeeId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            mockEmployeeRepository.Verify(e => e.Retrieve(employee.EmployeeId), Times.Once);
            mockEmployeeService.Verify(e => e.Save(employee.EmployeeId, employee), Times.Never);
        }

        [TestMethod]
        public void PatchEmployee_WithExistingEmployeeDataAndId_ReturnOkObjectResult()
        {
            // Act
            var result = sut.PatchEmployee(patchedEmployee, employee.EmployeeId);

            // Assert
            mockEmployeeService.Verify(e => e.Save(employee.EmployeeId, employee), Times.Once);
            mockEmployeeRepository.Verify(e => e.Retrieve(employee.EmployeeId), Times.Once);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void PatchEmployee_WithoutExistingEmployeeDataAndId_ReturnNotFoundResult()
        {
            // Arrange
            mockEmployeeRepository
              .Setup(e => e.Retrieve(employee.EmployeeId))
              .Returns<Employee>(null);

            // Act
            var result = sut.PatchEmployee(patchedEmployee, employee.EmployeeId);

            // Assert
            mockEmployeeRepository.Verify(e => e.Retrieve(employee.EmployeeId), Times.Once);
            mockEmployeeService.Verify(e => e.Save(employee.EmployeeId, employee), Times.Never);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void PatchEmployee_EmployeeWithEmptyPatchDocument_ReturnBadRequestResult()
        {
            // Arrange
            patchedEmployee = null;

            // Act
            var result = sut.PatchEmployee(patchedEmployee, employee.EmployeeId);

            // Assert
            mockEmployeeService.Verify(e => e.Save(employee.EmployeeId, employee), Times.Never);
            mockEmployeeService.Verify(e => e.Save(employee.EmployeeId, employee), Times.Never);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }
    }
}
