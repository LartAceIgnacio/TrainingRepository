﻿using System;
using System.Collections.Generic;
using BlastAsia.DigiBook.Api.Utils;
using BlastAsia.DigiBook.Domain;
using BlastAsia.DigiBook.Domain.Inventories;
using BlastAsia.DigiBook.Domain.Models.Inventories;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace BlastAsia.DigiBook.Api.Controllers
{
    [EnableCors("day2app")]
    [Produces("application/json")]
    [Route("api/Inventories")]
    public class InventoryController : Controller
    {
        private IInventoryRepository inventoryRepository;
        private IInventoryService inventoryService;

        public InventoryController(IInventoryRepository inventoryRepository, IInventoryService inventoryService)
        {
            this.inventoryRepository = inventoryRepository;
            this.inventoryService = inventoryService;
        }

        [HttpGet]
        public IActionResult GetInventories(Guid? id)
        {
            try
            {
                var result = new List<Inventory>();
                if (id == null)
                {
                    result.AddRange(this.inventoryRepository.Retreive());
                }
                else
                {
                    result.Add(this.inventoryRepository.Retrieve(id.Value));
                }
                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public IActionResult PostInventories(
            [FromBody] Inventory inventory)
        {
            try
            {
                if (inventory == null)
                {
                    return BadRequest();
                }

                var result = this.inventoryService.Save(inventory.ProductId, inventory);
                return CreatedAtAction("GetInventories",
                    new { id = inventory.ProductId }, result);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut]
        public IActionResult PutInventories(
            [FromBody] Guid id, Inventory inventory)
        {
            try
            {
                if (inventory == null)
                {
                    return BadRequest();
                }
                var inventoryToUpdate = this.inventoryRepository.Retrieve(id);
                if (inventoryToUpdate == null)
                {
                    return NotFound();
                }
                inventoryToUpdate.ApplyChanges(inventory);
                var result = this.inventoryService.Save(id, inventoryToUpdate);
                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpDelete]
        public IActionResult DeleteInventories(Guid id)
        {
            try
            {
                var found = this.inventoryRepository.Retrieve(id);
                if (found == null)
                {
                    return NotFound();
                }
                else
                {
                    this.inventoryRepository.Delete(id);
                    return NoContent();
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPatch]
        public IActionResult PatchInventories(
            [FromBody] Guid id, JsonPatchDocument patch)
        {
            try
            {
                if (patch == null)
                {
                    return BadRequest();
                }
                var inventory = this.inventoryRepository.Retrieve(id);
                if(inventory == null)
                {
                    return NotFound();
                }
                patch.ApplyTo(inventory);
                this.inventoryService.Save(inventory.ProductId, inventory);
                return Ok(inventory);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}