using BlastAsia.DigiBook.API.Controllers;
using BlastAsia.DigiBook.Domain.Employees;
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
    public class EmployeesControllerTest
    {
        private Mock<IEmployeeRepository> mockEmployeeRepo;
        private Mock<IEmployeeService> mockEmployeeService;
        private EmployeesController sut;
        private Employee employee;
        private Guid emptyEmployeeId;
        private Guid existingEmployeeId;
        private JsonPatchDocument patchedEmployee;


        [TestInitialize]
        public void InitializeTest()
        {
            mockEmployeeRepo = new Mock<IEmployeeRepository>();
            mockEmployeeService = new Mock<IEmployeeService>();
            sut = new EmployeesController(mockEmployeeService.Object, mockEmployeeRepo.Object);
            employee = new Employee();
            existingEmployeeId = Guid.NewGuid();
            emptyEmployeeId = Guid.Empty;
            patchedEmployee = new JsonPatchDocument();

            mockEmployeeRepo
                .Setup(cr => cr.Retrieve(existingEmployeeId))
                .Returns(employee);
            mockEmployeeRepo
                .Setup(cr => cr.Retrieve(emptyEmployeeId))
                .Returns<Employee>(null);
        }

        [TestCleanup]
        public void CleanupTest()
        {

        }

        [TestMethod]
        public void GetEmployees_WithEmptyEmployeeId_ReturnsOkObjectValue()
        {
            // Arrange
            mockEmployeeRepo
                .Setup(cr => cr.Retrieve())
                .Returns(new List<Employee>());

            // Act
            var result = sut.GetEmployees(emptyEmployeeId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void GetEmployees_WithExistingEmployeeId_ReturnsOkObjectValue()
        {
            // Act
            var result = sut.GetEmployees(existingEmployeeId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void CreateEmployee_WithEmptyEmployee_ReturnsBadRequest()
        {
            // Act
            var result = sut.CreateEmployee(null);

            // Assert
            mockEmployeeService
                .Verify(cs => cs.Save(Guid.Empty, employee), Times.Never());
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));

        }

        [TestMethod]
        public void CreateEmployee_WithValidEmployee_ReturnsNewEmployeeWithEmployeeId()
        {
            // Arrange
            mockEmployeeService
                .Setup(cs => cs.Save(Guid.Empty, employee))
                .Returns(employee);

            // Act
            var result = sut.CreateEmployee(employee);

            // Assert
            mockEmployeeService
                .Verify(cs => cs.Save(Guid.Empty, employee), Times.Once);
            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
        }

        [TestMethod]
        public void DeleteEmployee_WithEmptyEmployeeId_ReturnsNotFound()
        {
            // Act
            var result = sut.DeleteEmployee(emptyEmployeeId);

            // Assert
            mockEmployeeRepo
                .Verify(cr => cr.Delete(emptyEmployeeId), Times.Never);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void DeleteEmployee_WithExistingEmployeeId_ReturnsNoContent()
        {
            // Act
            var result = sut.DeleteEmployee(existingEmployeeId);

            // Assert
            mockEmployeeRepo
                .Verify(cr => cr.Delete(existingEmployeeId), Times.Once);
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }

        [TestMethod]
        public void UpdateEmployee_WithEmptyEmployee_ReturnsBadRequest()
        {
            // Act
            var result = sut.UpdateEmployee(null, existingEmployeeId);

            // Assert
            mockEmployeeService
                .Verify(cs => cs.Save(existingEmployeeId, null), Times.Never);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void UpdateEmployee_WithEmptyEmployeeId_ReturnsNotFound()
        {
            // Act
            var result = sut.UpdateEmployee(employee, emptyEmployeeId);

            // Assert
            mockEmployeeService
                .Verify(cs => cs.Save(emptyEmployeeId, employee), Times.Never);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void UpdateEmployee_WithExistingEmployeeIdAndEmployee_ReturnsOkObjectValue()
        {
            // Act
            var result = sut.UpdateEmployee(employee, existingEmployeeId);

            // Assert
            mockEmployeeService
                .Verify(cs => cs.Save(existingEmployeeId, employee), Times.Once);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void PatchEmployee_WithEmptyPatchedEmployee_ReturnsBadRequest()
        {
            // Arrange 
            patchedEmployee = null;

            // Act
            var result = sut.PatchEmployee(patchedEmployee, existingEmployeeId);

            // Assert
            mockEmployeeRepo
                .Verify(cr => cr.Retrieve(existingEmployeeId), Times.Never);
            mockEmployeeService
                .Verify(cs => cs.Save(existingEmployeeId, employee), Times.Never);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));

        }

        [TestMethod]
        public void PatchEmployee_WithEmptyEmployeeId_ReturnsNotFound()
        {
            // Act
            var result = sut.PatchEmployee(patchedEmployee, emptyEmployeeId);

            // Assert
            mockEmployeeService
                .Verify(cs => cs.Save(emptyEmployeeId, employee), Times.Never);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void PatchEmployee_WithExistingEmployeeId_ReturnsOkObjectValue()
        {
            // Act
            var result = sut.PatchEmployee(patchedEmployee, existingEmployeeId);

            // Assert
            mockEmployeeService
                .Verify(cs => cs.Save(existingEmployeeId, employee), Times.Once);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }
    }
}
