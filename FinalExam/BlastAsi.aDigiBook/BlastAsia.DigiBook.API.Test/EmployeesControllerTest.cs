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
        private Employee employee;
        private Mock<IEmployeeService> mockEmployeeService;
        private Mock<IEmployeeRepository> mockEmployeeRepository;
        private EmployeesController sut;
        private readonly Guid existingEmployeeId = Guid.NewGuid();
        private readonly Guid nonExistingEmployeeId = Guid.Empty;
        private JsonPatchDocument patchedEmployee;

        [TestInitialize]
        public void Initialize()
        {
            employee = new Employee
            {
                EmployeeId = existingEmployeeId,
                FirstName = "Jasmin",
                LastName = "Magdaleno",
                MobilePhone = "09057002880",
                EmailAddress = "jasminmagdaleno@blastasia.com",
                Photo = new MemoryStream(),
                OfficePhone = "5551212",
                Extension = "1"
            };

            mockEmployeeService = new Mock<IEmployeeService>();
            mockEmployeeRepository = new Mock<IEmployeeRepository>();
            sut = new EmployeesController(mockEmployeeService.Object, mockEmployeeRepository.Object);

            patchedEmployee = new JsonPatchDocument();
            patchedEmployee.Replace("FirstName", "Grace");

            mockEmployeeService
                .Setup(c => c.Save(existingEmployeeId, employee))
                .Returns(employee);

            mockEmployeeRepository
               .Setup(c => c.Retrieve(existingEmployeeId))
               .Returns(employee);
        }

        [TestCleanup]
        public void CleanUp()
        {

        }

        [TestMethod]
        public void GetEmployees_WithEmptyEmployeeId_ReturnsOkObjectResult()
        {
            //Arrange
            mockEmployeeRepository
                .Setup(c => c.Retrieve())
                .Returns(() => new List<Employee>{
                       new Employee()
                       });
            //Act
            var result = sut.GetEmployees(null);

            //Assert
            mockEmployeeRepository.Verify(c => c.Retrieve(), Times.Once());

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void GetEmployees_WithExistingEmployeeId_ReturnsOkObjectResult()
        {
            //Arrange

            //Act
            var result = sut.GetEmployees(employee.EmployeeId);

            //Assert
            mockEmployeeRepository.Verify(c => c.Retrieve(employee.EmployeeId), Times.Once());

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void CreateEmployee_WithValidContactData_ReturnsCreatedAtActionResult()
        {
            //Arrange
            employee.EmployeeId = nonExistingEmployeeId;

            mockEmployeeService
                .Setup(c => c.Save(employee.EmployeeId, employee))
                .Returns(employee);

            //Act
            var result = sut.CreateEmployee(employee);

            //Assert
            mockEmployeeService.Verify(c => c.Save(employee.EmployeeId, employee), Times.Once());
            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
        }

        [TestMethod]
        public void CreateEmployee_WithInvalidContactData_ReturnsBadRequestObjectResult()
        {
            //Arrange
            employee.EmailAddress = "jasminmagdalenoblastasiacom";
            employee.EmployeeId = nonExistingEmployeeId;

            mockEmployeeService
                .Setup(c => c.Save(employee.EmployeeId, employee))
                .Throws(new EmailAddressRequiredException(""));

            //Act
            var result = sut.CreateEmployee(employee);

            //Assert
            mockEmployeeService.Verify(c => c.Save(employee.EmployeeId, employee), Times.Once());
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public void CreateEmployee_WithEmptyContactData_ReturnsBadRequestResult()
        {
            //Arrange
            employee = null;

            //Act
            var result = sut.CreateEmployee(employee);

            //Assert
            mockEmployeeService.Verify(c => c.Save(Guid.NewGuid(), employee), Times.Never);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void DeleteEmployee_WithExistingEmployeeId_ReturnsNoContentResult()
        {
            //Arrange

            //Act
            var result = sut.DeleteEmployee(employee.EmployeeId);

            //Assert
            mockEmployeeRepository.Verify(c => c.Retrieve(employee.EmployeeId), Times.Once);
            mockEmployeeRepository.Verify(c => c.Delete(employee.EmployeeId), Times.Once);
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }

        [TestMethod]
        public void DeleteEmployee_WithNonExistingEmployeeId_ReturnsNotFoundResult()
        {
            //Arrange
            employee.EmployeeId = nonExistingEmployeeId;

            //Act
            var result = sut.DeleteEmployee(employee.EmployeeId);

            //Assert
            mockEmployeeRepository.Verify(c => c.Retrieve(employee.EmployeeId), Times.Once);
            mockEmployeeRepository.Verify(c => c.Delete(employee.EmployeeId), Times.Never);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void UpdateEmployee_WithExistingEmployeeIdAndData_ReturnsOkObjectResult()
        {
            //Arrange

            //Act
            var result = sut.UpdateEmployee(employee, employee.EmployeeId);

            //Assert
            mockEmployeeRepository.Verify(c => c.Retrieve(employee.EmployeeId), Times.Once);
            mockEmployeeService.Verify(c => c.Save(employee.EmployeeId, employee), Times.Once);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void UpdateEmployee_WithEmptyContactData_ReturnsBadRequestResult()
        {
            //Arrange
            employee = null;

            //Act
            var result = sut.UpdateEmployee(employee, nonExistingEmployeeId);

            //Assert
            mockEmployeeRepository.Verify(c => c.Retrieve(nonExistingEmployeeId), Times.Never);
            mockEmployeeService.Verify(c => c.Save(nonExistingEmployeeId, employee), Times.Never);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void UpdateEmployee_WithInvalidContactData_ReturnsBadRequestObjectResult()
        {
            //Arrange
            employee.EmailAddress = "jasminmagdalenoblastasiacom";

            mockEmployeeService
                .Setup(c => c.Save(employee.EmployeeId, employee))
                .Throws(new EmailAddressRequiredException(""));

            //Act
            var result = sut.UpdateEmployee(employee, employee.EmployeeId);

            //Assert
            mockEmployeeRepository.Verify(c => c.Retrieve(employee.EmployeeId), Times.Once);
            mockEmployeeService.Verify(c => c.Save(employee.EmployeeId, employee), Times.Once);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public void UpdateEmployee_WithNonExistingId_ReturnsNotFound()
        {
            //Arrange
            employee.EmployeeId = nonExistingEmployeeId;

            //Act
            var result = sut.UpdateEmployee(employee, employee.EmployeeId);

            //Assert
            mockEmployeeRepository.Verify(c => c.Retrieve(employee.EmployeeId), Times.Once);
            mockEmployeeService.Verify(c => c.Save(employee.EmployeeId, employee), Times.Never);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void PatchEmployee_WithExistingIdAndValidData_ReturnOkObjectResult()
        {
            //Arrange

            //Act
            var result = sut.PatchEmployee(patchedEmployee, employee.EmployeeId);

            //Assert
            mockEmployeeRepository.Verify(c => c.Retrieve(employee.EmployeeId), Times.Once);
            mockEmployeeService.Verify(c => c.Save(employee.EmployeeId, employee), Times.Once);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void PatchEmployee_WithEmptyContactData_ReturnsBadRequestResult()
        {
            //Arrange
            patchedEmployee = null;

            //Act
            var result = sut.PatchEmployee(patchedEmployee, nonExistingEmployeeId);

            //Assert
            mockEmployeeRepository.Verify(c => c.Retrieve(nonExistingEmployeeId), Times.Never);
            mockEmployeeService.Verify(c => c.Save(nonExistingEmployeeId, employee), Times.Never);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void PatchEmployee_WithInvalidContactData_ReturnsBadRequestObjectResult()
        {
            //Arrange
            patchedEmployee.Replace("EmailAddress", "jasminmagdalenoblastasiacom");

            mockEmployeeService
                .Setup(c => c.Save(employee.EmployeeId, employee))
                .Throws(new EmailAddressRequiredException(""));

            //Act
            var result = sut.PatchEmployee(patchedEmployee, employee.EmployeeId);

            //Assert
            mockEmployeeRepository.Verify(c => c.Retrieve(employee.EmployeeId), Times.Once);
            mockEmployeeService.Verify(c => c.Save(employee.EmployeeId, employee), Times.Once);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public void PatchEmployee_WithNonExistingId_ReturnsNotFound()
        {
            //Arrange
            employee.EmployeeId = nonExistingEmployeeId;

            //Act
            var result = sut.PatchEmployee(patchedEmployee, employee.EmployeeId);

            //Assert
            mockEmployeeRepository.Verify(c => c.Retrieve(employee.EmployeeId), Times.Once);
            mockEmployeeService.Verify(c => c.Save(employee.EmployeeId, employee), Times.Never);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }
    }
}
