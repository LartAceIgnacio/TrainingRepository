using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace BlastAsia.DigiBook.Domain.Test.Inventories
{

    [TestClass]
    public class InventoryServiceTest
    {

        [TestInitialize]
        public void Initialize()
        {

        }

        [TestCleanup]
        public void Cleanup()
        {

        }

        [TestMethod]
        public void Save_WithValidData_ShouldReturnInventoryId()
        {
            // arrange
        //    var inventory = new Inventory() {
        //        ProductId = Guid.NewGuid(),
        //        ProductCode = "asdf1234",
        //        ProductName = "Vape",
        //        ProductDescription = "Vape on!",
        //        QuantityOnHand = 10,
        //        QuantityReserved = 10,
        //        QuantityOrdered = 10,
        //        DateCreated = DateTime.Now,
        //        DateModified = DateTime.Now,
        //        IsActive = false,
        //        Bin = "01B1Z"
        //    };

        //    var mockRepo = new Mock<IInventoryRepository>();

        //    var sut = new InventoryService(mockRepo.Object);
            
        //    // act 

        //    // assert

        //    //var regex = @"0[1-5][Bb][1-9][A-Za-z]{1}$";
        //    //var result = Regex.IsMatch("01b1Z", regex);
        //    //Assert.IsTrue(result);
        //}

    }
}
