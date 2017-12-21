using BlastAsia.DigiBook.API.Controllers;
using BlastAsia.DigiBook.Domain.Employees;
using BlastAsia.DigiBook.Domain.Employees.Services;
using BlastAsia.DigiBook.Domain.Models.Employees;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.API.Test
{
    [TestClass]
    public class EmployeeControllerTest
    {

        private Mock<IEmployeeService> _mockEmployeeService;
        private Mock<IEmployeeRepository> _mockEmployeeRepository;
        private EmployeeController _sut;
        private Employee employee;

        [TestInitialize]
        public void Initialize()
        {
            _mockEmployeeService = new Mock<IEmployeeService>();
            _mockEmployeeRepository = new Mock<IEmployeeRepository>();
            _sut = new EmployeeController(_mockEmployeeRepository.Object, _mockEmployeeService.Object);

            employee = new Employee()
            {
                Id = Guid.NewGuid(),
                Firstname = "Matt",
                Lastname = "Mendez",
                MobilePhone = "09293235700",
                Extension = "02",
                OfficePhone = "1234567",
                EmailAddress = "mmendez@blastasia.com"
            };

            _mockEmployeeRepository.Setup(c => c.Retrieve(employee.Id))
                .Returns(employee);

            _mockEmployeeRepository.Setup(x => x.Retrieve())
                .Returns(() => new List<Employee>
                {
                    new Employee()
                });


            //_mockEmployeeRepository.Setup(c => c.Create(null))
            //    .Returns<Employee>(null);

        }

        [TestCleanup]
        public void Cleanup()
        {

        }

        [TestMethod]
        [TestProperty("API Test", "EmployeeController")]
        public void GetEmployee_NoRetrieveParameterGetAllEmployees_ReturnsOkResult()
        {

            // Act
            var result = _sut.GetEmployee(null);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            _mockEmployeeRepository.Verify(c => c.Retrieve(), Times.Once);
        }

        [TestMethod]
        [TestProperty("API Test", "EmployeeController")]
        public void GetEmployee_WithValidId_ReturnsOkResult()
        {

            var result = _sut.GetEmployee(employee.Id);

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            _mockEmployeeRepository.Verify(c => c.Retrieve(employee.Id), Times.Once);
        }

        [TestMethod]
        [TestProperty("API Test", "EmployeeController")]
        public void PostEmployee_WithNoEmployeeInformation_ReturnsBadRequest()
        {
            // Arrange

            // act
            var result = _sut.PostEmployee(null);

            //assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            _mockEmployeeService.Verify(service => service.Save(Guid.Empty, employee), Times.Never);
        }

        [TestMethod]
        [TestProperty("API Test", "EmployeeController")]
        public void PostEmployee_WithValidInformation_ReturnsCreateActionResult()
        {
            // Act
            var result = _sut.PostEmployee(employee);

            // Assert
            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
            _mockEmployeeService.Verify(service => service.Save(Guid.Empty, employee));//Empty Guid coz' we're creating new data.

        }

        [TestMethod]
        [TestProperty("API Test", "EmployeeController")]
        public void DeleteEmployee_WithValidId_ReturnsNoContentResult()
        {
            // Act

            var result = _sut.DeleteEmployee(employee.Id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
            _mockEmployeeRepository.Verify(repo => repo.Delete(employee.Id), Times.Once);
        }

        [TestMethod]
        [TestProperty("API Test", "EmployeeController")]
        public void DeleteEmployee_WithInvalidId_ReturnsBadRequest()
        {
            // Act

            var result = _sut.DeleteEmployee(Guid.Empty);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            _mockEmployeeRepository.Verify(repo => repo.Delete(Guid.Empty), Times.Never);
        }

        [TestMethod]
        [TestProperty("API Test", "EmployeeController")]
        public void UpdateEmployee_WithValidData_ReturnsOkResult()
        {
            // Act
            var result = _sut.UpdateEmployee(employee, employee.Id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkResult));
            _mockEmployeeService.Verify(service => service.Save(employee.Id, employee), Times.Once);

        }

        [TestMethod]
        [TestProperty("API Test", "EmployeeController")]
        public void UpdateEmployee_WithInvalid_ReturnsBadRequest()
        {
            // Act
            employee.Firstname = null;

            _mockEmployeeService.Setup(service => service.Save(employee.Id, employee))
                .Throws<Exception>();

            // Act
            var result = _sut.UpdateEmployee(employee, employee.Id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            _mockEmployeeRepository.Verify(repo => repo.Retrieve(employee.Id), Times.Once);
            _mockEmployeeService.Verify(service => service.Save(employee.Id, employee), Times.Once);
        }

        [TestMethod]
        [TestProperty("API Test", "EmployeeController")]
        public void Patch_WithNullPatchEmployee_ReturnsBadRequest()
        {
            // Arrange

            // Act
            var result = _sut.PatchEmployee(null, employee.Id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            _mockEmployeeRepository.Verify(repo => repo.Retrieve(employee.Id), Times.Never);
            _mockEmployeeService.Verify(service => service.Save(employee.Id, employee), Times.Never);
        }

        [TestMethod]
        [TestProperty("API Test", "EmployeeController")]
        public void Patch_WithoutRetrievedEmployee_ReturnsNotFounnd()
        {
            // Arrange
            var patchedDoc = new JsonPatchDocument();
            var guid = Guid.Empty;

            _mockEmployeeService.Setup(service => service.Save(Guid.Empty, employee))
                .Returns<Employee>(null);

            // Act
            var result = _sut.PatchEmployee(patchedDoc, guid);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            _mockEmployeeRepository.Verify(repo => repo.Retrieve(guid), Times.Once);
            _mockEmployeeService.Verify(service => service.Save(guid, employee), Times.Never);
        }
    }
}
