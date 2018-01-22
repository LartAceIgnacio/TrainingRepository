using BlastAsia.DigiBook.API.Utils;
using BlastAsia.DigiBook.Domain.Inventories;
using BlastAsia.DigiBook.Domain.Models.Inventories;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace BlastAsia.DigiBook.API.Controllers
{
    [EnableCors("DayTwoApp")]
    [Produces("application/json")]
    [Route("api/Inventories")]
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

        [HttpPost]
        public IActionResult PostInventory([FromBody] Inventory inventory)
        {
            try
            {
                if (inventory == null)
                {
                    return BadRequest();
                }

                var result = this.inventoryService.Save(Guid.Empty, inventory);

                return CreatedAtAction("GetLocations",
                    new { id = inventory.ProductId }, result);
            }

            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpDelete]
        public IActionResult DeleteInventory(Guid id)
        {
            var inventoryToDelete = this.inventoryRepository.Retrieve(id);
            if (inventoryToDelete == null)
            {
                return NotFound();
            }

            inventoryRepository.Delete(id);
            return NoContent();
        }

        [HttpPut]
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