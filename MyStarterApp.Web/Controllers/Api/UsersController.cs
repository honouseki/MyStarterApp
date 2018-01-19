﻿using MyStarterApp.Models.Domain;
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
    [RoutePrefix("api/users")]
    public class UsersController : ApiController
    {
        private UserService userService = new UserService();

        [Route(), HttpGet]
        public HttpResponseMessage AdminSelectAll()
        {
            try
            {
                ItemsResponse<User> resp = new ItemsResponse<User>();
                resp.Items = userService.AdminSelectAll();
                return Request.CreateResponse(HttpStatusCode.OK, resp);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [Route("{username}"), HttpGet]
        public HttpResponseMessage SelectByUsername(string username)
        {
            try
            {
                ItemResponse<User> resp = new ItemResponse<User>();
                resp.Item = userService.SelectByUsername(username);
                return Request.CreateResponse(HttpStatusCode.OK, resp);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [Route(), HttpPost]
        public HttpResponseMessage Insert(LoginUser model)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
            try
            {
                ItemResponse<int> resp = new ItemResponse<int>();
                resp.Item = userService.Insert(model);
                return Request.CreateResponse(HttpStatusCode.OK, resp);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }
    }
}
