using System;
using System.Collections.Generic;
using BlastAsia.DigiBook.API.Utils;
using BlastAsia.DigiBook.Domain.Luigis;
using BlastAsia.DigiBook.Domain.Models;
using BlastAsia.DigiBook.Domain.Models.Luigis;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace BlastAsia.DigiBook.API.Controllers
{
    [EnableCors("DemoApp")]
    [Produces("application/json")]
    //[Route("api/Luigis")]
    public class LuigiController : Controller
    {
        private static List<Luigi> luigis = new List<Luigi>();
        private readonly ILuigiRepository luigiRepository;
        private readonly ILuigiService luigiService;

        public LuigiController(ILuigiRepository luigiRepository, ILuigiService luigiService)
        {
            this.luigiRepository = luigiRepository;
            this.luigiService = luigiService;
        }

        [HttpGet, ActionName("GetLuigisWithPagination")]
        [Route("api/Luigis/{page}/{record}")]
        public IActionResult GetLuigisWithPagination(int page, int record, string filter)
        {
            var result = new Pagination<Luigi>();
            try
            {
                result = this.luigiRepository.Retrieve(page, record, filter);
            }
            catch (Exception)
            {
                return BadRequest();
            }

            return Ok(result);
        }

        [HttpGet, ActionName("GetLuigis")]
        [Route("api/Luigis")]
        public IActionResult GetLuigis(Guid? id)
        {
            var result = new List<Luigi>();
            if (id == null)
            {
                result.AddRange(this.luigiRepository.Retrieve());
            }
            else
            {
                var luigi = this.luigiRepository.Retrieve(id.Value);
                result.Add(luigi);
            }
            return Ok(result);
        }

        [HttpPost]
        [Route("api/Luigis")]
        public IActionResult CreateLuigi([FromBody]Luigi luigi)
        {
            try
            {
                if (luigi == null)
                {
                    return BadRequest();
                }
                var result = this.luigiService.Save(Guid.Empty, luigi);

                return CreatedAtAction("GetLuigis", new { id = luigi.LuigiId }, result);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpDelete]
        [Route("api/Luigis/{id}")]
        public IActionResult DeleteLuigi(Guid id)
        {
            var deletedLuigi = luigiRepository.Retrieve(id);
            if (deletedLuigi == null)
            {
                return NotFound();
            }

            this.luigiRepository.Delete(id);

            return Ok();
        }
        [HttpPut]
        [Route("api/Luigis/{id}")]
        public IActionResult UpdateLuigi(
            [FromBody] Luigi updatedLuigi, Guid id)
        {
            var luigi = luigiRepository.Retrieve(id);
            if (luigi == null)
            {
                return BadRequest();
            }

            luigi.ApplyChanges(updatedLuigi);
            luigiService.Save(id, luigi);
            return Ok(luigi);

        }

        [HttpPatch]
        [Route("api/Luigis")]
        public IActionResult PatchLuigi(
            [FromBody]JsonPatchDocument patchedLuigi, Guid id)
        {
            if (patchedLuigi == null)
            {
                return BadRequest();
            }

            var luigi = luigiRepository.Retrieve(id);
            if (luigi == null)
            {
                return NotFound();
            }

            patchedLuigi.ApplyTo(luigi);
            luigiService.Save(id, luigi);

            return Ok(luigi);
        }
    }
}