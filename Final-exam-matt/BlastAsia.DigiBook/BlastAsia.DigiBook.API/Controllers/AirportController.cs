using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Schema;
using System.Runtime.Serialization.Json;

namespace BlastAsia.DigiBook.API.Controllers
{
    [EnableCors("PrimeNgDemoApp")]
    [Produces("application/json")]
    [Route("api/Airport")]
    public class AirportController : Controller
    {
        string baseUrl = "http://iatacodes.org/api/v6/airports?api_key=dd6a69c4-9ebb-4df8-a0b3-dc00ad3e3ec1";

        // GET: api/Airport
        [HttpGet]
        public async Task<IActionResult> GetCodesAsync()
        {
            string result;
            
            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage res = await client.GetAsync(baseUrl))
            using (HttpContent content = res.Content)
            {
                //JObject joResponse = JObject.Parse(content);
                //JObject ojObject = (JObject)joResponse["response"];
                //JArray array = (JArray)ojObject["chats"];

                result = await content.ReadAsStringAsync();
                
            }

            
            return Content(result);
        }

        [HttpGet]
        [Route("codes")]
        public async Task<JsonResult> GetCodes()
        {
            JsonResult result;
           
            var listCodes = new List<AirportResponse>();
            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage res = await client.GetAsync(baseUrl))
            using (HttpContent content = res.Content)
            {

                var data = await content.ReadAsStringAsync();
            
                if (data != null)
                {
                    Console.WriteLine(data);
                    

                    JObject joResponse = JObject.Parse(data);
                    //JObject ojObject = (JObject)joResponse["response"];
                    JArray arrResponse = (JArray)joResponse["response"];

                    var a = arrResponse.ToObject<List<AirportResponse>>();
                    listCodes.AddRange(a);
                    result = new JsonResult(listCodes);//new JsonResult(arrResponse);
                }
                else
                {
                    result = new JsonResult("No Content");
                }
            }
            return result;
        }
        

    }


    class Airport
    {
        public AirportResponse[] Response { get; set; }
    }

    class AirportResponse
    {
        public string Name { get; set; }
        public string Code { get; set; }
    }
}