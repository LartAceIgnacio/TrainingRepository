using BlastAsia.DigiBook.API.Controllers;
using BlastAsia.DigiBook.Domain.Departments;
using BlastAsia.DigiBook.Domain.Models.Departments;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.API.Test
{
    [TestClass]
    public class DepartmentsControllerTest
    {
        private Department department;

        Mock<IDepartmentRepository> mockDepartmentRepository;
        Mock<IDepartmentService> mockDepartmentService;

        DepartmentsController sut;

        [TestInitialize]
        public void Initialize()
        {
            department = new Department
            {
                DepartmentId = Guid.NewGuid(),
                DepartmentName = "IT",
                DepartmentHeadId = Guid.NewGuid()
            };

            mockDepartmentRepository = new Mock<IDepartmentRepository>();
            mockDepartmentService = new Mock<IDepartmentService>();

            sut = new DepartmentsController(mockDepartmentRepository.Object, mockDepartmentService.Object);
        }

        [TestCleanup]
        public void CleanUp()
        {

        }

        [TestMethod]
        public void GetDepartments_WithEmptyDepartmentId_ReturnObjectResult()
        {
            // Arrange
            //mockDepartmentRepository
            //    .Setup(d => d.Retrieve())
            //    .Returns(new List<Department>());

            // Act
            var result = sut.GetDepartments(null);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            mockDepartmentRepository.Verify(d => d.Retrieve(), Times.Once);

        }

        [TestMethod]
        public void GetDepartments_WithValidDepartmentId_ReturnOkObjectResult()
        {
            // Arrange
            mockDepartmentRepository
                .Setup(d => d.Retrieve(department.DepartmentId))
                .Returns(department);

            // Act
            var result = sut.GetDepartments(department.DepartmentId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            mockDepartmentRepository.Verify(d => d.Retrieve(department.DepartmentId), Times.Once);
        }

        [TestMethod]
        public void CreateDepartment_DepartmentWithValidData_ReturnCreatedAtActionResult()
        {
            // Act
            var result = sut.CreateDepartment(department);

            // Assert
            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
            mockDepartmentService.Verify(d => d.Save(Guid.Empty,department), Times.Once);
        }

        [TestMethod]
        public void CreateDepartment_DepartmentWithoutValidData_ReturnBadRequestResult()
        {
            // Arrange
            department = null;

            // Act
            var result = sut.CreateDepartment(department);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            mockDepartmentService.Verify(d => d.Save(Guid.Empty, department), Times.Never);
        }

        [TestMethod]
        public void DeleteDepartment_DeleteDepartmentWithValidId_ReturnNoContentResult()
        {
            // Act
            var result = sut.DeleteDepartment(department.DepartmentId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
            mockDepartmentRepository.Verify(d => d.Retrieve(department.DepartmentId), Times.Once);
            mockDepartmentRepository.Verify(d => d.Delete(department.DepartmentId), Times.Once);
        }

        [TestMethod]
        public void DeleteDepartment_DepartmentWithNoExistingId_ReturnNotFoundResult()
        {
            // Act
            var result = sut.DeleteDepartment(department.DepartmentId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            mockDepartmentRepository.Verify(d => d.Retrieve(department.DepartmentId), Times.Once);
            mockDepartmentRepository.Verify(d => d.Delete(department.DepartmentId), Times.Never);

        }

        [TestMethod]
        public void UpdateDepartment_WithExistingDepartmentDataAndId_ReturnOkObjectResult()
        {
            // Arrange
            mockDepartmentRepository
                .Setup(d => d.Retrieve(department.DepartmentId))
                .Returns(department);

            // Act
            var result = sut.UpdateDepartment(department, department.DepartmentId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            mockDepartmentRepository.Verify(d => d.Retrieve(department.DepartmentId), Times.Once);
            mockDepartmentService.Verify(d => d.Save(department.DepartmentId, department), Times.Once);
        }

        [TestMethod]
        public void UpdateDepartment_WithoutExistingDepartmentDataAndId_ReturnNotFoundResult()
        {
            // Arrange

            // Act

            // Assert

        }
    }
}
