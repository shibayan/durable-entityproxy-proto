using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace FunctionApp62
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]
            HttpRequest req,
            [OrchestrationClient] IDurableOrchestrationClient client,
            ILogger log)
        {
            await client.StartNewAsync("OrchestratorStart", null);

            return new OkResult();
        }

        [FunctionName("OrchestratorStart")]
        public static async Task OrchestratorStart([OrchestrationTrigger] IDurableOrchestrationContext context, ILogger log)
        {
            var proxy = context.CreateEntityProxy<ICounterEntity>(new EntityId(nameof(CounterEntity), "game1"));

            proxy.Entity.Set(50);

            proxy.Entity.Increment();

            proxy.Entity.Add(100);

            var value = await proxy.Entity.Get();

            log.LogWarning($"Value : {value}");

            //await proxy.Reset();
        }
    }
}
