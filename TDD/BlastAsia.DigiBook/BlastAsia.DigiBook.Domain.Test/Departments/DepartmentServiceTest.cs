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
        //private Department department;
        //private Mock<IDepartment> mockDepartmentRepository;
        //private 

        [TestInitialize]
        public void Initialize()
        {

        }

        [TestMethod]
        public void Save_NewDepartmentWithValidData_ShouldCallRepositoryCreate()
        {
            //// Arrange
            //Mock<IDepartmentRepository> mockDepartmentRepository = new Mock<IDepartmentRepository>();
            //var sut = new DepartmentService(mockDepartmentRepository.object);

            //var department = new Department
            //{
            //    DepartmentId = Guid.NewGuid(),
            //    DepartmentName = "HR Department",
            //    DepartmentHeadId = existingEmployeeId
            //};

            //// Act 
            //sut.Save(department);

            //// Assert
            //mockDepartmentRepository
            //    .Verify(dr => dr.Save(department).Times.Once());
        }
    }
}
