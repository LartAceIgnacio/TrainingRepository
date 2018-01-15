using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using Microsoft.Data.Tools.Schema.Sql.UnitTesting;
using Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.JsonPatch;
using BlastAsia.DigiBook.Domain.Employees;
using BlastAsia.DigiBook.Domain.Models.Employees;
using Moq;
using BlastAsia.DigiBook.API.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace BlastAsia.DigiBook.API.Test
{
    [TestClass]
    public class EmployeesControllerTest
    {
        private Employee employee;
        private Mock<IEmployeeRepository> mockEmployeeRepository;
        private Mock<IEmployeeService> mockEmployeeService;
        private JsonPatchDocument patchedEmployee;

        private EmployeesController sut;

        private Guid existingEmployeeId = Guid.NewGuid();
        private Guid notExistingEmployeeId = Guid.Empty;

        [TestInitialize]
        public void TestInitialize()
        {
            mockEmployeeRepository = new Mock<IEmployeeRepository>();
            mockEmployeeService = new Mock<IEmployeeService>();
            patchedEmployee = new JsonPatchDocument();

            employee = new Employee
            {

                EmployeeId = Guid.NewGuid()
            };

            sut = new EmployeesController(mockEmployeeService.Object,
               mockEmployeeRepository.Object);

            mockEmployeeRepository
           .Setup(x => x.Retrieve())
           .Returns(() => new List<Employee>{
               new Employee()});

            mockEmployeeRepository
            .Setup(x => x.Retrieve(employee.EmployeeId))
            .Returns(employee);

            mockEmployeeRepository
                .Setup(cr => cr.Retrieve(existingEmployeeId))
                .Returns(employee);

            //Setup for Update
            mockEmployeeRepository
            .Setup(cr => cr.Retrieve(notExistingEmployeeId))
            .Returns<Employee>(null);

            //Setup for Delete
            mockEmployeeRepository
                .Setup(cr => cr.Retrieve(existingEmployeeId))
                .Returns(employee);
        }
        [TestCleanup()]
        public void TestCleanup()
        {

        }
        [TestMethod]
        public void GetEmployees_WithEmptyEmployeeId_ReturnsOkObjectResult()
        {
            // Act
            var result = sut.GetEmployee(null);

            // Assert              
            mockEmployeeRepository
               .Verify(c => c.Retrieve(), Times.Once());

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void GetEmployee_WithEmployeeId_ReturnsObjectResult()
        {
            // Act
            var result = sut.GetEmployee(notExistingEmployeeId);

            // Assert
            mockEmployeeRepository
               .Verify(c => c.Retrieve(notExistingEmployeeId), Times.Once());

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void CreateEmployee_WithEmptyEmployee_ReturnsBadRequestResult()
        {
            employee = null;
            var result = sut.CreateEmployee(employee);

            // Assert
            mockEmployeeService
               .Verify(c => c.Save(Guid.Empty, employee), Times.Never());

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void CreateEmployee_WithValidEmployee_ReturnsObjectResult()
        {

            mockEmployeeService
              .Setup(cs => cs.Save(Guid.Empty, employee))
              .Returns(employee);

            //Act
            var result = sut.CreateEmployee(employee);

            // Assert         
            mockEmployeeService
             .Verify(c => c.Save(Guid.Empty, employee), Times.Once());

            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
        }

        [TestMethod]
        public void UpdateEmployee_WithValidEmployee_ReturnsObjectResult()
        {

            var result = sut.UpdateEmployee(employee, existingEmployeeId);
            // Assert

            mockEmployeeRepository
                .Verify(c => c.Retrieve(existingEmployeeId), Times.Once());

            mockEmployeeService
                .Verify(c => c.Save(existingEmployeeId, employee), Times.Once());

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void UpdateEmployee_WithEmptyEmployee_ReturnsBadRequestResult()
        {
            employee = null;
            // Act
            var result = sut.UpdateEmployee(employee, existingEmployeeId);

            // Assert
            mockEmployeeRepository
                .Verify(c => c.Update(existingEmployeeId, employee), Times.Never());

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }
        [TestMethod]
        public void UpdateEmployee_WithEmptyEmployeeId_ReturnsNotFoundResult()
        {
            var result = sut.UpdateEmployee(employee, notExistingEmployeeId);

            // Assert
            mockEmployeeRepository
                .Verify(c => c.Update(notExistingEmployeeId, employee), Times.Never());

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }
        [TestMethod]
        public void DeleteEmployee_WithEmployeeId_ReturnsNoContentResult()
        {
            // Act
            var result = sut.DeleteEmployee(existingEmployeeId);

            //Assert          
            mockEmployeeRepository
                .Verify(c => c.Delete(existingEmployeeId), Times.Once());

            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }

        [TestMethod]
        public void DeleteEmployee_WithEmptyEmployeeId_ReturnsNotFound()
        {
            //Act
            var result = sut.DeleteEmployee(notExistingEmployeeId);

            // Assert 
            mockEmployeeRepository
                .Verify(c => c.Delete(notExistingEmployeeId),
                Times.Never());

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void PatchEmployee_WithValidPatchedEmployee_ReturnsObjectResult()
        {
            var result = sut.PatchEmployee(patchedEmployee, existingEmployeeId);
            // Assert

            mockEmployeeRepository
                .Verify(c => c.Retrieve(existingEmployeeId), Times.Once());

            mockEmployeeService
                .Verify(c => c.Save(existingEmployeeId, employee), Times.Once());

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

        }
        [TestMethod]
        public void PatchEmployee_WithEmptyPatchedEmployee_ReturnsBadRequestResult()
        {
            patchedEmployee = null;
            // Act
            var result = sut.PatchEmployee(patchedEmployee, existingEmployeeId);

            // Assert
            mockEmployeeRepository
               .Verify(c => c.Retrieve(notExistingEmployeeId), Times.Never());

            mockEmployeeService
                .Verify(c => c.Save(notExistingEmployeeId, employee), Times.Never());

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void PatchEmployee_WithInvalidEmployeeId_ReturnsNotFound()
        {
            var result = sut.PatchEmployee(patchedEmployee, notExistingEmployeeId);

            //Assert
            mockEmployeeRepository
                .Verify(c => c.Retrieve(notExistingEmployeeId), Times.Once());

            mockEmployeeService
                 .Verify(c => c.Save(notExistingEmployeeId, employee), Times.Never());

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }


    }
}

