using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace TestAzureFunction.Services
{
    public class StorageService : IStorageService
    {
        public async Task ConfigureStorageAndUploadFiles(IFormFileCollection fileCollection)
        {
            CloudStorageAccount storageAccount = GetCloudStorageAccount();
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();  
            CloudBlobContainer container = blobClient.GetContainerReference("test-container");
            CreateContainerIfNotExists();
                
                
            for (int i = 0; i < fileCollection.Count; i++)
            {
                var file = fileCollection[i];
                    
                //Console.WriteLine(file.FileName);
            
                CloudBlockBlob blob = container.GetBlockBlobReference(file.FileName);
                blob.Properties.ContentType = file.ContentType;
            
                await blob.UploadFromStreamAsync(file.OpenReadStream()); 
                
                await blob.SetPropertiesAsync();
            }
        }
        
        private static void CreateContainerIfNotExists()  
        {  
            CloudStorageAccount storageAccount = GetCloudStorageAccount();  
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();  
            string[] containers = new string[] { "test-container" };  
            foreach (var item in containers)  
            {  
                CloudBlobContainer blobContainer = blobClient.GetContainerReference(item);  
                blobContainer.CreateIfNotExistsAsync();  
            }  
        }  
  
        private static CloudStorageAccount GetCloudStorageAccount()  
        {  
            var config = new ConfigurationBuilder()  
                .SetBasePath(Environment.CurrentDirectory)  
                .AddJsonFile("local.settings.json", true, true)  
                .AddEnvironmentVariables().Build();  
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(config["AzureWebJobsStorage"]);  
            return storageAccount;  
        }
    }
}