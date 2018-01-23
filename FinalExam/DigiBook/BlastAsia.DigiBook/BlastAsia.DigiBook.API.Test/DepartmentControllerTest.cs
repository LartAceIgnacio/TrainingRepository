using BlastAsia.DigiBook.API.Controllers;
using BlastAsia.DigiBook.Domain.Departments;
using BlastAsia.DigiBook.Domain.Models.Departments;
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
    public class DepartmentControllerTest
    {
        private Department department;
        private Mock<IDepartmentRepository> mockDepartmentRepository;
        private Mock<IDepartmentService> mockDepartmentService;
        private JsonPatchDocument patchedDepartment;

        private DepartmentsController sut;

        private Guid existingDepartmentId = Guid.NewGuid();
        private Guid nonExistingDepartmentId = Guid.Empty;

        [TestInitialize]
        public void Initialize()
        {
            mockDepartmentRepository = new Mock<IDepartmentRepository>();
            mockDepartmentService = new Mock<IDepartmentService>();
            patchedDepartment = new JsonPatchDocument();

            department = new Department
            {
                DepartmentId = Guid.NewGuid()
            };

            sut = new DepartmentsController(mockDepartmentService.Object,
                mockDepartmentRepository.Object);

            mockDepartmentRepository
                .Setup(d => d.Retrieve())
                .Returns(() => new List<Department>{
                    new Department() });

            mockDepartmentRepository
                .Setup(d => d.Retrieve(department.DepartmentId))
                .Returns(department);
            mockDepartmentRepository
                .Setup(d => d.Retrieve(existingDepartmentId))
                .Returns(department);
        }
        [TestMethod]
        public void GetDepartments_WithEmptyDepartmentId_ReturnsOkObjectResult()
        {
            // Act
            var result = sut.GetDepartments(null);

            // Assert 
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

            mockDepartmentRepository
                .Verify(d => d.Retrieve(), Times.Once());
        }
        [TestMethod]
        public void GetDepartment_WithDepartmentId_ReturnsOkObjectResult()
        {
            // Act
            var result = sut.GetDepartments(existingDepartmentId);

            //Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

            mockDepartmentRepository
                .Verify(d => d.Retrieve(existingDepartmentId), Times.Once());
        }
        [TestMethod]
        public void CreateDepartment_WithEmptyDepartment_ReturnsBadRequestResult()
        {
            // Act 
            department = null;
            var result = sut.CreateDepartment(department);

            //Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));

            mockDepartmentService
                .Verify(d => d.Save(Guid.Empty, department), Times.Never);
        }
        [TestMethod]
        public void CreateDepartment_WithValidData_ReturnsCreatedAtActionResult()
        {
            //Arrange
            //department.DeparmentMemberId = existingEmployeeId;
            // Act 
            var result = sut.CreateDepartment(department);
            // Assert
            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));

            mockDepartmentService
                .Verify(d => d.Save(Guid.Empty, department), Times.Once);

            //mockDepartmentRepository
            //    .Verify(d => d.Retrieve(department.DeparmentMemberId), Times.Once);
        }
    }
}
