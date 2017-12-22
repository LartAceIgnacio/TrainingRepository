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
        private Mock<IDepartmentRepository> mockDepartmentRepository;
        private DepartmentService sut;
        private Department department;
        private Object result;
        private Guid existingDepartmentHead = Guid.NewGuid();
        private Guid nonExistingDepartmentHead = Guid.Empty;

        [TestInitialize]
        public void Init()
        {
            mockDepartmentRepository = new Mock<IDepartmentRepository>();
            sut = new DepartmentService(mockDepartmentRepository.Object);
            department = new Department
            {
                DepartmentName = "Hr Department",
                DepartmentHead = existingDepartmentHead
            };

            mockDepartmentRepository
                .Setup(dr => dr.Retrieve(existingDepartmentHead))
                .Returns(department);

            mockDepartmentRepository
                .Setup(dr => dr.Retrieve(nonExistingDepartmentHead))
                .Returns<Department>(null);

        }
        [TestMethod]
        public void Save_WithValidDeparmentData_ShouldCallCreateRepository()
        {
            //Arrange
            
            //Act
            result = sut.Save(department);
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
                () => sut.Save(department));
            mockDepartmentRepository
               .Verify(dr => dr.Create(department), Times.Never);
        }

        [TestMethod]
        public void Save_WithNonExistingDepartmentHead_ThrowsNonExistingDepartmentHeadException()
        {
            //Arrange
            department.DepartmentHead = nonExistingDepartmentHead;

            //Act
     
            //Assert
            Assert.ThrowsException<NonExistingDepartmentHeadException>(
                () => sut.Save(department));
            mockDepartmentRepository
                .Verify(dr => dr.Create(department), Times.Never);
        }

        //[TestMethod]
        //public void Save_WithExistingDepartmentId_ShouldRepositoryUpdate()
        //{
        //    //Arrange
        //    mockDepartmentRepository
        //        .Setup(dr => dr.Retrieve(department.DepartmentId)
        //    //Act

        //    //Assert
        //}
    }
}
