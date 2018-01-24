using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using BlastAsia.DigiBook.Domain.Models.Airports;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BlastAsia.DigiBook.API.Controllers
{
    [EnableCors("DigiBook-web")]
    [Produces("application/json")]
    [Route("api/Airport")]
    public class AirportController : Controller
    {
        [HttpGet, ActionName("GetAirports")]
        public async Task<IActionResult> GetAirports()
        {
            HttpClient http = new HttpClient();
            string url = "https://iatacodes.org/api/v6/airports?api_key=dd6a69c4-9ebb-4df8-a0b3-dc00ad3e3ec1";

            List<Airport> list = new List<Airport>();

            try
            {
                var stringTask = await http.GetStringAsync(url);

                if (stringTask != null)
                {
                    var stringResult = JObject.Parse(stringTask);
                    JArray arrResponse = (JArray)stringResult["response"];
                    list = arrResponse.ToObject<List<Airport>>();
                    return Ok(list);
                }

                return NoContent();
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}