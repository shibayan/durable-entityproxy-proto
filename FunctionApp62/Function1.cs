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
            var proxy = context.CreateEntityProxy<ICounterEntity>("game1");

            var value1 = await proxy.Add(100);

            log.LogWarning($"Add : {value1}");

            var value2 = await proxy.Sub(50);

            log.LogWarning($"Sub : {value2}");

            await proxy.Reset();
        }
    }
}
