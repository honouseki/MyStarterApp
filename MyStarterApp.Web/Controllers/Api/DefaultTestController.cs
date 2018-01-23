using MyStarterApp.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MyStarterApp.Web.Controllers.Api
{
    [RoutePrefix("api/defaulttest")]
    public class DefaultTestController : ApiController
    {
        [HttpGet, Route("{msg}")]
        public HttpResponseMessage EchoItBack(string msg)
        {
            // Just making sure that the WEB API Controller works
            // This file is only being used for testing purposes; not part of the main application
            return Request.CreateResponse(HttpStatusCode.OK, msg);
        }

        // Testing email service...
        [Route("emailtest"), HttpGet]
        public HttpResponseMessage EmailTest()
        {
            EmailTestService emailTest = new EmailTestService();
            bool resp = emailTest.SendEmail();
            return Request.CreateResponse(HttpStatusCode.OK, resp);
        }
    }
}
