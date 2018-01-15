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
        private Department department;
        private Mock<IDepartmentRepository> mockDepartmentRepository;
        private Mock<IDepartmentService> mockDepartmentService;
        private JsonPatchDocument patchedDepartment;

        private DepartmentsController sut;

        private Guid existingDepartmentId = Guid.NewGuid();
        private Guid notExistingDepartmentId = Guid.Empty;


        [TestInitialize]
       public void Initialize()
        {
            mockDepartmentRepository = new Mock<IDepartmentRepository>();
            mockDepartmentService = new Mock<IDepartmentService>();
            patchedDepartment = new JsonPatchDocument();

            department = new Department
            {

                DepartmentId = Guid.NewGuid(),
                DepartmentName = "Project Management Department"
            };

            sut = new DepartmentsController(mockDepartmentService.Object,
               mockDepartmentRepository.Object);
        }

        [TestMethod]
        public void GetDepartment_WithEmptyDepartmentId_ReturnsObjectResult()
        {
            // Act
            var result = sut.GetDepartment(null);

            // Assert 
           
            mockDepartmentRepository
               .Verify(c => c.Retrieve(), Times.Once());

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void GetDepartment_WithDepartmentID_ReturnsObjectResult()
        {
            // Act
            var result = sut.GetDepartment(notExistingDepartmentId);

            // Assert
            
            mockDepartmentRepository
               .Verify(c => c.Retrieve(notExistingDepartmentId), Times.Once());

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

        }

        [TestMethod]
        public void CreateDepartment_WithEmptyDepartment_ReturnsBadRequestResult()
        {
            // Arrange

            department = null;

            // Act

            var result = sut.CreateDepartment(department);

            //Assert

            mockDepartmentService
                .Verify(d => d.Save(Guid.Empty, department), Times.Never());

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void CreateDepartment_WithValidDepartment_ReturnsObjectResult()
        {

            mockDepartmentService
              .Setup(d => d.Save(Guid.Empty, department))
              .Returns(department);

            //Act
            var result = sut.CreateDepartment(department);

            // Assert         
            mockDepartmentService
             .Verify(c => c.Save(Guid.Empty, department), Times.Once());

            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
        }


        //[TestMethod]
        //public void UpdateDepartment_WithValidDepartment_ReturnsObjectResult()
        //{

        //    var result = sut.UpdateDepartment(department, existingDepartmentId);
        //    // Assert

        //    //mockDepartmentRepository
        //    //    .Verify(c => c.Retrieve(existingDepartmentId), Times.Once());

        //    // mockDepartmentService
        //    //    .Verify(c => c.Save(existingDepartmentId, department), Times.Once());

        //    Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        //}
        //[TestMethod]
        //public void UpdateDepartment_WithEmptyDepartment_ReturnsBadRequestResult()
        //{
        //    department = null;
        //    // Act
        //    var result = sut.UpdateDepartment(department, existingDepartmentId);

        //    // Assert
        //    mockDepartmentRepository
        //        .Verify(c => c.Update(existingDepartmentId, department), Times.Never());

        //    Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        //}

        //[TestMethod]
        //public void UpdateDepartment_WithEmptyDepartmentId_ReturnsNotFoundResult()
        //{
        //    var result = sut.UpdateDepartment(department, notExistingDepartmentId);

        //    // Assert
        //    mockDepartmentRepository
        //        .Verify(c => c.Update(notExistingDepartmentId, department), Times.Never());

        //    Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        //}

        //[TestMethod]
        //public void DeleteDepartment_WithDepartmentId_ReturnsNoContentResult()
        //{
        //    // Act
        //    var result = sut.DeleteDepartment(existingDepartmentId);

        //    //Assert          
        //    mockDepartmentRepository
        //        .Verify(c => c.Delete(existingDepartmentId), Times.Once());

        //    Assert.IsInstanceOfType(result, typeof(NoContentResult));
        //}

        //[TestMethod]
        //public void DeleteDepartment_WithEmptyDepartmentId_ReturnsNotFound()
        //{
        //    //Act
        //    var result = sut.DeleteDepartment(notExistingDepartmentId);

        //    // Assert 
        //    mockDepartmentRepository
        //        .Verify(c => c.Delete(notExistingDepartmentId),
        //        Times.Never());
        //    Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        //}

        //[TestMethod]
        //public void PatchDepartment_WithValidPatchedDepartment_ReturnsObjectResult()
        //{
        //    var result = sut.PatchDepartment(patchedDepartment, existingDepartmentId);
        //    // Assert

        //    mockDepartmentRepository
        //        .Verify(c => c.Retrieve(existingDepartmentId), Times.Once());

        //    mockDepartmentService
        //        .Verify(c => c.Save(existingDepartmentId, department), Times.Once());

        //    Assert.IsInstanceOfType(result, typeof(OkObjectResult));

        //}
        //[TestMethod]
        //public void PatchDepartment_WithEmptyPatchedDepartment_ReturnsBadRequestResult()
        //{
        //    patchedDepartment = null;
        //    // Act
        //    var result = sut.PatchDepartment(patchedDepartment, existingDepartmentId);

        //    // Assert
        //    mockDepartmentRepository
        //       .Verify(c => c.Retrieve(notExistingDepartmentId), Times.Never());

        //    mockDepartmentService
        //        .Verify(c => c.Save(notExistingDepartmentId, department), Times.Never());

        //    Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        //}

        //[TestMethod]
        //public void PatchDepartment_WithInvalidDepartmentId_ReturnsNotFound()
        //{
        //    var result = sut.PatchDepartment(patchedDepartment, notExistingDepartmentId);

        //    //Assert
        //    mockDepartmentRepository
        //        .Verify(c => c.Retrieve(notExistingDepartmentId), Times.Once());

        //    mockDepartmentService
        //         .Verify(c => c.Save(notExistingDepartmentId, department), Times.Never());

        //    Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        //}
    }
}
