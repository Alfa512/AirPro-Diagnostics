using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AirPro.Common.Enumerations;
using AirPro.Service;
using AirPro.Service.DTOs.Interface;
using AirPro.Site.Models.Uploads;
using AirPro.Site.Results;
using Microsoft.WindowsAzure.Storage;
using UniMatrix.Common.Extensions;

namespace AirPro.Site.Controllers
{
    [Authorize]
    public class UploadController : Controller
    {
        private ServiceFactory _factory;
        private ServiceFactory Factory => _factory ?? (_factory = new ServiceFactory(MvcApplication.ConnectionString, User.Identity));

        private string StorageAccount { get; } = ConfigurationManager.ConnectionStrings["AzureWebJobsStorage"].ConnectionString;

        public ActionResult UploadControl(UploadType type, string key, string title)
        {
            var model = new UploadControlViewModel
            {
                Type = (int)type,
                Key = key
            };

            if (!string.IsNullOrEmpty(title))
                model.Title = title;

            return PartialView("_UploadControl", model);
        }

        [HttpPost]
        public ActionResult GetUploadsByPage(int current, int rowCount, string searchPhrase, UploadType type, string key)
        {
            // Load Arguments.
            var args = new ServiceArgs();
            args.AddGridOptions(current, rowCount, Request.Form.GetDynamicSortString(), searchPhrase);
            args.Add("UploadTypeId", (int)type);
            args.Add("UploadKey", key);

            var result = Factory.GetAllByGridPage<IUploadDto>(args);

            return new JsonCamelCaseResult(result, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            return Factory.Delete<IUploadDto>(id.ToString()) 
                ? new HttpStatusCodeResult(HttpStatusCode.OK, "File Deleted.")
                : new HttpStatusCodeResult(HttpStatusCode.BadRequest, "File Delete Failed.");
        }

        [HttpPost]
        public ActionResult Save(HttpPostedFileBase file, UploadType type, string key)
        {
            try
            {
                // Check File.
                if (file != null && file.ContentLength > 0)
                {
                    // Check Connection String.
                    if (CloudStorageAccount.TryParse(StorageAccount, out var storageAccount))
                    {
                        // Create Client.
                        var cloudBlobClient = storageAccount.CreateCloudBlobClient();

                        // Create Upload Container.
                        var cloudBlobContainer = cloudBlobClient.GetContainerReference("uploads");
                        cloudBlobContainer.CreateIfNotExists();

                        // Get File Hash and Mime.
                        var hash = file.InputStream.ToMD5HashString();
                        var mime = MimeMapping.GetMimeMapping(file.FileName);

                        // Save File Stream to Blob.
                        var cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference($"{hash}");
                        if (!cloudBlockBlob.Exists())
                        {
                            file.InputStream.Seek(0, SeekOrigin.Begin);
                            cloudBlockBlob.UploadFromStream(file.InputStream);
                            cloudBlockBlob.Properties.ContentType = mime;
                            cloudBlockBlob.SetProperties();
                        }

                        // Split File Name.
                        var parts = file.FileName.Split('.');
                        var fileName = parts.Length > 2 ? string.Join(".", parts.Take(parts.Length - 1)) : parts[0];
                        var fileExtension = parts.Length > 1 ? parts[parts.Length - 1] : null;

                        // Create Upload.
                        var upload = new UploadViewModel
                        {
                            UploadKey = key,
                            UploadTypeId = type,
                            UploadStorageName = hash,
                            UploadFileSizeBytes = file.InputStream.Length,
                            UploadFileName = fileName,
                            UploadFileExtension = fileExtension,
                            UploadMimeType = mime
                        };

                        // Save File Upload.
                        var result = Factory.Save((IUploadDto) upload);
                        if (result?.UpdateResult?.Success ?? false)
                            return new JsonCamelCaseResult(result, JsonRequestBehavior.DenyGet);
                    }
                }
            }
            catch (Exception e)
            {
                Logging.Logger.LogException(e);
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "File Upload Failed.");
        }
    }
}