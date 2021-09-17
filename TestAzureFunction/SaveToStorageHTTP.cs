﻿using System;
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
using TestAzureFunction.Services;

namespace TestAzureFunction
{
    public class SaveToStorageHttpTrigger
    {
        
        private readonly IStorageService _storageService;

        public SaveToStorageHttpTrigger(IStorageService storageService)
        {
            _storageService = storageService;
        }
        
        [FunctionName("SaveToStorageHTTP")]
        public async Task<IActionResult> RunAsync(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "upload/file")] HttpRequest req,
            ILogger log, ExecutionContext context)
        {
            if (req.Form.Files.Count <= 0) return new BadRequestObjectResult("No file added.");
            await _storageService.ConfigureStorageAndUploadFiles(req.Form.Files);
               
            return new OkObjectResult($"File(s) added successfully to storage.");

        }
    }
}