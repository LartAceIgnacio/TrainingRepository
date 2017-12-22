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
        private Department department;
        private Mock<IDepartmentRepository> mockDepartmentRepository;
        private DepartmentService sut;

        private Guid existingDepartmentId = Guid.NewGuid();
        private Guid nonExistingDepartmentId = Guid.Empty;

        private Guid existingEmployeeId = Guid.NewGuid();

        [TestInitialize]
        public void Initialize()
        {
            department = new Department
            {
                DepartmentName = "Hr Department",
                DeparmentMemberId = existingEmployeeId
            };

            mockDepartmentRepository = new Mock<IDepartmentRepository>();

            sut = new DepartmentService(mockDepartmentRepository.Object);

            mockDepartmentRepository
               .Setup(d => d.Create(department))
               .Callback(() => department.DepartmentId = Guid.NewGuid())
               .Returns(department);

            mockDepartmentRepository
               .Setup(d => d.Retrieve(existingDepartmentId))
               .Returns(department);

            //mockDepartmentRepository
            //  .Setup(d => d.Retrieve(nonExistingDepartmentId))
            //  .Returns<Department>(null);
        }

        [TestMethod]
        public void Save_NewDepartmentWithValidData_ShouldCallRepositoryCreate()
        {
            //Act

            var result = sut.Save(department.DepartmentId,department);

            //Assert
            mockDepartmentRepository
               .Verify(c => c.Retrieve(nonExistingDepartmentId), Times.Once);

            mockDepartmentRepository
                .Verify(c => c.Create(department), Times.Once());
        }

        [TestMethod]
        public void Save_WithValidData_ReturnsNewDepartmentWithDepartmentId()
        {
           
            //Act
            var newDepartment = sut.Save(department.DepartmentId, department);

            //Assert
            mockDepartmentRepository
             .Verify(d => d.Create(department), Times.Once);

            Assert.IsTrue(newDepartment.DepartmentId != Guid.Empty);
        }

        [TestMethod]
        public void Save_WithBlankDepartmentName_ThrowsDepartmentNameRequiredException()
        {
            // Arrange 
            department.DepartmentName = "";

            //Asert
            Assert.ThrowsException<DepartmentNameRequiredException>(
                () => sut.Save(department.DepartmentId, department));

            mockDepartmentRepository
              .Verify(c => c.Create(department), Times.Never());
        }



        [TestMethod]
        public void Save_WithValidData_ShouldCallRepositoryUpdate()
        {
            // Arrange
            department.DepartmentId = existingDepartmentId;
            //Act

            sut.Save(department.DepartmentId, department);

            //Assert
            mockDepartmentRepository
            .Verify(c => c.Retrieve(department.DepartmentId), Times.Once);

            mockDepartmentRepository
            .Verify(c => c.Update(existingDepartmentId, department), Times.Once());
        }


    }
}
