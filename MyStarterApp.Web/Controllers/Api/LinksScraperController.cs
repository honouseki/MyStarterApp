using MyStarterApp.Models.Domain;
using MyStarterApp.Models.Responses;
using MyStarterApp.Models.ViewModels;
using MyStarterApp.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MyStarterApp.Web.Controllers.Api
{
    [RoutePrefix("api/linksscraper")]
    public class LinksScraperController : ApiController
    {
        // Where all the link scraping goes! Each model will be its own depending on what site/page is being scraped!
        // Note: Example scrape is extremely basic; receives only 4 pieces of data
        ILinksScraperService linksScraperService;

        public LinksScraperController(ILinksScraperService _linksScraperService)
        {
            linksScraperService = _linksScraperService;
        }

        [Route("deviantart"), HttpPost]
        public HttpResponseMessage GetDeviantArt(Url url)
        {
            try
            {
                ItemResponse<LinkDeviantArt> resp = new ItemResponse<LinkDeviantArt>();
                resp.Item = linksScraperService.GetDeviantArt(url);
                return Request.CreateResponse(HttpStatusCode.OK, resp);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }

        }
    }
}