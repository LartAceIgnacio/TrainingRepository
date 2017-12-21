using BlastAsia.DigiBook.Domain.Departments;
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
        private Department department;
        private Mock<IDepartmentRepository> mockDepartmentRepository;
        private DepartmentService sut;


        [TestInitialize]
        public void Initialize()
        {
            department = new Department
            {
                DepartmentName = "Department",
                DepartmentHeadId = Guid.NewGuid()
            };

            mockDepartmentRepository = new Mock<IDepartmentRepository>();

            sut = new DepartmentService(mockDepartmentRepository.Object);
        }

        [TestCleanup]
        public void Cleanup()
        {

        }

        [TestMethod]
        public void Save_DepartmentWithValidData_ShouldCallRepositoryCreate()
        {
            //Act
            var result = sut.Save(department);

            //Assert
            mockDepartmentRepository
                .Verify(d => d.Retrieve(department.DeparmentId), Times.Once);

            mockDepartmentRepository
                .Verify(d => d.Save(department), Times.Once);
        }

        public void Save_ExistingDepartment_ShouldCallRepositoryUpdate()
        {
            // Arrange
            var existingDepartmentId = Guid.NewGuid();

            // Act

            // Assert
        }

        [TestMethod]
        public void Save_BlankDepartmentName_ThrowsDepartmentNameRequiredException()
        {
            // Arrange
            department.DepartmentName = "";

            // Assert
            Assert.ThrowsException<DepartmentNameRequiredException>(
                () => sut.Save(department));
        }

        [TestMethod]
        public void Save_DepartmentNameLessThanMinimumLength_ThrowsMinimumLengthRequired()
        {
            // Arrange
            department.DepartmentName = "Depar";

            // Assert
            Assert.ThrowsException<MinimumLengthRequiredException>(
                () => sut.Save(department));

        }
    }
}
