using BlastAsia.DigiBook.Domain.Departments;
using BlastAsia.DigiBook.Domain.Departments.Exceptions;
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
        private Mock<IDepartmentRepository> mockDepartmentRepo;
        private Mock<IEmployeeRepository> mockEmployeeRepo;
        private DepartmentService sut;
        private Department department;

        private readonly Guid nonExistingHeadId = Guid.Empty;

        private readonly Guid existingHeadId = Guid.NewGuid();

        private readonly Guid existingDepartmentId = Guid.NewGuid();

        private readonly Guid nonExistingDepartmentId = Guid.Empty;



        [TestInitialize]
        public void Intialize()
        {
            mockDepartmentRepo = new Mock<IDepartmentRepository>();
            mockEmployeeRepo = new Mock<IEmployeeRepository>();
            sut = new DepartmentService(mockDepartmentRepo.Object, mockEmployeeRepo.Object);

            department = new Department
            {
                DepartmentId = new Guid(),
                DepartmentName = "Hr Department",
                DepartmentHeadId = new Guid()
            };

            mockEmployeeRepo
                .Setup(
                    er => er.Retrieve(existingHeadId)
                )
                .Returns(new Employee());
        }

        [TestMethod]
        public void Save_WithValidData_ShouldCallCreateRepository()
        {
            // arrange
            department.DepartmentHeadId = existingHeadId;
            // act 
            sut.Save(department.DepartmentId, department);

            // assert 
            mockDepartmentRepo
                .Verify(
                    dr => dr.Create(department), Times.Once()
                );
        }

        [TestMethod]
        public void Save_WithDepartmentNameLessThanRequiredLength_ShouldThrowDepartmentNameException()
        {
            // arrange
            department.DepartmentName = "asdas";

            // assert
            Assert.ThrowsException<InvalidDepartmentNameException>(
                    () => sut.Save(department.DepartmentId, department)
                );

            mockDepartmentRepo
                .Verify(
                    dr => dr.Create(department), Times.Never()
                );
        }
        [TestMethod]
        public void Save_WithNoExistingDepartmentHeadId_ShouldThrowInvalidDepartmentIdException()
        {
            // arrange
            department.DepartmentHeadId = nonExistingHeadId;

            mockEmployeeRepo
                .Setup(
                    er => er.Retrieve(nonExistingHeadId)
                )
                .Returns<Employee>(null);
            // act 

            // assert
            Assert.ThrowsException<InvalidDepartmentIdException>(
                  () => sut.Save(department.DepartmentId, department)
              );

            mockEmployeeRepo
                .Verify(
                    er => er.Retrieve(nonExistingHeadId), Times.Once()
                );

            mockDepartmentRepo
              .Verify(
                  dr => dr.Create(department), Times.Never()
              );
        }
        [TestMethod]
        public void Save_WithExistingDepartmentId_ShouldCallRepositoryUpdate()
        {
            // arrange 
            department.DepartmentHeadId = existingHeadId;
            department.DepartmentId = existingDepartmentId;

            mockDepartmentRepo
                .Setup(
                    dr => dr.Retrieve(existingDepartmentId)
                ).
                Returns(new Department());

            // act
            sut.Save(department.DepartmentId, department);
            // assert 
            mockDepartmentRepo
                .Verify(
                    dr => dr.Retrieve(existingDepartmentId), Times.Once()
                );

            mockDepartmentRepo
                .Verify(
                    dr => dr.Update(existingDepartmentId, department), Times.Once()
                );
        }

        [TestMethod]
        public void Save_WithNonExistingDepartmentId_ShouldCallRepositoryCreate()
        {
            // arrange 
            department.DepartmentHeadId = existingHeadId;
            department.DepartmentId = nonExistingDepartmentId;

            mockDepartmentRepo
                .Setup(
                    dr => dr.Retrieve(nonExistingDepartmentId)
                ).
                Returns <Department>(null);

            // act
            sut.Save(department.DepartmentId, department);
            // assert 
            mockDepartmentRepo
                .Verify(
                    dr => dr.Retrieve(nonExistingDepartmentId), Times.Once()
                );

            mockDepartmentRepo
                .Verify(
                    dr => dr.Create(department), Times.Once()
                );
        }
    }
}
