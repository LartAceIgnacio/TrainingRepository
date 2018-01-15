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
        private readonly Guid existingEmployeeId = Guid.NewGuid();
        private readonly Guid nonExistingEmployeeId = Guid.Empty;
        private Mock<IEmployeeRepository> mockEmployeeRepository;
        private Mock<IEmployeeService> employeeService;
        private EmployeesController sut;

        private Employee employee = new Employee();
        private List<Employee> employeeList = new List<Employee>();

        [TestInitialize]
        public void InitializeTest()
        {
            mockEmployeeRepository = new Mock<IEmployeeRepository>();
            employeeService = new Mock<IEmployeeService>();

            sut = new EmployeesController(employeeService.Object, mockEmployeeRepository.Object);

            //GetEmployees with id
            mockEmployeeRepository
              .Setup(cr => cr.Retrieve(existingEmployeeId))
              .Returns(employee);

            //GetEmployees without id
            mockEmployeeRepository
                .Setup(cr => cr.Retrieve())
                .Returns(employeeList);

            //Update with existingId
            mockEmployeeRepository
                .Setup(cr => cr.Retrieve(existingEmployeeId))
                .Returns(employee);

            //Update without existingId
            mockEmployeeRepository
                .Setup(cr => cr.Retrieve(nonExistingEmployeeId))
                .Returns<Employee>(null);

            //CreatEmployee with null Employee
            mockEmployeeRepository
                .Setup(cr => cr.Create(null))
                .Returns<Employee>(null);


            //CreatEmployee with valid Employee
            mockEmployeeRepository
                .Setup(cr => cr.Create(employee))
                .Returns(employee);


        }
        [TestMethod]
        public void GetEmployees_WithValidId_ShouldReturnOkObjectValue()
        {
            //Act
            var result = sut.GetEmployees(existingEmployeeId);

            //Assert
            mockEmployeeRepository
                .Verify(cr => cr.Retrieve(existingEmployeeId), Times.Once);

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void GetEmployees_WithoutId_ShouldReturnOkObjectValue()
        {
            //Act
            var result = sut.GetEmployees(null);

            //Assert
            mockEmployeeRepository
                .Verify(cr => cr.Retrieve(), Times.Once);

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void UpdateEmployee_WithValidId_ShouldReturnOkObjectValue()
        {


            //Act
            var result = sut.UpdateEmployee(employee, existingEmployeeId);


            //Assert 
            mockEmployeeRepository
                .Verify(cr => cr.Retrieve(existingEmployeeId), Times.Once);

            employeeService
               .Verify(cr => cr.Save(existingEmployeeId, employee), Times.Once);

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }
        [TestMethod]
        public void UpdateEmployee_WithNullEmployee_ShouldReturnBadRequest()
        {
            //Arrange
            employee = null;
            //act
            var result = sut.UpdateEmployee(employee, nonExistingEmployeeId);

            //Assert
            mockEmployeeRepository
                .Verify(cr => cr.Retrieve(nonExistingEmployeeId), Times.Never);

            employeeService
                .Verify(cs => cs.Save(nonExistingEmployeeId, employee), Times.Never);

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }
        [TestMethod]
        public void UpdateEmployee_WithInvalidId_ShouldReturnBadRequest()
        {
            //Act
            var result = sut.UpdateEmployee(employee, nonExistingEmployeeId);

            //Assert
            mockEmployeeRepository
                .Verify(cr => cr.Retrieve(nonExistingEmployeeId), Times.Once);

            employeeService
                .Verify(cs => cs.Save(nonExistingEmployeeId, employee), Times.Never);

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void CreateEmployee_WithNullEmployee_ShouldReturnBadRequest()
        {
            //Arrange
            employee = null;

            //Act
            var result = sut.CreateEmployee(employee);


            //Assert
            employeeService
                .Verify(cs => cs.Save(Guid.Empty, employee), Times.Never);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void CreateEmployee_WithValidEmployee_ShouldReturnCreatedAtActionResult()
        {
            //Act
            var result = sut.CreateEmployee(employee);


            //Assert  
            employeeService
                .Verify(cs => cs.Save(Guid.Empty, employee), Times.Once);

            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
        }

        [TestMethod]
        public void DeleteEmployee_WithValidEmployeeId_ShouldReturnNoContentResult()
        {
            //Act
            var result = sut.DeleteEmployee(existingEmployeeId);


            //Assert
            mockEmployeeRepository
                .Verify(cr => cr.Retrieve(existingEmployeeId), Times.Once);

            mockEmployeeRepository
                .Verify(cr => cr.Delete(existingEmployeeId), Times.Once);
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }

        [TestMethod]
        public void DeleteEmployee_WithInvalidEmployeeId_ShouldReturnBadRequest()
        {
            //Act
            var result = sut.DeleteEmployee(nonExistingEmployeeId);

            //Assert
            mockEmployeeRepository
                .Verify(cr => cr.Retrieve(nonExistingEmployeeId), Times.Once);

            mockEmployeeRepository
                .Verify(cr => cr.Delete(nonExistingEmployeeId), Times.Never);

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));

        }

        [TestMethod]
        public void PatchEmployee_WithValidEmployeeId_ShouldReturnOkObjectValue()
        {
            //Arrange
            var patchDoc = new JsonPatchDocument();

            //Act
            var result = sut.PatchEmployee(patchDoc, existingEmployeeId);

            //Assert
            mockEmployeeRepository
                .Verify(cr => cr.Retrieve(existingEmployeeId), Times.Once);

            employeeService
                .Verify(cs => cs.Save(existingEmployeeId, employee), Times.Once);

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

        }

        [TestMethod]
        public void PatchEmployee_WithInvalidEmployeeId_ShouldReturnBadRequest()
        {
            var patchDoc = new JsonPatchDocument();

            //Act
            var result = sut.PatchEmployee(patchDoc, nonExistingEmployeeId);

            //Assert
            mockEmployeeRepository
                .Verify(cr => cr.Retrieve(nonExistingEmployeeId), Times.Once);

            employeeService
                .Verify(cs => cs.Save(nonExistingEmployeeId, employee), Times.Never);

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void PatchEmployee_WithNullPatchEmployee_ShouldReturnBadRequest()
        {
            var patchDoc = new JsonPatchDocument();
            patchDoc = null;

            //Act
            var result = sut.PatchEmployee(patchDoc, existingEmployeeId);

            //Assert
            mockEmployeeRepository
                .Verify(cr => cr.Retrieve(existingEmployeeId), Times.Never);

            employeeService
                .Verify(cs => cs.Save(existingEmployeeId, employee), Times.Never);

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }


    }
}
