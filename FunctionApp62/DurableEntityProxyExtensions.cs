using Microsoft.Azure.WebJobs;

namespace FunctionApp62
{
    public static class DurableEntityProxyExtensions
    {
        public static TEntity CreateEntityProxy<TEntity>(this IDurableOrchestrationContext context, string entityKey)
        {
            return EntityProxyFactory.Create<TEntity>(context, entityKey);
        }
    }
}