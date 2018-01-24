using BlastAsia.DigiBook.API.Utils;
using BlastAsia.DigiBook.Domain.Inventories;
using BlastAsia.DigiBook.Domain.Models;
using BlastAsia.DigiBook.Domain.Models.Inventories;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BlastAsia.DigiBook.API.Controllers
{
    [EnableCors("DayTwoApp")]
    [Produces("application/json")]
    //[Route("api/Inventories")]
    public class InventoriesController : Controller
    {
        public static List<Inventory> inventory = new List<Inventory>();
        private readonly IInventoryRepository inventoryRepository;
        private readonly IInventoryService inventoryService;

        public InventoriesController(IInventoryRepository inventoryRepository, IInventoryService inventoryService)
        {
            this.inventoryRepository = inventoryRepository;
            this.inventoryService = inventoryService;
        }

        [HttpGet, ActionName("GetInventoriesWithPagination")]
        [Route("api/Inventories/{page}/{record}")]
        public IActionResult GetInventoriesWithPagination(int page, int record, string filter)
        {
            var result = new PaginationClass<Inventory>();
            try
            {
                result = this.inventoryRepository.Retrieve(page, record, filter);
            }
            catch (Exception)
            {
                return BadRequest();
            }

            return Ok(result);
        }

        [HttpGet, ActionName("GetInventories")]
        [Route("api/Inventories/{id?}")]
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

        [HttpPost]
        [Route("api/Inventories")]
        public IActionResult PostInventory([FromBody] Inventory inventory)
        {
            try
            {
                if (inventory == null)
                {
                    return BadRequest();
                }

                inventory.DateCreated = DateTime.Now;
                var result = this.inventoryService.Save(Guid.Empty, inventory);
                return CreatedAtAction("GetInventories",
                    new { id = inventory.ProductId }, result);
            }

            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpDelete]
        [Route("api/Inventories/{id}")]
        public IActionResult DeleteInventory(Guid id)
        {
            var inventoryToDelete = this.inventoryRepository.Retrieve(id);
            if (inventoryToDelete == null)
            {
                return NotFound();
            }

            inventoryToDelete.IsActive = false;
            this.inventoryRepository.Update(id, inventoryToDelete);
            return Ok(inventory);

        }

        [HttpPut]
        [Route("api/Inventories/{id}")]
        public IActionResult PutInventory([FromBody] Inventory inventory, Guid id)
        {
            if (inventory == null)
            {
                return BadRequest();
            }

            var inventoryToUpdate = inventoryRepository.Retrieve(id);
            if (inventoryToUpdate == null)
            {
                return NotFound();
            }

            inventoryToUpdate.ApplyChanges(inventory);
            this.inventoryRepository.Update(id, inventoryToUpdate);
            return Ok(inventory);
        }

        [HttpPatch]
        [Route("api/Inventories/{id}")]
        public IActionResult PatchInventory([FromBody] JsonPatchDocument patchedInventory, Guid id)
        {
            if (patchedInventory == null)
            {
                return BadRequest();
            }

            var inventory = inventoryRepository.Retrieve(id);
            if (inventory == null)
            {
                return NotFound();
            }

            patchedInventory.ApplyTo(inventory);
            inventoryService.Save(id, inventory);

            return Ok(inventory);
        }
    }
}