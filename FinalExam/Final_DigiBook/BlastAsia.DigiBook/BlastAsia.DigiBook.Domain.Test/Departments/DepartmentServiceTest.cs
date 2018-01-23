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
        private Mock<IDepartmentRepository> mockDepartmentRepository;
        private Mock<IEmployeeRepository> mockEmployeeRepository;
        private DepartmentService sut;
        private Employee employee;
        private Department department;
        private Guid existingDepartmentHeadId = Guid.NewGuid();
        private Guid nonExistingDepartmentHeadId = Guid.Empty;

        [TestInitialize]
        public void Init()
        {
            mockDepartmentRepository = new Mock<IDepartmentRepository>();
            mockEmployeeRepository = new Mock<IEmployeeRepository>();
            sut = new DepartmentService(mockDepartmentRepository.Object, 
                mockEmployeeRepository.Object);

            employee = new Employee();

            department = new Department
            {
                DepartmentName = "Hr Department",
                DepartmentHeadId = existingDepartmentHeadId
            };

             mockDepartmentRepository
                .Setup(dr => dr.Retrieve(existingDepartmentHeadId))
                .Returns(department);

            mockEmployeeRepository
               .Setup(dr => dr.Retrieve(existingDepartmentHeadId))
               .Returns(employee);

            mockDepartmentRepository
                .Setup(dr => dr.Retrieve(nonExistingDepartmentHeadId))
                .Returns<Department>(null);

        }
        [TestMethod]
        public void Save_WithValidDeparmentData_ShouldCallCreateRepository()
        {
            //Arrange
            
            //Act
            sut.Save(department.DepartmentId, department);
            //Assert

            mockDepartmentRepository
                 .Verify(dr => dr.Create(department), Times.Once);
            Assert.IsInstanceOfType(department, typeof(Department));
        }

        [TestMethod]
        public void Save_WithBlankDepartmentName_ThrowsRequiredDepartmentNameException()
        {
            //Arrange
            department.DepartmentName = "";

            //Act

            //Assert
           
            Assert.ThrowsException<RequiredDepartmentNameException>(
                () => sut.Save(department.DepartmentId, department));
            mockDepartmentRepository
               .Verify(dr => dr.Create(department), Times.Never);
        }

        [TestMethod]
        public void Save_WithNonExistingDepartmentHead_ThrowsNonExistingDepartmentHeadException()
        {
            //Arrange
            department.DepartmentHeadId = nonExistingDepartmentHeadId;

            //Act
     
            //Assert
            Assert.ThrowsException<NonExistingDepartmentHeadException>(
                () => sut.Save(department.DepartmentId, department));
            mockDepartmentRepository
                .Verify(dr => dr.Create(department), Times.Never);
        }

        [TestMethod]
        public void Save_WithExistingDepartmentId_ShouldCallRepositoryUpdate()
        {
            //Arrange
            mockEmployeeRepository
                .Setup(er => er.Retrieve(department.DepartmentHeadId))
                .Returns(employee);

            mockDepartmentRepository
               .Setup(dr => dr.Retrieve(department.DepartmentId))
               .Returns(department);

            //Act
            sut.Save(department.DepartmentId, department);

            //Assert
            mockDepartmentRepository
                .Verify(dr => dr.Update(department.DepartmentId, department), Times.Once);
            mockDepartmentRepository
                .Verify(dr => dr.Update(department.DepartmentId, department), Times.Once);
            mockDepartmentRepository
                .Verify(dr => dr.Create(department), Times.Never);
        }
    }
}
