using BlastAsia.DigiBook.Domain.Departments;
using BlastAsia.DigiBook.Domain.Employees;
using BlastAsia.DigiBook.Domain.Models.Departments;
using BlastAsia.DigiBook.Domain.Models.Employees;
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
        private Mock<IEmployeeRepository> mockEmployeeRepository;
        private DepartmentService sut;
        private Employee employee;

        [TestInitialize]
        public void Initialize()
        {
            department = new Department
            {
                DepartmentName = "IT",
                DepartmentHeadId = Guid.NewGuid()
            };
            employee = new Employee();
            mockDepartmentRepository = new Mock<IDepartmentRepository>();
            mockEmployeeRepository = new Mock<IEmployeeRepository>();
            sut = new DepartmentService(mockDepartmentRepository.Object, mockEmployeeRepository.Object);
            mockDepartmentRepository
                .Setup(d => d.Retrieve(department.DepartmentId))
                .Returns<Department>(null);
            mockEmployeeRepository
                .Setup(e => e.Retrieve(department.DepartmentHeadId))
                .Returns(employee);
        }

        [TestCleanup]
        public void Cleanup()
        {

        }

        [TestMethod]
        public void Save_DepartmentWithValidDetails_ShouldCallRepositoryCreate()
        {
            // Act
            var result = sut.Save(department.DepartmentId, department);

            // Assert
            mockDepartmentRepository.Verify(d => d.Retrieve(department.DepartmentId), Times.Once);
            mockDepartmentRepository.Verify(d => d.Create(department), Times.Once);
        }

        [TestMethod]
        public void Save_DepartmentWithValidDetails_ShouldCreateDepartmentId()
        {
            // Arrange
            mockDepartmentRepository
                .Setup(d => d.Create(department))
                .Callback(() => department.DepartmentHeadId = Guid.NewGuid())
                .Returns(department);

            // Act
            var result = sut.Save(department.DepartmentId, department);

            // Assert
            mockDepartmentRepository.Verify(d => d.Retrieve(department.DepartmentId), Times.Once);
            mockDepartmentRepository.Verify(d => d.Create(department), Times.Once);
        }

        [TestMethod]
        public void Save_ExistingDepartmentWithValidDetails_ShouldCallRepositoryUpdate()
        {
            // Arrange
            mockDepartmentRepository
                .Setup(d => d.Retrieve(department.DepartmentId))
                .Returns(department);

            // Act
            var result = sut.Save(department.DepartmentId, department);

            // Assert
            mockDepartmentRepository.Verify(d => d.Retrieve(department.DepartmentId), Times.Once);
            mockDepartmentRepository.Verify(d => d.Update(department.DepartmentId, department), Times.Once);
        }

        [TestMethod]
        public void Save_WithBlankDepartmentName_ShouldThrowDepartmentNameRequiredException()
        {
            // Arrange
            department.DepartmentName = null;

            // Assert
            Assert.ThrowsException<DepartmentNameRequiredException>(
                () => sut.Save(department.DepartmentId, department));
        }

        [TestMethod]
        public void Save_NotFoundDepartmentHeadId_ShouldThrowDepartmentHeadRequiredException()
        {
            // Arrange
            mockEmployeeRepository
                .Setup(e => e.Retrieve(department.DepartmentHeadId))
                .Returns<Employee>(null);

            // Assert
            Assert.ThrowsException<DepartmentHeadRequiredException>(
                () => sut.Save(department.DepartmentId, department));
        }
    }
}