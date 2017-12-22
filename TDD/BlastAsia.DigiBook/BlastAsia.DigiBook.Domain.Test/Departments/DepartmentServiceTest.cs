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
        Department department;
        Employee employee;

        Mock<IDepartmentRepository> mockDepartmentRepository;
        Mock<IEmployeeRepository> mockEmployeeRepository;

        DepartmentService sut;


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

            mockEmployeeRepository
                .Setup(e => e.Retrieve(department.DepartmentHeadId))
                .Returns(employee);
        }

        [TestCleanup]
        public void Cleanup()
        {

        }

        [TestMethod]
        public void Save_WithValidDepartmentData_ShouldCallRepositoryCreate()
        {
            // Act
            sut.Save(department.DepartmentId,department);

            // Assert
            mockDepartmentRepository.Verify(d => d.Create(department), Times.Once);
        }

        [TestMethod]
        public void Save_DepartmentWithValidData_ShouldReturnDataWithDepartmentId()
        {
            // Arrange
            mockDepartmentRepository
                .Setup(d => d.Create(department))
                .Callback(() => department.DepartmentId = Guid.NewGuid())
                .Returns(department);

            // Act
            sut.Save(department.DepartmentId,department);

            // Assert
            mockDepartmentRepository.Verify(d => d.Create(department), Times.Once);
        }

        [TestMethod]
        public void Save_DepartmentWithExistingData_ShouldCallRepositoryUpdate()
        {
            // Arrange
            mockDepartmentRepository
                .Setup(d => d.Retrieve(department.DepartmentId))
                .Returns(department);

            // Act
            sut.Save(department.DepartmentId,department);

            // Assert
            mockDepartmentRepository
                .Verify(d => d.Retrieve(department.DepartmentId), Times.Once);
            mockDepartmentRepository
                .Verify(d => d.Update(department.DepartmentId, department), Times.Once);
        }

        [TestMethod]
        public void Save_DepartmentNameBlank_ThrowsDeparmentNameRequiredException()
        {
            // Arrange
            department.DepartmentName = null;

            // Assert
            Assert.ThrowsException<DeparmentNameRequiredException>(
                () => sut.Save(department.DepartmentId,department));
            mockDepartmentRepository
                .Verify(d => d.Create(department), Times.Never);
        }

        [TestMethod]
        public void Save_NotexistingDepartmentHeadId_ThrowsDepartmentHeadIdNotFoundException()
        {
            // Arrange
            mockEmployeeRepository
                .Setup(e => e.Retrieve(department.DepartmentHeadId))
                .Returns<Employee>(null);

            // Assert
            Assert.ThrowsException<DepartmentHeadIdNotFoundException>(
                () => sut.Save(department.DepartmentId,department));
            mockEmployeeRepository
                .Verify(e => e.Retrieve(department.DepartmentHeadId), Times.Once);
            mockDepartmentRepository
                .Verify(d => d.Create(department), Times.Never);
        }
    }
}
