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
        private Mock<IEmployeeRepository> mockEmployeeRepository;
        private Mock<IEmployeeService> mockEmployeeService;
        private EmployeesController sut;
        private JsonPatchDocument patchedEmployee;

        [TestInitialize]
        public void Initialize()
        {
            mockEmployeeRepository = new Mock<IEmployeeRepository>();
            mockEmployeeService = new Mock<IEmployeeService>();
            sut = new EmployeesController(mockEmployeeService.Object, mockEmployeeRepository.Object);
            employee = new Employee {
                EmployeeId = Guid.NewGuid(),
                FirstName = "Chris",
                LastName = "Manuel",
                MobilePhone = "09156879240",
                EmailAddress = "cmanuel@blastasia.com",
                Photo = new MemoryStream(),
                OfficePhone = "758-5224",
                Extension = "02"
            };

            patchedEmployee = new JsonPatchDocument();

            mockEmployeeRepository
                .Setup(x => x.Retrieve())
                .Returns(() => new List<Employee>
                {
                    new Employee()
                });

            mockEmployeeRepository
                .Setup(x => x.Retrieve(employee.EmployeeId))
                .Returns(employee);
        }

        [TestCleanup]
        public void Cleanup()
        {

        }

        [TestMethod]
        public void GetEmployees_WithEmptyEmployeeId_ReturnsOkObjectResult()
        {
            //Arrange

            //Act
            var result = sut.GetEmployees(null);

            //Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            mockEmployeeRepository.Verify(x => x.Retrieve(), Times.Once);
        }

        [TestMethod]
        public void GetEmployees_WithEmployeeId_ReturnsOkObjectResult()
        {
            //Arrange

            //Act
            var result = sut.GetEmployees(employee.EmployeeId);

            //Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            mockEmployeeRepository.Verify(x => x.Retrieve(employee.EmployeeId), Times.Once);
        }

        [TestMethod]
        public void CreateEmployee_WithEmptyEmployee_ReturnsBadRequestResult()
        {
            //Arrange
            employee = null;

            //Act
            var result = sut.CreateEmployee(employee);

            //Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            mockEmployeeService.Verify(x => x.Save(Guid.Empty, employee), Times.Never);
        }

        [TestMethod]
        public void CreateEmployee_WithEmployee_ReturnsCreatedAtActionResult()
        {
            //Arrange

            //Act
            var result = sut.CreateEmployee(employee);

            //Assert
            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
            mockEmployeeService.Verify(x => x.Save(Guid.Empty, employee), Times.Once);
        }

        [TestMethod]
        public void CreateEmployee_WithFieldThatThrowsException_ReturnsBadRequestResult()
        {
            //Arrange
            employee.LastName = "";

            mockEmployeeService
                .Setup(x => x.Save(Guid.Empty, employee))
                .Throws(new Exception());

            //Act
            var result = sut.CreateEmployee(employee);

            //Assert
            mockEmployeeService.Verify(x => x.Save(Guid.Empty, employee), Times.Once);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void DeleteEmployee_WithNonExistingEmployeeId_ReturnsNotFoundResult()
        {
            //Arrange
            employee.EmployeeId = Guid.Empty;

            //Act
            var result = sut.DeleteEmployee(employee.EmployeeId);

            //Assert
            mockEmployeeRepository.Verify(x => x.Delete(employee.EmployeeId), Times.Never);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void DeleteEmployee_WithExistingEmployeeId_ReturnsNoContentResult()
        {
            //Arrange


            //Act
            var result = sut.DeleteEmployee(employee.EmployeeId);

            //Assert
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
            mockEmployeeRepository.Verify(x => x.Delete(employee.EmployeeId), Times.Once);

        }

        [TestMethod]
        public void UpdateEmployee_WithEmptyEmployee_ReturnsBadRequestResult()
        {
            //Arrange
            employee = null;

            //Act
            var result = sut.UpdateEmployee(employee, Guid.Empty);

            //Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            mockEmployeeRepository.Verify(x => x.Retrieve(Guid.NewGuid()), Times.Never);
            mockEmployeeService.Verify(x => x.Save(Guid.NewGuid(), employee), Times.Never);
        }

        [TestMethod]
        public void UpdateEmployee_WithEmployeeButNonExistingEmployeeId_ReturnsNotFoundResult()
        {
            //Arrange
            employee.EmployeeId = Guid.Empty;

            //Act
            var result = sut.UpdateEmployee(employee, employee.EmployeeId);

            //Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            mockEmployeeRepository.Verify(x => x.Retrieve(employee.EmployeeId), Times.Once);
            mockEmployeeService.Verify(x => x.Save(Guid.NewGuid(), employee), Times.Never);
        }

        [TestMethod]
        public void UpdateEmployee_WithEmployeeAndExistingEmployeeId_ReturnsOkObjectResult()
        {
            //Arrange


            //Act
            var result = sut.UpdateEmployee(employee, employee.EmployeeId);

            //Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            mockEmployeeRepository.Verify(x => x.Retrieve(employee.EmployeeId), Times.Once);
            mockEmployeeService.Verify(x => x.Save(employee.EmployeeId, employee), Times.Once);
        }

        [TestMethod]
        public void UpdateEmployee_WithFieldThatThrowsException_ReturnsBadRequestResult()
        {
            //Arrange
            employee.MobilePhone = "";

            mockEmployeeService
                .Setup(x => x.Save(employee.EmployeeId, employee))
                .Throws(new Exception());


            //Act
            var result = sut.UpdateEmployee(employee, employee.EmployeeId);

            //Assert
            mockEmployeeService.Verify(x => x.Save(employee.EmployeeId, employee), Times.Once);
            mockEmployeeRepository.Verify(x => x.Retrieve(employee.EmployeeId), Times.Once);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }


        [TestMethod]
        public void PatchEmployee_WithEmptyPacthedEmployee_ReturnsBadRequestResult()
        {
            //Arrange
            patchedEmployee = null;

            //Act
            var result = sut.PatchEmployee(patchedEmployee, Guid.Empty);

            //Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            mockEmployeeRepository.Verify(x => x.Retrieve(Guid.NewGuid()), Times.Never);
            mockEmployeeService.Verify(x => x.Save(Guid.NewGuid(), employee), Times.Never);
        }

        [TestMethod]
        public void PatchEmployee_WithPatchedEmployeeButNonExistingEmployeeId_ReturnsNotFoundResult()
        {
            //Arrange

            //Act
            var result = sut.PatchEmployee(patchedEmployee, Guid.Empty);

            //Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            mockEmployeeRepository.Verify(x => x.Retrieve(Guid.Empty), Times.Once);
            mockEmployeeService.Verify(x => x.Save(Guid.NewGuid(), employee), Times.Never);
        }

        [TestMethod]
        public void PatchEmployee_WithPatchedEmployeeAndExistingEmployeeId_ReturnsOkObjectResult()
        {
            //Arrange


            //Act
            var result = sut.PatchEmployee(patchedEmployee, employee.EmployeeId);

            //Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            mockEmployeeRepository.Verify(x => x.Retrieve(employee.EmployeeId), Times.Once);
            mockEmployeeService.Verify(x => x.Save(employee.EmployeeId, employee), Times.Once);
        }

        [TestMethod]
        public void PatchEmployee_WithFieldThatThrowsException_ReturnsBadRequestResult()
        {
            //Arrange
            patchedEmployee.Replace("EmailAddress", "");

            mockEmployeeService
                .Setup(x => x.Save(employee.EmployeeId, employee))
                .Throws(new Exception());


            //Act
            var result = sut.PatchEmployee(patchedEmployee, employee.EmployeeId);

            //Assert
            mockEmployeeService.Verify(x => x.Save(employee.EmployeeId, employee), Times.Once);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }
    }
}

