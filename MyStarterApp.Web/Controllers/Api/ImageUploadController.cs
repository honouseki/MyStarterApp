using MyStarterApp.Models.Domain;
using MyStarterApp.Models.Interfaces;
using MyStarterApp.Models.Responses;
using MyStarterApp.Models.ViewModels;
using MyStarterApp.Services;
using MyStarterApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MyStarterApp.Web.Controllers.Api
{
    [RoutePrefix("api/images")]
    public class ImageUploadController : ApiController
    {
        IImageUploadService imageUploadService;
        IAuthenticationService authService;

        public ImageUploadController(IImageUploadService _imageUploadService, IAuthenticationService _authService)
        {
            imageUploadService = _imageUploadService;
            authService = _authService;
        }

        [Route(), HttpPost]
        public HttpResponseMessage Insert(ImageFile model)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
            try
            {
                // ModifiedBy is set to current user
                IUserAuthData user = authService.GetCurrentUser();
                model.ModifiedBy = String.Format("{0}_{1}", user.Username, user.UserId.ToString());
                // Setting other required model properties here; May set as fit, i.e. based on 'category'
                // The name attached to the beginning of the file name
                model.ImageFileName = "generalImage";
                // File type is custom; if uploading images based on category/type, this can be important!
                model.ImageFileType = 1;
                // File location under images; make sure it matches!
                model.Location = "general";

                // If the model contains non-null EncodedImageFile and FileExtension properties, convert to bytes
                if (model.EncodedImageFile != null && model.FileExtension != null)
                {
                    byte[] newBytes = Convert.FromBase64String(model.EncodedImageFile);
                    model.ByteArray = newBytes;
                }

                // Uploads image
                ItemResponse<int> resp = new ItemResponse<int>();
                resp.Item = imageUploadService.Insert(model);
                return Request.CreateResponse(HttpStatusCode.OK, resp);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [Route(), HttpGet]
        public HttpResponseMessage SelectAll()
        {
            try
            {
                ItemsResponse<BasicImage> resp = new ItemsResponse<BasicImage>();
                resp.Items = imageUploadService.SelectAll();
                return Request.CreateResponse(HttpStatusCode.OK, resp);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [Route("{id:int}"), HttpGet]
        public HttpResponseMessage SelectById(int id)
        {
            try
            {
                ItemResponse<BasicImage> resp = new ItemResponse<BasicImage>();
                resp.Item = imageUploadService.SelectById(id);
                return Request.CreateResponse(HttpStatusCode.OK, resp);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [Route("delete"), HttpDelete]
        public HttpResponseMessage Delete(BasicImage model)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
            try
            {
                imageUploadService.Delete(model);
                SuccessResponse resp = new SuccessResponse();
                return Request.CreateResponse(HttpStatusCode.OK, resp);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }
    }
}
