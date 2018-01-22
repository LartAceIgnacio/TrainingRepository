using System;
using System.Collections.Generic;
using BlastAsia.DigiBook.Api.Utils;
using BlastAsia.DigiBook.Domain.Models.Names;
using BlastAsia.DigiBook.Domain.Names;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace BlastAsia.DigiBook.Api.Controllers
{
    public class NameController : Controller
    {
        private INameRepository nameRepository;
        private INameService nameService;

        public NameController(INameRepository nameRepository, INameService nameService)
        {
            this.nameRepository = nameRepository;
            this.nameService = nameService;
        }

        [HttpGet]
        public IActionResult GetNames(Guid? id)
        {
            try
            {
                var result = new List<Name>();
                if (id == null)
                {
                    result.AddRange(this.nameRepository.Retreive());

                }
                else
                {
                    var name = this.nameRepository.Retrieve(id.Value);
                    result.Add(name);

                }
                return Ok(result);

            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public IActionResult CreateNames(
            [FromBody] Name name)
        {
            try
            {
                if (name == null)
                {
                    return BadRequest();
                }
                else
                {
                    var result = this.nameService.Save(Guid.Empty, name);

                    return CreatedAtAction("GetNames",
                        new { id = name.NameId }, result);
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpDelete]
        public IActionResult DeleteNames(Guid id)
        {
            try
            {
                var found = this.nameRepository.Retrieve(id);
                if (found == null)
                {
                    return BadRequest();
                }
                else
                {
                    this.nameRepository.Delete(id);
                    return NoContent();
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPut]
        public IActionResult UpdateNames(
            [FromBody] Guid id, Name name)
        {
            try
            {

                if (name == null)
                {
                    return BadRequest();
                }
                var nameToUpdate = this.nameRepository.Retrieve(id);
                if (nameToUpdate == null)
                {
                    return NotFound();
                }
                nameToUpdate.ApplyChanges(name);
                var result = this.nameService.Save(id, nameToUpdate);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPatch]
        public IActionResult PatchUpdate(
            [FromBody] Guid id, JsonPatchDocument patch)
        {
            try
            {
                if(patch == null)
                {
                    return BadRequest();
                }
                var name = this.nameRepository.Retrieve(id);
                if(name == null)
                {
                    return NotFound();
                }
                patch.ApplyTo(name);
                this.nameService.Save(id, name);
                return Ok(name);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}