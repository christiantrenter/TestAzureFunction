using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace TestAzureFunction
{
    public static class TestingTimer
    {
        static SomeClass someClass = new SomeClass();
        
        [FunctionName("TestingTimer")]
        public static async Task RunAsync([TimerTrigger("*/5 */1 * * * *")] TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.UtcNow}");

            var something = await TestAsyncAwaitTask();
            log.LogInformation(something.ToString());

        }
        
        private static async Task<int> TestAsyncAwaitTask()
        {
            Task<int> t = new Task<int>(() => someClass.getNumber());
            t.Start();
            return await t;
        }
    }
    
}