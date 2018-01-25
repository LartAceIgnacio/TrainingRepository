using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using BlastAsia.DigiBook.Domain.Models.Airports;

namespace BlastAsia.DigiBook.API.Controllers
{
    [EnableCors("PrimeNgDemoApp")]
    [Produces("application/json")]
    [Route("api/Airport")]
    public class AirportController : Controller
    {
        private List<AirportResponse> listCodes = new List<AirportResponse>();
        private int PAGE_SIZE = 10;
        private string baseUrl = "https://iatacodes.org/api/v6/airports?api_key=dd6a69c4-9ebb-4df8-a0b3-dc00ad3e3ec1";

        private IUrlHelperFactory urlHelperFactory;
        private IActionContextAccessor actionAccessor;


        public AirportController(/*IUrlHelperFactory urlHelperFactory, IActionContextAccessor actionAccessor*/)
        {
            /*this.urlHelperFactory = urlHelperFactory;
            this.actionAccessor = actionAccessor;*/
        }

        // GET: api/Airport
        [HttpGet]
        public async Task<IActionResult> GetCodesAsync(string url)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                // iatacode code params
                //using (HttpResponseMessage res = await client.GetAsync(string.Concat(baseUrl, "&code=", code)))
                using (HttpResponseMessage res = await client.GetAsync(url))
                using (HttpContent content = res.Content)
                {

                    var data = await content.ReadAsStringAsync();

                    if (data != null)
                    {

                        JObject joResponse = JObject.Parse(data);
                        JArray arrResponse = (JArray)joResponse["response"];

                        var response = arrResponse.ToObject<List<AirportResponse>>();
                        listCodes.AddRange(response);

                    }
                    else
                    {
                        return NoContent();
                    }

                }
            }
            catch (Exception)
            {

                return BadRequest();
            }


            return Ok(listCodes);
        }


        //[HttpGet]
        //[Route("codes")]
        // returns JsonResult
        //public async Task<JsonResult> GetCodes()
        //{
        //    JsonResult result;

        //    using (HttpClient client = new HttpClient())
        //    using (HttpResponseMessage res = await client.GetAsync(baseUrl))
        //    using (HttpContent content = res.Content)
        //    {

        //        var data = await content.ReadAsStringAsync();

        //        if (data != null)
        //        {
        //            Console.WriteLine(data);


        //            JObject joResponse = JObject.Parse(data);
        //            //JObject ojObject = (JObject)joResponse["response"];
        //            JArray arrResponse = (JArray)joResponse["response"];

        //            var a = arrResponse.ToObject<List<AirportResponse>>();
        //            listCodes.AddRange(a);
        //            result = new JsonResult(listCodes);
        //        }
        //        else
        //        {
        //            result = new JsonResult("No Content");
        //        }
        //    }
        //    return result;
        //}

        #region Object Airport Pagination API for testing in different route
        //[Route("paginate"), ActionName("PagingAirport")]
        //public object Get(int page = 0)
        //{

        //    var baseQuery = listCodes.OrderBy(o => o.Name);

        //    var PAGE_COUNT = baseQuery.Count();

        //    var TOTAL_PAGES = Math.Ceiling((double)PAGE_COUNT / PAGE_SIZE);

        //    var helper = this.urlHelperFactory.GetUrlHelper(this.actionAccessor.ActionContext);

        //    //var urlHelper = this.HttpContext.RequestServices.GetRequiredService<IUrlHelper>();

        //    var prevUrl = page > 0 ? helper.Action("PagingAirport", "Airport", new { page = page - 1 }) : "";
        //    var nextUrl = page < TOTAL_PAGES - 1 ? helper.Action("PagingAirport", "Airport", new { page = page + 1 }) : "";


        //    var results = baseQuery.Skip(PAGE_SIZE * page)
        //                           .Take(PAGE_SIZE)
        //                           .ToList();
        //    return new
        //    {
        //        TotalCount = PAGE_COUNT,
        //        TotalPage = TOTAL_PAGES,
        //        PrevPageUrl = prevUrl,
        //        NextPageUrl = nextUrl,
        //        Results = results
        //    };
        //}
        #endregion


    }



}