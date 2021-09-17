using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace TestAzureFunction.Services
{
    public class StorageService : IStorageService
    {
        private CloudBlockBlob _blob;
        private CloudStorageAccount _storageAccount;
        private CloudBlobClient _blobClient;
        private readonly CloudBlobContainer _container;
        
        public StorageService()
        {
            _storageAccount = GetCloudStorageAccount();
            _blobClient = _storageAccount.CreateCloudBlobClient();  
            _container = _blobClient.GetContainerReference("test-container");
            CreateContainerIfNotExists();
        }
        
        public async Task ConfigureStorageAndUploadFiles(IFormFileCollection fileCollection)
        {
            for (var i = 0; i < fileCollection.Count; i++)
            {
                var file = fileCollection[i];
                
                _blob = _container.GetBlockBlobReference(file.FileName);
                _blob.Properties.ContentType = file.ContentType;
            
                await _blob.UploadFromStreamAsync(file.OpenReadStream()); 
                
                await _blob.SetPropertiesAsync();
            }
        }
        
        private void CreateContainerIfNotExists()  
        {  
            _storageAccount = GetCloudStorageAccount();  
            _blobClient = _storageAccount.CreateCloudBlobClient();  
            var containers = new string[] { "test-container" };  
            foreach (var item in containers)  
            {  
                CloudBlobContainer blobContainer = _blobClient.GetContainerReference(item);  
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