using BlastAsia.DigiBook.Domain.Inventories;
using BlastAsia.DigiBook.Domain.Models.Inventories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace BlastAsia.DigiBook.API.Controllers
{
    public class InventoriesController : Controller
    {
        public static List<Inventory> employee = new List<Inventory>();
        private readonly IInventoryRepository inventoryRepository;
        private readonly IInventoryService inventoryService;

        public InventoriesController(IInventoryRepository inventoryRepository, IInventoryService inventoryService)
        {
            this.inventoryRepository = inventoryRepository;
            this.inventoryService = inventoryService;
        }

        [HttpGet, ActionName("GetInventories")]
        public IActionResult GetInventories(Guid? id)
        {
            var result = new List<Inventory>();
            if (id == null)
            {
                result.AddRange(this.inventoryRepository.Retrieve());
            }
            else
            {
                var inventory = this.inventoryRepository.Retrieve(id.Value);
                result.Add(inventory);
            }

            return Ok(result);
        }
    }
}