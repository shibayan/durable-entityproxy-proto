using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace FunctionApp62
{
    public static class Function2
    {
        [FunctionName("Function2")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]
            HttpRequest req,
            [OrchestrationClient] IDurableOrchestrationClient client,
            ILogger log)
        {
            var proxy = client.CreateEntityProxy<ICounterEntity>(new EntityId(nameof(CounterEntity), "game1"));

            // use explicit flush
            proxy.Entity.Set(50);

            proxy.Entity.Add(200);

            await proxy.FlushAsync();

            // or
            await proxy.BatchAsync(x =>
            {
                x.Set(50);

                x.Set(100);
            });

            return new OkResult();
        }
    }
}
