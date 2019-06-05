using Microsoft.Azure.WebJobs;

namespace FunctionApp62
{
    public static class DurableEntityProxyExtensions
    {
        public static TProxy CreateEntityProxy<TProxy>(this IDurableOrchestrationContext context, string entityKey)
        {
            return EntityProxyFactory.Create<TProxy>(context, entityKey);
        }
    }
}