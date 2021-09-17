using System;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.WindowsAzure.Storage;
using TestAzureFunction.Services;

[assembly:FunctionsStartup(typeof(TestAzureFunction.StartupClass))]
namespace TestAzureFunction
{
    public class StartupClass : FunctionsStartup
    {

        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton<IStorageService, StorageService>();
        }
        
    }
}