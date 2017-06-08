using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System.IO;

namespace hyundaiPlastic.Controllers
{
    public class BlobsController : Controller
    {
        // GET: Blobs
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateBlobContainer()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("hyundaiplastic_AzureStorageConnectionString"));
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("hyundaiplastic");
            ViewBag.Success = container.CreateIfNotExists();
            ViewBag.BlobContainerName = container.Name;
            return View();
        }

        public EmptyResult UploadBlob()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("hyundaiplastic_AzureStorageConnectionString"));
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("hyundaiplastic");
            CloudBlockBlob blob = container.GetBlockBlobReference("blobexample");
            
            using (var fileStream = System.IO.File.OpenRead(@"C:\Project\IMG_8332.jpg"))
            {
                blob.UploadFromStream(fileStream);
            }
            return new EmptyResult();
        }
        public ActionResult ListBlobs()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("hyundaiplastic_AzureStorageConnectionString"));
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("hyundaiplastic");

            List<string> blobs = new List<string>();
            foreach(IListBlobItem item in container.ListBlobs(useFlatBlobListing:true))
            {
                if (item.GetType() == typeof(CloudBlockBlob))
                {
                    CloudBlockBlob blob = (CloudBlockBlob)item;
                    
                    blobs.Add(blob.Uri.ToString());
                }
                else if (item.GetType() == typeof(CloudPageBlob))
                {
                    CloudPageBlob blob = (CloudPageBlob)item;
                    blobs.Add(blob.Name);
                }
                else if (item.GetType() == typeof(CloudBlobDirectory))
                {
                    CloudBlobDirectory dir = (CloudBlobDirectory)item;
                    blobs.Add(dir.Uri.ToString());
                }
            }
            return View(blobs);
        }
        public EmptyResult DownloadBlob()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("hyundaiplastic_AzureStorageConnectionString"));
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("hyundaiplastic");
            CloudBlockBlob blob = container.GetBlockBlobReference("IMG_8316.jpg");
            using (var fileStream = System.IO.File.OpenWrite(@"C:\Project\downloadedImage.jpg"))
            {
                blob.DownloadToStream(fileStream);
            }
            return new EmptyResult();
        }
    }
}