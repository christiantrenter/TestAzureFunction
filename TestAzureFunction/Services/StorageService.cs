using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace TestAzureFunction.Services
{
    public class StorageService : IStorageService
    {
        private readonly IConfigurationRoot _config = new ConfigurationBuilder()  
            .SetBasePath(Environment.CurrentDirectory)  
            .AddJsonFile("local.settings.json", true, true)  
            .AddEnvironmentVariables().Build();
        
        private CloudBlockBlob _blob;
        private readonly CloudBlobContainer _container;
        
        public StorageService()
        {
            var storageAccount = CloudStorageAccount.Parse(_config["AzureWebJobsStorage"]); 
            var blobClient = storageAccount.CreateCloudBlobClient();  
            _container = blobClient.GetContainerReference("test-container");
            _container.CreateIfNotExistsAsync(); 
        }
        
        public async Task<ObjectResult> UploadFile(IFormFileCollection fileCollection)
        {
            if (fileCollection.Count <= 0) return new BadRequestObjectResult("No file added.");
            
            foreach (var file in fileCollection)
            {
                _blob = _container.GetBlockBlobReference(file.FileName);
                _blob.Properties.ContentType = file.ContentType;
            
                await _blob.UploadFromStreamAsync(file.OpenReadStream()); 
                
                await _blob.SetPropertiesAsync();
            }
            
            return new OkObjectResult($"File(s) added successfully to storage.");
        }

        /***
        private static CloudStorageAccount GetCloudStorageAccount()  
        {  
            var config = new ConfigurationBuilder()  
                .SetBasePath(Environment.CurrentDirectory)  
                .AddJsonFile("local.settings.json", true, true)  
                .AddEnvironmentVariables().Build();  
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(config["AzureWebJobsStorage"]);  
            return storageAccount;  
        }
        */
    }
}