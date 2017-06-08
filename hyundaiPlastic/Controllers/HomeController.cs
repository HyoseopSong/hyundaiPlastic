using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace hyundaiPlastic.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "";

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("hyundaiplastic_AzureStorageConnectionString"));
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("product");

            IDictionary<string, string> blobs = new Dictionary<string, string>();
            foreach (IListBlobItem item in container.ListBlobs(useFlatBlobListing: true))
            {
                if (item.GetType() == typeof(CloudBlockBlob))
                {
                    CloudBlockBlob blob = (CloudBlockBlob)item;
                    string[] temp = blob.Name.Split('.');
                    blobs.Add(temp[1], blob.Uri.ToString());
                }
                else if (item.GetType() == typeof(CloudPageBlob))
                {
                    CloudPageBlob blob = (CloudPageBlob)item;
                    blobs.Add(blob.Name, blob.Uri.ToString());
                }
                //else if (item.GetType() == typeof(CloudBlobDirectory))
                //{
                //    CloudBlobDirectory dir = (CloudBlobDirectory)item;
                //    blobs.Add(dir.Uri.ToString());
                //}
            }

            return View(blobs);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "";

            return View();
        }
    }
}