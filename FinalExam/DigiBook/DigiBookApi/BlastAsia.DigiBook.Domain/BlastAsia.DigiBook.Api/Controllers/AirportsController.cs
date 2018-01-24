using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using BlastAsia.DigiBook.Domain.Airports;
using BlastAsia.DigiBook.Domain.Models.Airports;
using BlastAsia.DigiBook.Domain.Models.Paginations;
using BlastAsia.DigiBook.Infrastructure.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BlastAsia.DigiBook.Api.Controllers
{
    [Produces("application/json")]
    public class AirportsController : Controller
    {
        string url = "https://iatacodes.org/api/v6/airports?api_key=dd6a69c4-9ebb-4df8-a0b3-dc00ad3e3ec1";

        List<Airport> response = new List<Airport>();

        //[HttpGet]
        //[Route("api/Airports/")]
        //public async Task<IActionResult> GetAirports()
        //{

        //    JsonResult result;
        //    HttpClient client = new HttpClient();
        //    HttpResponseMessage res = await client.GetAsync(url);
        //    HttpContent content = res.Content;

        //    var data = await content.ReadAsStringAsync();

        //    if (data != null)
        //    {
        //        Console.WriteLine(data);


        //        JObject joResponse = JObject.Parse(data);
        //        JArray array = (JArray)joResponse["response"];

        //        response = array.ToObject<List<Airport>>().ToList();

        //        result = new JsonResult(response);
        //    }
        //    else
        //    {
        //        result = new JsonResult("No Content");
        //    }

        //    return Ok(result.Value);
        //}

        [HttpGet]
        [Route("api/Airports/")]
        public async Task<IActionResult> GetAirports(string search)
        {

            JsonResult result;
            HttpClient client = new HttpClient();
            HttpResponseMessage res = await client.GetAsync(url);
            HttpContent content = res.Content;

            var data = await content.ReadAsStringAsync();


            if (data != null)
            {
                JObject joResponse = JObject.Parse(data);
                JArray array = (JArray)joResponse["response"];

                response = array.ToObject<List<Airport>>().ToList();

                if (search != null)
                {
                    result = new JsonResult(response.Where(r => r.Code == search || r.Name == search).ToList());
                }
                else
                {
                    result = new JsonResult(response);
                }
            }
            else
            {
                result = new JsonResult("No Content");
            }

            return Ok(result.Value);
        }
    }
}
