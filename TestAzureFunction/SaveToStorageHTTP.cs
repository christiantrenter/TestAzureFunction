using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;

namespace TestAzureFunction
{
    public static class SaveToStorageHttpTrigger
    {
        [FunctionName("SaveToStorageHTTP")]
        public static async Task<IActionResult> RunAsync(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "upload/file")] HttpRequest req,
            ILogger log, ExecutionContext context)
        {
            //var formdata = await req.ReadFormAsync();

            //string name = formdata["name"];
            //string[] interests = JsonConvert.DeserializeObject<string[]>( formdata["interests"]);

            
            log.LogInformation(req.Form.Files.Count.ToString());
            
            if (req.Form.Files.Count > 0)
            {

                CloudStorageAccount storageAccount = GetCloudStorageAccount(context);
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();  
                CloudBlobContainer container = blobClient.GetContainerReference("test-container");
                CreateContainerIfNotExists(context);
                
                
                for (int i = 0; i < req.Form.Files.Count; i++)
                {
                    var file = req.Form.Files[i];
                    
                    Console.WriteLine(file.FileName);
            
                    CloudBlockBlob blob = container.GetBlockBlobReference(file.FileName);
                    blob.Properties.ContentType = file.ContentType;
            
                    await blob.UploadFromStreamAsync(file.OpenReadStream()); 
                    
                    log.LogInformation($"Blob {file.FileName} is uploaded to container {container.Name}");  
                    await blob.SetPropertiesAsync();
                }
                
                return new OkObjectResult($"File(s) added successfully to storage.");
            }
           
            return new BadRequestObjectResult("No file added.");
        }
        
        private static void CreateContainerIfNotExists(ExecutionContext executionContext)  
        {  
            CloudStorageAccount storageAccount = GetCloudStorageAccount(executionContext);  
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();  
            string[] containers = new string[] { "test-container" };  
            foreach (var item in containers)  
            {  
                CloudBlobContainer blobContainer = blobClient.GetContainerReference(item);  
                blobContainer.CreateIfNotExistsAsync();  
            }  
        }  
  
        private static CloudStorageAccount GetCloudStorageAccount(ExecutionContext executionContext)  
        {  
            var config = new ConfigurationBuilder()  
                .SetBasePath(executionContext.FunctionAppDirectory)  
                .AddJsonFile("local.settings.json", true, true)  
                .AddEnvironmentVariables().Build();  
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(config["AzureWebJobsStorage"]);  
            return storageAccount;  
        }
    }
}