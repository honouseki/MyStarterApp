using MyStarterApp.Models;
using MyStarterApp.Models.Domain;
using MyStarterApp.Models.Interfaces;
using MyStarterApp.Models.Responses;
using MyStarterApp.Models.ViewModels;
using MyStarterApp.Services.Interfaces;
using MyStarterApp.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MyStarterApp.Web.Controllers.Api
{
    // This API Controller contains endpoints to user information/interaction,
    //     as well as login/logout/getCurrentUser functions
    //     As best practice, you may want to separate those functions as needed when creating a new application
    [RoutePrefix("api/users")]
    public class UsersController : ApiController
    {
        // Implementing interfaces (with Unity)
        // Make sure to register interfaces in UnityConfig in order to use
        private IUserService _userService;
        private IAuthenticationService _authService;
        public UsersController(IUserService userService, IAuthenticationService authService)
        {
            _userService = userService;
            _authService = authService;
        }

        // Select all users, with their information
        [Route(), HttpGet]
        public HttpResponseMessage AdminSelectAll()
        {
            try
            {
                ItemsResponse<User> resp = new ItemsResponse<User>();
                resp.Items = _userService.AdminSelectAll();
                return Request.CreateResponse(HttpStatusCode.OK, resp);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        // Select a specific user with his/her information
        [Route("{username}"), HttpGet]
        public HttpResponseMessage SelectByUsername(string username)
        {
            try
            {
                ItemResponse<User> resp = new ItemResponse<User>();
                resp.Item = _userService.SelectByUsername(username);
                return Request.CreateResponse(HttpStatusCode.OK, resp);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        // Registering a new user
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
                resp.Item = _userService.Insert(model);
                return Request.CreateResponse(HttpStatusCode.OK, resp);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        // Logging in a user
        [Route("login/{remember}"), HttpPost]
        public HttpResponseMessage Login(LoginUser model, bool remember)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
            try
            {
                ItemResponse<LoginType> resp = new ItemResponse<LoginType>();
                resp.Item = _userService.Login(model, remember);
                return Request.CreateResponse(HttpStatusCode.OK, resp);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }
        
        // Logging out a user
        [Route("logout"), HttpGet]
        public HttpResponseMessage Logout()
        {
            _authService.LogOut();
            SuccessResponse resp = new SuccessResponse();
            return Request.CreateResponse(HttpStatusCode.OK, resp);
        }

        // Get current user's information
        [Route("getcurrentuser"), HttpGet]
        public HttpResponseMessage GetCurrentUser()
        {
            try
            {
                IUserAuthData model = _authService.GetCurrentUser();
                ItemResponse<User> resp = new ItemResponse<User>();
                if (model != null)
                {
                    resp.Item = _userService.AdminSelectById(model.UserId);
                }
                return Request.CreateResponse(HttpStatusCode.OK, resp);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        // Checks if username exists; returns 1 if true, 0 if false
        [Route("checkusername/{username}"), HttpGet]
        public HttpResponseMessage CheckUsername(string username)
        {
            try
            {
                ItemResponse<int> resp = new ItemResponse<int>();
                resp.Item = _userService.CheckUsername(username);
                return Request.CreateResponse(HttpStatusCode.OK, resp);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        // Checks if email exists; returns 1 if true, 0 if false
        [Route("checkemail/{email}/"), HttpGet]
        public HttpResponseMessage CheckEmail(string email)
        {
            try
            {
                ItemResponse<int> resp = new ItemResponse<int>();
                resp.Item = _userService.CheckEmail(email);
                return Request.CreateResponse(HttpStatusCode.OK, resp);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }
    }
}
