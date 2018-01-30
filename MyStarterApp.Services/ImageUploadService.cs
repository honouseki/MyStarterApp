using MyStarterApp.Data;
using MyStarterApp.Models.Domain;
using MyStarterApp.Models.ViewModels;
using MyStarterApp.Services.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MyStarterApp.Services
{
    public class ImageUploadService : BaseService, IImageUploadService
    {
        // Inserting new image file
        public int Insert(ImageFile model)
        {
            int id = 0;
            string systemFileName = string.Empty;

            // To save the image file/set location
            if (model.ByteArray != null)
            {
                // Setting up system file name
                systemFileName = string.Format("{0}_{1}{2}",
                    model.ImageFileName,
                    Guid.NewGuid().ToString(),
                    model.FileExtension
                );
                // Save to local drive
                SaveToDrive(model.Location, systemFileName, model.ByteArray);
            }
            // If the ByteArray is empty, checks if image upload is via URL
            // Converts that URL to image, then saves to local drive
            else if (model.ImageUrl != null)
            {
                // Grabs image extension from url
                string dlUrl = model.ImageUrl.Split('?')[0];
                string ext = Path.GetExtension(dlUrl);

                // Downloads image url bytes
                WebClient webClient = new WebClient();
                byte[] imgBytes = webClient.DownloadData(model.ImageUrl);
                webClient.Dispose();

                // Setting up system file name
                systemFileName = string.Format("{0}_{1}{2}",
                    model.ImageFileName,
                    Guid.NewGuid().ToString(),
                    ext
                );
                // Save to local drive
                SaveToDrive(model.Location, systemFileName, imgBytes);
            }

            // To save image file information in database
            model.SystemFileName = systemFileName;
            this.DataProvider.ExecuteNonQuery(
                "ImageFiles_Insert",
                inputParamMapper: delegate(SqlParameterCollection paramCol)
                {
                    SqlParameter paramId = new SqlParameter();
                    paramId.ParameterName = "@id";
                    paramId.SqlDbType = SqlDbType.Int;
                    paramId.Direction = ParameterDirection.Output;
                    paramCol.Add(paramId);

                    paramCol.AddWithValue("@imageFileName", model.ImageFileName);
                    paramCol.AddWithValue("@systemFileName", model.SystemFileName);
                    paramCol.AddWithValue("@imageFileType", model.ImageFileType);
                    paramCol.AddWithValue("@location", model.Location);
                    paramCol.AddWithValue("@modifiedBy", model.ModifiedBy);
                },
                returnParameters: delegate(SqlParameterCollection paramCol)
                {
                    id = (int)paramCol["@id"].Value;
                }
            );

            return id;
        }

        // Selecting all images
        public List<BasicImage> SelectAll()
        {
            List<BasicImage> result = new List<BasicImage>();

            this.DataProvider.ExecuteCmd(
                "ImageFiles_SelectAll",
                inputParamMapper: null,
                singleRecordMapper: delegate(IDataReader reader, short set)
                {
                    BasicImage model = BasicImageMapper(reader);
                    result.Add(model);
                }
            );

            return result;
        }

        // Select image by id
        public BasicImage SelectById(int id)
        {
            BasicImage model = new BasicImage();
            this.DataProvider.ExecuteCmd(
                "ImageFiles_SelectById",
                inputParamMapper: delegate(SqlParameterCollection paramCol)
                {
                    paramCol.AddWithValue("@id", id);
                },
                singleRecordMapper: delegate(IDataReader reader, short set)
                {
                    model = BasicImageMapper(reader);
                }
            );
            return model;
        }

        // Update specified image
        // Update for an image would just be a combination of insert/delete
        // Process:
        //     Extract first half method of Insert to reuse (where system file name / bytes are formed)
        //     Use that to convert image and upload to drive
        //     Having passed previous system file name, use that to delete the old image from drive
        //     Update systemFileName in model and send that to update with update stored proc
        // When creating this service, do not forget to add the same method signature to the interface

        // Delete specified image
        public void Delete(BasicImage model)
        {
            // Deletes image from drive
            DeleteFromDrive(model.SystemFileName);
            // Deletes image from database
            this.DataProvider.ExecuteNonQuery(
                "ImageFiles_Delete",
                inputParamMapper: delegate (SqlParameterCollection paramCol)
                {
                    paramCol.AddWithValue("@id", model.Id);
                }
            );
        }

        // Private functions to be used by the above functions
        private void SaveToDrive(string location, string systemFileName, byte[] bytes)
        {
            string fileBase = String.Format("~/images/{0}/{1}", location, systemFileName);
            var filePath = HttpContext.Current.Server.MapPath(fileBase);
            File.WriteAllBytes(filePath, bytes);
        }
        private void DeleteFromDrive(string fileName)
        {
            string deletePath = HttpContext.Current.Server.MapPath("~/images/general/" + fileName);
            File.Delete(deletePath);
        }
        private static BasicImage BasicImageMapper(IDataReader reader)
        {
            BasicImage model = new BasicImage();
            int index = 0;
            model.Id = reader.GetSafeInt32(index++);
            model.ImageFileName = reader.GetSafeString(index++);
            model.SystemFileName = reader.GetSafeString(index++);
            model.ImageFileType = reader.GetSafeInt32(index++);
            model.Location = reader.GetSafeString(index++);
            model.CreatedDate = reader.GetSafeDateTime(index++);
            model.ModifiedDate = reader.GetSafeDateTime(index++);
            model.ModifiedBy = reader.GetSafeString(index++);
            return model;
        }
    }
}
