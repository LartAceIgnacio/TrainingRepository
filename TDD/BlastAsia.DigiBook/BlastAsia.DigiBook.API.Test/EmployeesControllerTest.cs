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
        private Mock<IEmployeeService> mockEmployeeService;
        private Mock<IEmployeeRepository> mockEmployeeRepository;
        EmployeesController sut;
        private Guid newEmployeeId;
        private Guid noEmployeeId;
        private Employee employee;
        private JsonPatchDocument patchedEmployee;

        [TestInitialize]
        public void EmployeeInitialize()
        {
            employee = new Employee
            {

            };

            mockEmployeeService = new Mock<IEmployeeService>();
            mockEmployeeRepository = new Mock<IEmployeeRepository>();
            sut = new EmployeesController(mockEmployeeService.Object, mockEmployeeRepository.Object);
            newEmployeeId = Guid.NewGuid();
            noEmployeeId = Guid.Empty;
            patchedEmployee = new JsonPatchDocument();

            mockEmployeeRepository
                .Setup(cr => cr.Retrieve(newEmployeeId))
                .Returns(employee);

            mockEmployeeRepository
                .Setup(cr => cr.Retrieve(noEmployeeId))
                .Returns<Employee>(null);

        }

        [TestMethod]
        public void GetEmployee_WithNoId_ShouldReturnOkObjectValue()
        {
            //Arrange
            mockEmployeeRepository
                .Setup(cr => cr.Retrieve())
                .Returns(new List<Employee>());


            //Act
            var result = sut.GetEmployees(null);

            //Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void GetEmployee_WithExistingId_ShouldReturnOkObjectValue()
        {
            //Arrange

            mockEmployeeRepository
                .Setup(cr => cr.Retrieve(newEmployeeId))
                .Returns(employee);


            //Act
            var result = sut.GetEmployees(newEmployeeId);

            //Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void CreateEmployee_WithEmptyEmployee_ReturnBadRequestResult()
        {
            //Arrange
            Employee employee = null;

            //Act
            var result = sut.CreateEmployee(null);

            //Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));

            mockEmployeeService
                .Verify(cr => cr.Save(Guid.Empty, employee), Times.Never());
        }

        [TestMethod]
        public void CreateEmployee_WithValidEmployee_ReturnCreatedAtActionResult()
        {
            ;

            //Act
            var result = sut.CreateEmployee(employee);

            //Assert
            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));

            mockEmployeeService
                .Verify(cr => cr.Save(Guid.Empty, employee), Times.Once());
        }


        [TestMethod]
        public void DeleteEmployee_WithExistingId_ShouldReturnNoContentResult()
        {
            //Arrange
            mockEmployeeRepository
                .Setup(cr => cr.Retrieve(newEmployeeId))
                .Returns(employee);

            //Act
            var result = sut.DeleteEmployee(newEmployeeId);


            //Assert
            mockEmployeeRepository
                .Verify(cr => cr.Delete(newEmployeeId), Times.Once());

            Assert.IsInstanceOfType(result, typeof(NoContentResult));


        }

        [TestMethod]
        public void DeleteEmployee_WithNotExistingId_ShouldReturnNotFoundResult()
        {
            //Arrange
            mockEmployeeRepository
                .Setup(cr => cr.Retrieve(noEmployeeId))
                .Returns<Employee>(null);

            //Act
            var result = sut.DeleteEmployee(noEmployeeId);


            //Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));

            mockEmployeeRepository
                .Verify(cr => cr.Delete(newEmployeeId), Times.Never());




        }

        [TestMethod]
        public void UpdateEmployee_WithNoExistingEmployee_ShouldReturnBadRequestResult()
        {
            //Arrange
            employee = null;

            //Act
            var result = sut.UpdateEmployee(employee, newEmployeeId);

            //Arrange
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));

            mockEmployeeService
                .Verify(cr => cr.Save(newEmployeeId, employee), Times.Never());
        }

        [TestMethod]
        public void UpdateEmployee_WithNoExistingId_ShouldReturnNotFoundResult()
        {
            //Arrange


            //Act
            var result = sut.UpdateEmployee(employee, noEmployeeId);

            //Arrange
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));

            mockEmployeeService
                .Verify(cr => cr.Save(noEmployeeId, employee), Times.Never());
        }

        [TestMethod]
        public void UpdateEmployee_WithExistingEmployeeAndId_ShouldReturnOkObjectResult()
        {


            //Act
            var result = sut.UpdateEmployee(employee, newEmployeeId);

            //Arrange
            mockEmployeeService
                .Verify(cr => cr.Save(newEmployeeId, employee), Times.Once());


            Assert.IsInstanceOfType(result, typeof(OkObjectResult));


        }

        [TestMethod]
        public void PatchEmployee_WithNoExistingPatchEmployee_ShouldReturnBadRequestResult()
        {
            //Arrage
            patchedEmployee = null;
            //Act
            var result = sut.PatchEmployee(patchedEmployee, newEmployeeId);
            //Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));

            mockEmployeeService
                .Verify(cr => cr.Save(newEmployeeId, employee), Times.Never());


        }

        [TestMethod]
        public void PatchEmployee_WithNoExistingId_ShouldReturnNotFoundResult()
        {
            //Arrage

            //Act
            var result = sut.PatchEmployee(patchedEmployee, noEmployeeId);
            //Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));

            mockEmployeeService
                .Verify(cr => cr.Save(noEmployeeId, employee), Times.Never());


        }

        [TestMethod]
        public void PatchEmployee_WithExistingPactchEmployeeAndId_ShouldReturnOkObjectResult()
        {

            //Act
            var result = sut.PatchEmployee(patchedEmployee, newEmployeeId);
            //Assert
            mockEmployeeService
                .Verify(cr => cr.Save(newEmployeeId, employee), Times.Once());

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));




        }
    }
}
