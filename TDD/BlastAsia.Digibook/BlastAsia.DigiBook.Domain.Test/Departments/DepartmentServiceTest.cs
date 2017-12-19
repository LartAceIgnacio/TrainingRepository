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
        private Mock<IDepartmentRepository> mockDepartmentRepo;
        private DepartmentService sut;
        private Department department;

        [TestInitialize]
        public void Intialize()
        {
            mockDepartmentRepo = new Mock<IDepartmentRepository>();
            sut = new DepartmentService(mockDepartmentRepo.Object);

            department = new Department
            {
                DepartmentId = new Guid(),
                DepartmentName = "Hr Department",
                DepartmentHeadId = new Guid()
            };
        }

        [TestMethod]
        public void Save_WithValidData_ShouldCallCreateRepository()
        {
            // arrange
            // act 
            sut.Save(department);

            // assert 
            mockDepartmentRepo
                .Verify(
                    dr => dr.Create(department), Times.Once()
                );
        }

    }
}
