using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlastAsia.DigiBook.API.Utils;
using BlastAsia.DigiBook.Domain.Models;
using BlastAsia.DigiBook.Domain.Models.Reservations;
using BlastAsia.DigiBook.Domain.Reservations;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace BlastAsia.DigiBook.API.Controllers
{
    [EnableCors("PrimeNgDemoApp")]
    [Produces("application/json")]
    //[Route("api/Reservations")]
    public class ReservationController : Controller
    {
        private readonly IReservationService reservationService;
        private readonly IReservationRepository reservationRepository;

        public ReservationController(IReservationService reservationService, IReservationRepository reservationRepository)
        {
            this.reservationService = reservationService;
            this.reservationRepository = reservationRepository;
        }

        [HttpGet, ActionName("GetReservationsWithPagination")]
        [Route("api/Reservations/{page}/{record}")]
        public IActionResult GetReservationsWithPagination(int page, int record, string filter)
        {
            var result = new PaginationResult<Reservation>();
            try
            {
                result = this.reservationRepository.Retrieve(page, record, filter);
            }
            catch (Exception)
            {
                return BadRequest();
            }
            return Ok(result);
        }

        [HttpGet, ActionName("GetReservations")]
        [Route("api/Reservations")]
        public IActionResult GetReservations(Guid? id)
        {
            var result = new List<Reservation>();

            if (id == null)
            {
                result.AddRange(this.reservationRepository.Retrieve());
            }
            else
            {
                var reservation = this.reservationRepository.Retrieve(id.Value);
                result.Add(reservation);
            }

            return Ok(result);
        }

        [HttpPost]
        [Route("api/Reservations")]
        public IActionResult CreateReservation([FromBody] Reservation reservation)
        {
            try
            {
                if (reservation == null)
                {
                    return BadRequest();
                }

                var result = this.reservationService.Save(Guid.Empty, reservation);

                return CreatedAtAction("GetReservations", new { id = reservation.ReservationId }, reservation);
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }

        [HttpDelete]
        [Route("api/Reservations/{id}")]
        public IActionResult DeleteReservation(Guid id)
        {
            var reservationToDelete = this.reservationRepository.Retrieve(id);
            if (reservationToDelete == null)
            {
                return NotFound();
            }
            this.reservationRepository.Delete(id);
            return NoContent();
        }

        [HttpPut]
        [Route("api/Reservations/{id}")]
        public IActionResult UpdateReservation([FromBody] Reservation reservation, Guid id)
        {
            try
            {
                if (reservation == null)
                {
                    return BadRequest();
                }
                var existingReservation = reservationRepository.Retrieve(id);
                if (existingReservation  == null)
                {
                    return NotFound();
                }
                existingReservation.ApplyChanges(reservation);
                var result = this.reservationService.Save(id, existingReservation);
                return Ok(reservation);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPatch]
        [Route("api/Reservations/{id}")]
        public IActionResult PatchReservation([FromBody] JsonPatchDocument patchedReservation, Guid id)
        {
            if (patchedReservation == null)
            {
                return BadRequest();
            }
            var reservation = reservationRepository.Retrieve(id);

            if (reservation == null)
            {
                return NotFound();
            }

            patchedReservation.ApplyTo(reservation);
            reservationService.Save(id, reservation);

            return Ok(reservation);
        }
    }
}