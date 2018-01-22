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
    public class DepartmentsControllerTest
    {
        private Mock<IDepartmentService> mockDepartmentService;
        private Mock<IDepartmentRepository> mockDepartmentRepository;
        private Department department;
        private DepartmentsController sut;
        private JsonPatchDocument patchedDepartment;

        [TestInitialize]
        public void Initialize()
        {
            mockDepartmentService = new Mock<IDepartmentService>();
            mockDepartmentRepository = new Mock<IDepartmentRepository>();
            sut = new DepartmentsController(mockDepartmentService.Object, mockDepartmentRepository.Object);
            department = new Department
            {
                DepartmentName = "IT",
                DepartmentHeadId = Guid.NewGuid()
            };
            patchedDepartment = new JsonPatchDocument();
            patchedDepartment.Replace("DepartmentName", "HR");
            mockDepartmentRepository
               .Setup(d => d.Retrieve(department.DepartmentId))
               .Returns(department);
        }

        [TestCleanup]
        public void Cleanup()
        {

        }

        [TestMethod]
        public void GetDepartments_WithEmptyDepartmentId_ReturnOkObjectResult()
        {
            // Arrange
            mockDepartmentRepository
                .Setup(d => d.Retrieve())
                .Returns(new List<Department>());

            // Act
            var result = sut.GetDepartments(null);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            mockDepartmentRepository.Verify(d => d.Retrieve(), Times.Once);
        }

        [TestMethod]
        public void GetDepartments_WithValidDepartmentId_ReturnOkObjectResult()
        {
            // Act
            var result = sut.GetDepartments(department.DepartmentId);

            // Assert
            mockDepartmentRepository.Verify(d => d.Retrieve(department.DepartmentId), Times.Once);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void CreateDepartment_DepartmentWithValidData_ReturnCreatedAtActionResult()
        {
            // Act
            var result = sut.CreateDepartment(department);

            // Assert
            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
            mockDepartmentService.Verify(d => d.Save(department.DepartmentId, department), Times.Once);
        }

        [TestMethod]
        public void CreateDepartment_DepartmentWithNoData_ReturnBadRequestResult()
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
        public void DeleteDepartment_DepartmentDeleted_ReturnNoContentResult()
        {
            // Act
            var result = sut.DeleteDepartment(department.DepartmentId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
            mockDepartmentRepository.Verify(d => d.Delete(department.DepartmentId));
        }

        [TestMethod]
        public void DeleteDepartment_WithoutDepartmentId_ReturnNotFoundResult()
        {
            // Arrange
            mockDepartmentRepository
                .Setup(d => d.Retrieve(department.DepartmentId))
                .Returns<Department>(null);

            // Act
            var result = sut.DeleteDepartment(department.DepartmentId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            mockDepartmentRepository.Verify(d => d.Delete(department.DepartmentId), Times.Never);
        }

        [TestMethod]
        public void UpdateDepartment_WithExistingDepartmentDataAndId_ReturnOkObjectResult()
        {
            // Act
            var result = sut.UpdateDepartment(department, department.DepartmentId);

            // Assert
            mockDepartmentRepository.Verify(d => d.Retrieve(department.DepartmentId), Times.Once);
            mockDepartmentService.Verify(d => d.Save(department.DepartmentId, department), Times.Once);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void UpdateDepartment_DepartmentWithNoValue_ReturnBadRequestResult()
        {
            // Arrange
            department = null;

            // Act
            var result = sut.UpdateDepartment(department, Guid.Empty);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            mockDepartmentRepository.Verify(d => d.Retrieve(Guid.Empty), Times.Never);
            mockDepartmentService.Verify(d => d.Save(Guid.Empty, department), Times.Never);
        }

        [TestMethod]
        public void UpdateDepartment_WithNoExistingDepartmentId_ReturnNotFoundResult()
        {
            // Arrange
            department.DepartmentId = Guid.Empty;
            mockDepartmentRepository
                .Setup(d => d.Retrieve(department.DepartmentId))
                .Returns<Department>(null);

            // Act
            var result = sut.UpdateDepartment(department, department.DepartmentId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            mockDepartmentRepository.Verify(d => d.Retrieve(department.DepartmentId), Times.Once);
            mockDepartmentService.Verify(d => d.Save(department.DepartmentId, department), Times.Never);
        }

        [TestMethod]
        public void PatchDepartment_WithoutExistingDepartmentDataAndId_ReturnOkObjectResult()
        {      
            // Act
            var result = sut.PatchDepartment(patchedDepartment, department.DepartmentId);

            // Assert
            mockDepartmentRepository.Verify(d => d.Retrieve(department.DepartmentId), Times.Once);
            mockDepartmentService.Verify(d => d.Save(department.DepartmentId, department), Times.Once);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void PatchDepartment_WithoutExistingDepartmentDataAndId_ReturnNotFoundResult()
        {
            // Arrange
            mockDepartmentRepository
                .Setup(d => d.Retrieve(department.DepartmentId))
                .Returns<Department>(null);

            // Act
            var result = sut.PatchDepartment(patchedDepartment, department.DepartmentId);

            // Assert
            mockDepartmentRepository.Verify(d => d.Retrieve(department.DepartmentId), Times.Once);
            mockDepartmentService.Verify(d => d.Save(department.DepartmentId, department), Times.Never);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void PatchDepartment_DepartmentWithEmptyPatchDocument_ReturnBadRequestResult()
        {
            // Arrange
            patchedDepartment = null;

            // Act
            var result = sut.PatchDepartment(patchedDepartment, department.DepartmentId);

            // Assert
            mockDepartmentRepository.Verify(d => d.Retrieve(department.DepartmentId), Times.Never);
            mockDepartmentService.Verify(d => d.Save(department.DepartmentId, department), Times.Never);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }
    }
}
