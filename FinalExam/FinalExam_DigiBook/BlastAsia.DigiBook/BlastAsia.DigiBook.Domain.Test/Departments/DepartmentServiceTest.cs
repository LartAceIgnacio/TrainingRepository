using BlastAsia.DigiBook.Domain.Departments;
using BlastAsia.DigiBook.Domain.Departments.Exceptions;
using BlastAsia.DigiBook.Domain.Models.Departments;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Test.Departments
{
    [TestClass]
    public class DepartmentServiceTest
    {
        private Mock<IDepartmentRepository> mockDepartmentRepository;
        private DepartmentService sut;
        private Department department;
        private Guid existingDepartmentId = Guid.NewGuid();
        private Guid nonExistingDepartmentId = Guid.Empty;

        [TestInitialize]
        public void InitializeTest()
        {
            department = new Department
            {
                DepartmentName = "Accountung",
                Description = "Lala"
            };

            mockDepartmentRepository = new Mock<IDepartmentRepository>();

            sut = new DepartmentService(mockDepartmentRepository.Object);

            mockDepartmentRepository
                .Setup(c => c.Retrieve(existingDepartmentId))
                .Returns(department);

            mockDepartmentRepository
                .Setup(c => c.Retrieve(nonExistingDepartmentId))
                .Returns<Department>(null);
        }

        [TestCleanup]
        public void CleanupTest()
        {

        }

        [TestMethod]
        public void Create_NewDepartmentWithValidData_ShouldCallRepositoryCreate()
        {
            // Arrange

            // Act

            var result = sut.Save(department.DepartmentId, department);

            // Assert

            mockDepartmentRepository
                .Verify(c => c.Retrieve(nonExistingDepartmentId), Times.Once);
            mockDepartmentRepository
                .Verify(c => c.Create(department), Times.Once);

        }
        [TestMethod]
        public void Update_WithExistingDepartment_ShouldCallRepositoryUpdate()
        {
            // Arrange

            department.DepartmentId = existingDepartmentId;

            // Act

            var result = sut.Save(department.DepartmentId, department);

            // Assert

            mockDepartmentRepository
                .Verify(c => c.Retrieve(existingDepartmentId), Times.Once);
            mockDepartmentRepository
                .Verify(c => c.Update(existingDepartmentId, department), Times.Once);
        }
        [TestMethod]
        public void Save_WithValidData_ReturnsNewDepartmentWithId()
        {
            // Arrange

            mockDepartmentRepository
                .Setup(c => c.Create(department))
                .Callback(() => department.DepartmentId = Guid.NewGuid())
                .Returns(department);

            // Act

            var newDepartment = sut.Save(department.DepartmentId, department);

            // Assert

            Assert.IsNotNull(newDepartment);

            Assert.IsTrue(newDepartment.DepartmentId != Guid.Empty);
        }
        [TestMethod]
        public void Create_WithBlankDepartmentName_ThrowsDepartmentNameRequiredException()
        {
            // Arrange

            // Act

            department.DepartmentName = "";

            // Assert

            mockDepartmentRepository
                .Verify(c => c.Create(department), Times.Never());
            Assert.ThrowsException<DepartmentNameRequiredException>(
                () => sut.Save(department.DepartmentId, department));
        }
        [TestMethod]
        public void Create_WithBlankDepartmentDescription_ThrowsDepartmentDescriptionException()
        {
            // Arrange

            // Act

            department.Description = "";

            // Assert

            mockDepartmentRepository
                .Verify(c => c.Create(department), Times.Never());
            Assert.ThrowsException<DepartmentDescriptionException>(
                () => sut.Save(department.DepartmentId, department));
        }
    }
}
